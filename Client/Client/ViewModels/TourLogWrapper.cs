using System;
using Model;
using Type = Model.Type;

namespace Client.ViewModels
{
    public class TourLogWrapper : BaseViewModel
    {
        private readonly TourLog log;
        public TourLog Model => log;

        private DateTime? date;

        public DateTime? Date
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

        private TimeSpan? duration;

        public TimeSpan? Duration
        {
            get => duration;
            set
            {
                if (duration == value) return;
                duration = value;
                OnPropertyChanged();
            }
        }

        private double? distance;

        public double? Distance
        {
            get => distance;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (distance == value) return;
                distance = value;
                OnPropertyChanged();
            }
        }

        private Rating rating;

        public Rating Rating
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

        private double? avgSpeed;

        public double? AvgSpeed
        {
            get => avgSpeed;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (avgSpeed == value) return;
                avgSpeed = value;
                OnPropertyChanged();
            }
        }

        private double? maxSpeed;

        public double? MaxSpeed
        {
            get => maxSpeed;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (maxSpeed == value) return;
                maxSpeed = value;
                OnPropertyChanged();
            }
        }

        private double? heightDifference;

        public double? HeightDifference
        {
            get => heightDifference;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (heightDifference == value) return;
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

        private bool isValid = true;

        public bool IsValid
        {
            get => isValid;
            set
            {
                // Why negation?
                // HasValidationErrors = false => IsValid = true
                isValid = !value;
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
                DateTime.Today, Type.Car, TimeSpan.FromHours(1), 10.0, Rating.Good,
                "Report goes here...", 10.0,
                20.0, 100, 0
            )
        )
        {
        }

        public TourLog GetRequestTourLog()
        {
            return new(log.Id, (DateTime) date!, type, (TimeSpan) duration!, (int) distance!, rating, report,
                (double) avgSpeed!, (double) maxSpeed!, (double) heightDifference!,
                stops);
        }

        public void SaveChanges()
        {
            log.Date = (DateTime) date!;
            log.Type = type;
            log.Duration = (TimeSpan) duration!;
            log.Distance = (int) distance!;
            log.Rating = rating;
            log.Report = report;
            log.AvgSpeed = (double) avgSpeed!;
            log.MaxSpeed = (double) maxSpeed!;
            log.HeightDifference = (double) heightDifference!;
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