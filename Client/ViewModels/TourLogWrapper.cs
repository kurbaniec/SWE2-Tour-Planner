using System;
using Model;
using Type = Model.Type;

namespace Client.ViewModels
{
    public class TourLogWrapper : BaseViewModel
    {
        private readonly TourLog log;
        public TourLog Model => log;

        private DateTime date;

        public DateTime Date
        {
            get => date;
            set
            {
                if (date == value) return;
                date = value;
                OnPropertyChanged();
            }
        }

        private Type type;

        public Type Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan duration;

        public TimeSpan Duration
        {
            get => duration;
            set
            {
                if (duration == value) return;
                duration = value;
                OnPropertyChanged();
            }
        }

        private int distance;

        public int Distance
        {
            get => distance;
            set
            {
                if (distance == value) return;
                distance = value;
                OnPropertyChanged();
            }
        }

        private int rating;

        public int Rating
        {
            get => rating;
            set
            {
                if (rating == value) return;
                rating = value;
                OnPropertyChanged();
            }
        }

        private string report;

        public string Report
        {
            get => report;
            set
            {
                if (report == value) return;
                report = value;
                OnPropertyChanged();
            }
        }

        private double avgSpeed;

        public double AvgSpeed
        {
            get => avgSpeed;
            set
            {
                if (Math.Abs(avgSpeed - value) < 0.001) return;
                avgSpeed = value;
                OnPropertyChanged();
            }
        }

        private double maxSpeed;

        public double MaxSpeed
        {
            get => maxSpeed;
            set
            {
                if (Math.Abs(maxSpeed - value) < 0.001) return;
                maxSpeed = value;
                OnPropertyChanged();
            }
        }

        private double heightDifference;

        public double HeightDifference
        {
            get => heightDifference;
            set
            {
                if (Math.Abs(heightDifference - value) < 0.001) return;
                heightDifference = value;
                OnPropertyChanged();
            }
        }

        private int stops;

        public int Stops
        {
            get => stops;
            set
            {
                if (stops == value) return;
                stops = value;
                OnPropertyChanged();
            }
        }

        public TourLogWrapper(TourLog log)
        {
            this.log = log;
            date = log.Date;
            type = log.Type;
            duration = log.Duration;
            distance = log.Distance;
            rating = log.Rating;
            report = log.Report;
            avgSpeed = log.AvgSpeed;
            maxSpeed = log.MaxSpeed;
            heightDifference = log.HeightDifference;
            stops = log.Stops;
        }

        public TourLogWrapper() : this(
            new TourLog(
                DateTime.Today, Type.Car, TimeSpan.FromHours(1), 10, 10,
                "Report goes here...", 10.0,
                20.0, 100, 0
            )
        )
        {
        }

        public TourLog GetRequestTourLog()
        {
            return new TourLog(date, type, duration, distance, rating, report, avgSpeed, maxSpeed, heightDifference,
                stops);
        }

        public void SaveChanges()
        {
            log.Date = date;
            log.Type = type;
            log.Duration = duration;
            log.Distance = distance;
            log.Rating = rating;
            log.Report = report;
            log.AvgSpeed = avgSpeed;
            log.MaxSpeed = maxSpeed;
            log.HeightDifference = heightDifference;
            log.Stops = stops;
        }

        public void DiscardChanges()
        {
            Date = log.Date;
            Type = log.Type;
            Duration = log.Duration;
            Distance = log.Distance;
            Rating = log.Rating;
            Report = log.Report;
            AvgSpeed = log.AvgSpeed;
            MaxSpeed = log.MaxSpeed;
            HeightDifference = log.HeightDifference;
            Stops = log.Stops;
        }
    }
}