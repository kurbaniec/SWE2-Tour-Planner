using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Model;
using Npgsql;
using Server.Setup;
using WebService_Lib.Attributes;
using WebService_Lib.Logging;

namespace Server.DAL
{
    //[Component]
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

        private string connString;
        private readonly ILogger logger = WebServiceLogging.CreateLogger<IDataManagement>();

        public List<Tour> GetTours()
        {
            throw new System.NotImplementedException();
        }

        public Tour? GetTour(int id)
        {
            throw new System.NotImplementedException();
        }

        public (Tour?, string) AddTour(Tour tour)
        {
            throw new System.NotImplementedException();
        }

        public (Tour?, string) UpdateTour(Tour tour)
        {
            throw new System.NotImplementedException();
        }

        public (bool, string) DeleteTour(int id)
        {
            throw new System.NotImplementedException();
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