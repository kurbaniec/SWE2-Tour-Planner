using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Model;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Server.Setup;
using WebService_Lib.Attributes;
using WebService_Lib.Logging;

namespace Server.DAL
{
    [Component]
    public class PdfExportHandler : IExportHandler
    {
        [Autowired] private readonly Configuration cfg = null!;
        private string ExportPath => cfg.ExportPath;
        private readonly ILogger logger = WebServiceLogging.CreateLogger<IExportHandler>();

        public (string?, string) Export(Tour tour, string? imagePath, bool isSummary = false)
        {
            try
            {
                // Create pdf with Quest PDF
                // See: https://www.questpdf.com/documentation/getting-started.html
                var document = new TourReport(tour, imagePath, isSummary);
                var filePath = $"{ExportPath}{Path.DirectorySeparatorChar}{tour.Id}-{Guid.NewGuid()}";
                document.GeneratePdf(filePath);
                logger.Log(LogLevel.Information, 
                    $"Successfully created export \"{filePath}\" for Tour with id {tour.Id}");
                return (filePath, string.Empty);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Warning, ex.StackTrace);
                return (null, "Could not generate export");
            }
        }

        public class TourLogsSummary
        {
            public List<DateTime> Dates { get; set; }
            public List<Model.Type> UsedTypes { get; set; }
            public double OverallDistance { set; get; }
            public double AvgRating { get; set; }
            public double AvgSpeed { set; get; }
            public double MaxSpeed { set; get; }
            public double AvgHeightDifference { set; get; }
            public double AvgStops { set; get; }
        }

        public class TourReport : IDocument
        {
            public Tour Model { get; }
            public string? ImagePath { get; }
            public bool IsSummary { get; }


            public TourReport(Tour model, string? imagePath, bool isSummary = false)
            {
                Model = model;
                ImagePath = imagePath;
                IsSummary = isSummary;
            }

            public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

            public void Compose(IContainer container)
            {
                container
                    .PaddingHorizontal(50)
                    .PaddingVertical(50)
                    .Page(page =>
                    {
                        page.Header(ComposeHeader);
                        page.Content(ComposeContent);
                        page.Footer().AlignCenter().PageNumber("Page {number}");
                    });
            }

            void ComposeHeader(IContainer container)
            {
                container.Row(row =>
                {
                    row.RelativeColumn().Stack(stack =>
                    {
                        stack.Element().Text(!IsSummary ? "Tour Report" : "Tour Report - Summary",
                            TextStyle.Default.Size(20));
                        stack.Element().Text($"Tour:  \"{Model.Name}\" | #{Model.Id}");
                        stack.Element().Text($"Issue date: {DateTime.Now:yyyy/MM/dd}");
                    });
                });
            }

            void ComposeContent(IContainer container)
            {
                container.PaddingVertical(40).PageableStack(stack =>
                {
                    stack.Spacing(5);

                    if (ImagePath != null)
                        stack.Element(ComposeRouteImage);

                    if (!IsSummary)
                    {
                        stack.Element().Text("Logs:");
                        stack.Element(ComposeReport);
                    }
                    else
                    {
                        stack.Element().Text("Summary:");
                        stack.Element(ComposeSummary);
                    }
                });
            }

            void ComposeRouteImage(IContainer container)
            {
                if (!File.Exists(ImagePath)) return;
                var imageData = File.ReadAllBytes(ImagePath!);
                container.Stack(stack =>
                {
                    // Add image
                    // See: https://www.questpdf.com/documentation/api-reference.html#static-images
                    stack.Element().Image(imageData, ImageScaling.FitArea);
                    stack.Spacing(5);
                });
            }

            void ComposeReport(IContainer container)
            {
                container.PaddingTop(10).Section(section =>
                {
                    // header
                    section.Header().BorderBottom(1).Padding(5).Row(row =>
                    {
                        row.ConstantColumn(25).Text("#");
                        row.RelativeColumn().Text("Date");
                        row.RelativeColumn().Text("Type");
                        row.RelativeColumn().Text("Duration");
                        row.RelativeColumn().Text("Distance");
                        row.RelativeColumn().Text("Rating");
                        row.RelativeColumn().Text("Avg. Speed");
                        row.RelativeColumn().Text("Max Speed");
                        row.RelativeColumn().Text("Height Diff.");
                        row.RelativeColumn().Text("Stops");
                        row.RelativeColumn(2).Text("Report");
                    });
                    
                    // content
                    section
                        .Content()
                        .PageableStack(stack =>
                        {
                            foreach (var log in Model.Logs)
                            {
                                stack.Element().BorderBottom(1).BorderColor("CCC").Padding(5).Row(row =>
                                {
                                    row.ConstantColumn(25).Text(log.Id);
                                    row.RelativeColumn().Text($"{log.Date:yyyy/MM/dd}");
                                    row.RelativeColumn().Text(log.Type);
                                    row.RelativeColumn().Text(log.Duration);
                                    row.RelativeColumn().Text(log.Distance);
                                    row.RelativeColumn().Text(log.Rating);
                                    row.RelativeColumn().Text(log.AvgSpeed);
                                    row.RelativeColumn().Text(log.MaxSpeed);
                                    row.RelativeColumn().Text(log.HeightDifference);
                                    row.RelativeColumn().Text(log.Stops);
                                    row.RelativeColumn(2).Text(log.Report);
                                });
                            }
                        });
                });
            }

            /**
             * public double OverallDistance { set; get; }
                public double AvgRating { get; set; }
                public double AvgSpeed { set; get; }
                public double MaxSpeed { set; get; }
                public double AvgHeightDifference { set; get; }
                public double AvgStops { set; get; }
             */
            void ComposeSummary(IContainer container)
            {
                var overallDistance = Model.Logs.Sum(log => log.Distance);
                // Sum TimeSpans
                // See: https://stackoverflow.com/a/4703056/12347616
                var overallDuration = new TimeSpan(Model.Logs.Sum(log => log.Duration.Ticks));
                var avgRating = Model.Logs.Sum(log => log.Rating) / (double) Model.Logs.Count;
                var avgSpeed = Model.Logs.Sum(log => log.AvgSpeed) / (double) Model.Logs.Count;
                var maxSpeed = Model.Logs.Max(log => log.MaxSpeed);
                var maxHeightDifference = Model.Logs.Max(log => log.HeightDifference);
                var avgStops = Model.Logs.Sum(log => log.Stops) / (double) Model.Logs.Count;
            }
        }
    }
}