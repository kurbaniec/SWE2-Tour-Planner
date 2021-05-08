using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Microsoft.Extensions.Logging;
using Model;
using Npgsql;
using NpgsqlTypes;
using Server.Setup;
using WebService_Lib.Attributes;
using WebService_Lib.Logging;

namespace Server.DAL
{
    [Component]
    public class PostgresDb : IDataManagement
    {
        [Autowired]
        private Configuration Cfg
        {
            set
            {
                connString = value.PostgresConnString;
                CreateDatabaseIfNotExists();
            }
        }

        private string connString = null!;
        private readonly ILogger logger = WebServiceLogging.CreateLogger<IDataManagement>();

        public (List<Tour>?, string) GetTours()
        {
            try
            {
                var tours = new List<Tour>();
                using var conn = Connection();
                using var cmdTours = new NpgsqlCommand("SELECT * from tour", conn);
                var dr = cmdTours.ExecuteReader();
                while (dr.Read())
                {
                    var id = dr.GetInt32(0);
                    var from = dr.GetString(1);
                    var to = dr.GetString(2);
                    var name = dr.GetString(3);
                    var distance = dr.GetDouble(4);
                    var description = dr.GetTextReader(5).ReadToEnd();
                    tours.Add(new Tour(id, from, to, name, distance, description, new List<TourLog>()));
                }

                dr.Close();

                foreach (var tour in tours)
                {
                    var logs = new List<TourLog>();
                    using var cmdLogs = new NpgsqlCommand(@"
                        SELECT * from tourlog
                        WHERE tour = @p",
                        conn);
                    cmdLogs.Parameters.AddWithValue("p", NpgsqlDbType.Integer, tour.Id);
                    var drLogs = cmdLogs.ExecuteReader();
                    while (drLogs.Read())
                    {
                        var logId = drLogs.GetInt32(0);
                        var date = drLogs.GetDateTime(2);
                        var type = Enum.Parse<Model.Type>(drLogs.GetString(3));
                        var duration = (TimeSpan) drLogs.GetInterval(4);
                        var logDistance = drLogs.GetDouble(5);
                        var rating = drLogs.GetInt32(6);
                        var report = drLogs.GetTextReader(7).ReadToEnd();
                        var avgSpeed = drLogs.GetDouble(8);
                        var maxSpeed = drLogs.GetDouble(9);
                        var heightDifference = drLogs.GetDouble(10);
                        var stops = drLogs.GetInt32(11);
                        logs.Add(new TourLog(logId, date, type, duration, logDistance, rating, report, avgSpeed,
                            maxSpeed, heightDifference, stops));
                    }

                    tour.Logs = logs;
                    drLogs.Close();
                }

                return (tours, string.Empty);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Warning, ex.StackTrace);
                return (null, "Internal server error");
            }
        }

        public (Tour?, string) GetTour(int id)
        {
            try
            {
                using var conn = Connection();
                using var cmdTour = new NpgsqlCommand(@"
                    SELECT * from tour 
                    WHERE id = @p",
                    conn);
                cmdTour.Parameters.AddWithValue("p", NpgsqlDbType.Integer, id);
                cmdTour.Parameters.Add(new NpgsqlParameter("source", NpgsqlDbType.Varchar)
                    {Direction = ParameterDirection.Output});
                cmdTour.Parameters.Add(new NpgsqlParameter("destination", NpgsqlDbType.Varchar)
                    {Direction = ParameterDirection.Output});
                cmdTour.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar)
                    {Direction = ParameterDirection.Output});
                cmdTour.Parameters.Add(new NpgsqlParameter("distance", NpgsqlDbType.Double)
                    {Direction = ParameterDirection.Output});
                cmdTour.Parameters.Add(new NpgsqlParameter("description", NpgsqlDbType.Text)
                    {Direction = ParameterDirection.Output});
                cmdTour.ExecuteNonQuery();
                if (cmdTour.Parameters[1].Value is string from &&
                    cmdTour.Parameters[2].Value is string to &&
                    cmdTour.Parameters[3].Value is string name &&
                    cmdTour.Parameters[4].Value is double distance &&
                    cmdTour.Parameters[5].Value is string description)
                {
                    var logs = new List<TourLog>();
                    using var cmdLogs = new NpgsqlCommand(@"
                        SELECT * from tourlog
                        WHERE tour = @p",
                        conn);
                    cmdLogs.Parameters.AddWithValue("p", NpgsqlDbType.Integer, id);
                    using var dr = cmdLogs.ExecuteReader();
                    while (dr.Read())
                    {
                        var logId = dr.GetInt32(0);
                        var date = dr.GetDateTime(2);
                        var type = Enum.Parse<Model.Type>(dr.GetString(3));
                        var duration = (TimeSpan) dr.GetInterval(4);
                        var logDistance = dr.GetDouble(5);
                        var rating = dr.GetInt32(6);
                        var report = dr.GetTextReader(7).ReadToEnd();
                        var avgSpeed = dr.GetDouble(8);
                        var maxSpeed = dr.GetDouble(9);
                        var heightDifference = dr.GetDouble(10);
                        var stops = dr.GetInt32(11);
                        logs.Add(new TourLog(logId, date, type, duration, logDistance, rating, report, avgSpeed,
                            maxSpeed, heightDifference, stops));
                    }

                    return (new Tour(id, from, to, name, distance, description, logs), string.Empty);
                }

                return (null, "Invalid id given");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Warning, ex.StackTrace);
                return (null, "Internal server error");
            }
        }

        public (Tour?, string) AddTour(Tour tour)
        {
            NpgsqlTransaction? transaction = null;
            try
            {
                using var conn = Connection();
                transaction = BeginTransaction(conn);
                if (transaction == null) return (null, "Could not start new transaction");
                // Add tour and retrieve id
                // See: https://stackoverflow.com/a/5765441/12347616
                // Also use DEFAULT "value" to trigger Serial
                // See: https://www.postgresqltutorial.com/postgresql-serial/
                using var tourCmd = new NpgsqlCommand(@"
                    INSERT INTO tour VALUES(DEFAULT, @p0, @p1, @p2, @p3, @p4) 
                    RETURNING id",
                    conn);
                tourCmd.Parameters.AddWithValue("p0", NpgsqlDbType.Varchar, tour.From);
                tourCmd.Parameters.AddWithValue("p1", NpgsqlDbType.Varchar, tour.To);
                tourCmd.Parameters.AddWithValue("p2", NpgsqlDbType.Varchar, tour.Name);
                tourCmd.Parameters.AddWithValue("p3", NpgsqlDbType.Double, tour.Distance);
                tourCmd.Parameters.AddWithValue("p4", NpgsqlDbType.Text, tour.Description);
                // Get inserted id
                // See: https://stackoverflow.com/a/20758425/12347616
                var tourCmdId = tourCmd.ExecuteScalar();
                if (tourCmdId is int tourId)
                {
                    tour.Id = tourId;
                    foreach (var log in tour.Logs)
                    {
                        using var logCmd = new NpgsqlCommand(@"
                            INSERT INTO tourlog VALUES(
                                DEFAULT, @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10
                            ) RETURNING id",
                            conn);
                        logCmd.Parameters.AddWithValue("p0", NpgsqlDbType.Integer, tourId);
                        logCmd.Parameters.AddWithValue("p1", NpgsqlDbType.Date, log.Date);
                        logCmd.Parameters.AddWithValue("p2", NpgsqlDbType.Varchar, log.Type.ToString());
                        logCmd.Parameters.AddWithValue("p3", NpgsqlDbType.Interval, log.Duration);
                        logCmd.Parameters.AddWithValue("p4", NpgsqlDbType.Double, log.Distance);
                        logCmd.Parameters.AddWithValue("p5", NpgsqlDbType.Integer, log.Rating);
                        logCmd.Parameters.AddWithValue("p6", NpgsqlDbType.Text, log.Report);
                        logCmd.Parameters.AddWithValue("p7", NpgsqlDbType.Double, log.AvgSpeed);
                        logCmd.Parameters.AddWithValue("p8", NpgsqlDbType.Double, log.MaxSpeed);
                        logCmd.Parameters.AddWithValue("p9", NpgsqlDbType.Double, log.HeightDifference);
                        logCmd.Parameters.AddWithValue("p10", NpgsqlDbType.Integer, log.Stops);
                        var logCmdId = logCmd.ExecuteScalar();
                        if (logCmdId is int logId)
                        {
                            log.Id = logId;
                        }
                        else
                        {
                            transaction.Rollback();
                            return (null, "Internal server error");
                        }
                    }

                    transaction.Commit();
                    return (tour, string.Empty);
                }

                transaction.Rollback();
                return (null, "Internal server error");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Warning, ex.StackTrace);
                transaction?.Rollback();
                return (null, "Internal server error");
            }
        }

        public (Tour?, string) UpdateTour(Tour tour)
        {
            NpgsqlTransaction? transaction = null;
            try
            {
                using var conn = Connection();
                transaction = BeginTransaction(conn);
                if (transaction == null) return (null, "Could not start new transaction");
                // Remove tour (which also removes all related logs)
                using var deleteCmd = new NpgsqlCommand(@"
                    DELETE FROM tour WHERE id=@p",
                    conn);
                deleteCmd.Parameters.AddWithValue("p", NpgsqlDbType.Integer, tour.Id);
                deleteCmd.ExecuteNonQuery();
                // Add tour (with id)
                using var tourCmd = new NpgsqlCommand(@"
                    INSERT INTO tour VALUES(@pid, @p0, @p1, @p2, @p3, @p4)",
                    conn);
                tourCmd.Parameters.AddWithValue("pid", NpgsqlDbType.Integer, tour.Id);
                tourCmd.Parameters.AddWithValue("p0", NpgsqlDbType.Varchar, tour.From);
                tourCmd.Parameters.AddWithValue("p1", NpgsqlDbType.Varchar, tour.To);
                tourCmd.Parameters.AddWithValue("p2", NpgsqlDbType.Varchar, tour.Name);
                tourCmd.Parameters.AddWithValue("p3", NpgsqlDbType.Double, tour.Distance);
                tourCmd.Parameters.AddWithValue("p4", NpgsqlDbType.Text, tour.Description);
                tourCmd.ExecuteNonQuery();
                // Add logs
                foreach (var log in tour.Logs)
                {
                    using var logCmd = new NpgsqlCommand(@"
                        INSERT INTO tourlog VALUES(
                            DEFAULT, @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10
                        ) RETURNING id",
                        conn);
                    logCmd.Parameters.AddWithValue("p0", NpgsqlDbType.Integer, tour.Id);
                    logCmd.Parameters.AddWithValue("p1", NpgsqlDbType.Date, log.Date);
                    logCmd.Parameters.AddWithValue("p2", NpgsqlDbType.Varchar, log.Type.ToString());
                    logCmd.Parameters.AddWithValue("p3", NpgsqlDbType.Interval, log.Duration);
                    logCmd.Parameters.AddWithValue("p4", NpgsqlDbType.Double, log.Distance);
                    logCmd.Parameters.AddWithValue("p5", NpgsqlDbType.Integer, log.Rating);
                    logCmd.Parameters.AddWithValue("p6", NpgsqlDbType.Text, log.Report);
                    logCmd.Parameters.AddWithValue("p7", NpgsqlDbType.Double, log.AvgSpeed);
                    logCmd.Parameters.AddWithValue("p8", NpgsqlDbType.Double, log.MaxSpeed);
                    logCmd.Parameters.AddWithValue("p9", NpgsqlDbType.Double, log.HeightDifference);
                    logCmd.Parameters.AddWithValue("p10", NpgsqlDbType.Integer, log.Stops);
                    var logCmdId = logCmd.ExecuteScalar();
                    if (logCmdId is int logId)
                    {
                        log.Id = logId;
                    }
                    else
                    {
                        transaction.Rollback();
                        return (null, "Internal server error");
                    }
                }

                transaction.Commit();
                return (tour, string.Empty);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Warning, ex.StackTrace);
                transaction?.Rollback();
                return (null, "Internal server error");
            }
        }

        public (bool, string) DeleteTour(int id)
        {
            NpgsqlTransaction? transaction = null;
            try
            {
                using var conn = Connection();
                transaction = BeginTransaction(conn);
                if (transaction == null) return (false, "Could not start new transaction");

                using var cmd = new NpgsqlCommand(@"
                    DELETE from tour WHERE id=@p0",
                    conn);
                cmd.Parameters.AddWithValue("p0", NpgsqlDbType.Integer, id);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Warning, ex.StackTrace);
                transaction?.Rollback();
                return (false, "Internal server error");
            }
        }

        private void CreateDatabaseIfNotExists()
        {
            try
            {
                var conn = Connection();
                using var cmdChek = new NpgsqlCommand("SELECT 1 FROM pg_database WHERE datname='tp'", conn);
                var dbExists = cmdChek.ExecuteScalar() != null;
                if (dbExists)
                {
                    // Add databases to connString
                    connString += "Database=tp;";
                    return;
                }

                // Create databases
                using (var cmd = new NpgsqlCommand(@"
                    CREATE DATABASE tp
                        WITH OWNER = postgres
                        ENCODING = 'UTF8'
                ", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Add databases to connString
                conn.Close();
                connString += "Database=tp;";
                conn = Connection();
                // Create tables
                using (var cmd = new NpgsqlCommand(@"
                    Create TABLE IF NOT EXISTS tour(
                        id SERIAL,
                        source VARCHAR(256) NOT NULL,
                        destination VARCHAR(256) NOT NULL,
                        name VARCHAR(256) NOT NULL,
                        distance DOUBLE PRECISION NOT NULL,
                        description TEXT NOT NULL,
                        PRIMARY KEY(id)
                    )
                ", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new NpgsqlCommand(@"
                    Create TABLE IF NOT EXISTS tourlog(
                        id SERIAL,
                        tour INTEGER NOT NULL,
                        date DATE NOT NULL,
                        type VARCHAR(256) NOT NULL,
                        duration INTERVAL NOT NULL,
                        distance DOUBLE PRECISION NOT NULL,
                        rating INTEGER NOT NULL,
                        report TEXT NOT NULL,
                        avgspeed DOUBLE PRECISION NOT NULL,
                        maxspeed DOUBLE PRECISION NOT NULL,
                        heightdifference DOUBLE PRECISION NOT NULL,
                        stops INTEGER NOT NULL,
                        PRIMARY KEY(id),
                        CONSTRAINT fk_tour
                            FOREIGN KEY(tour) 
                                REFERENCES tour(id)
                                ON DELETE CASCADE 
                    )
                ", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "Could not setup database:");
                logger.Log(LogLevel.Error, ex.StackTrace);
                throw new ApplicationException("Please check your database configuration & health status");
            }
        }

        /// <summary>
        /// Try to start a new transaction.
        /// </summary>
        /// <returns>
        /// Returns a <c>NpgsqlTransaction</c> when a new transaction can be started or
        /// null when not.
        /// </returns>
        private NpgsqlTransaction? BeginTransaction(NpgsqlConnection conn)
        {
            // Try to start new transaction
            for (var i = 0; i < 15; i++)
            {
                try
                {
                    var transaction = conn.BeginTransaction();
                    return transaction;
                }
                catch (InvalidOperationException)
                {
                    // Try again later...
                    Thread.Sleep(50);
                }
            }

            return null;
        }

        /// <summary>
        /// Return a new connection to the Postgres database.
        /// </summary>
        /// <returns>
        /// Opened connection to the Postgres Database
        /// </returns>
        private NpgsqlConnection Connection()
        {
            var conn = new NpgsqlConnection(connString);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// Shorthand function for:
        ///     transaction.Rollback();
        ///     return false;
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>
        /// Always returns false.
        /// </returns>
        private static bool Rollback(NpgsqlTransaction transaction)
        {
            transaction.Rollback();
            return false;
        }
    }
}