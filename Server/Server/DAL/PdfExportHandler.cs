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
                    
                    stack.Element().Row(row =>
                    {
                        row.RelativeColumn().BorderBottom(1).PaddingBottom(5).Text("From");
                        row.RelativeColumn().BorderBottom(1).PaddingBottom(5).Text("To");
                        row.RelativeColumn().BorderBottom(1).PaddingBottom(5).Text("Distance");
                    });
                    stack.Element().Row(row =>
                    {
                        row.RelativeColumn().Text(Model.From);
                        row.RelativeColumn().Text(Model.To);
                        row.RelativeColumn().Text(Model.Distance);
                    });
                    if (!string.IsNullOrEmpty(Model.Description))
                    {
                        stack.Element().BorderBottom(1).PaddingBottom(5).Text("Description");
                        stack.Element().Text(Model.Description);
                    }

                    stack.Spacing(5);

                    if (!IsSummary)
                    {
                        stack.Element().PaddingTop(15).Text("Logs", TextStyle.Default.Size(15));
                        stack.Element(ComposeReport);
                    }
                    else
                    {
                        stack.Element().PaddingTop(15).Text("Summary", TextStyle.Default.Size(15));
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
                        row.ConstantColumn(25).Text("#", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Date", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Type", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Duration", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Distance", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Rating", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Avg. Speed", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Max Speed", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Height Diff.", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Stops", TextStyle.Default.Size(8));
                        row.RelativeColumn(2).Text("Report", TextStyle.Default.Size(8));
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
                                    row.ConstantColumn(25).Text(log.Id, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text($"{log.Date:yyyy/MM/dd}", TextStyle.Default.Size(10));
                                    row.RelativeColumn().Text(log.Type, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.Duration, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.Distance, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.Rating, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.AvgSpeed, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.MaxSpeed, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.HeightDifference, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.Stops, TextStyle.Default.Size(8));
                                    row.RelativeColumn(2).Text(log.Report, TextStyle.Default.Size(8));
                                });
                            }
                        });
                });
            }
            
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

                container.PaddingTop(10).Section(section =>
                {
                    // header
                    section.Header().BorderBottom(1).Padding(5).Row(row =>
                    {
                        row.RelativeColumn().Text("Overall distance", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Overall duration", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Avg. Rating", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Avg. Speed", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Max Speed", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Max Height Diff.", TextStyle.Default.Size(8));
                        row.RelativeColumn().Text("Avg. Stops", TextStyle.Default.Size(8));
                    });

                    // content
                    section.Content().PageableStack(stack =>
                    {
                        stack.Element().BorderBottom(1).BorderColor("CCC").Padding(5).Row(row =>
                        {
                            row.RelativeColumn().Text(overallDistance, TextStyle.Default.Size(8));
                            row.RelativeColumn().Text(overallDuration, TextStyle.Default.Size(8));
                            row.RelativeColumn().Text(avgRating, TextStyle.Default.Size(8));
                            row.RelativeColumn().Text(avgSpeed, TextStyle.Default.Size(8));
                            row.RelativeColumn().Text(maxSpeed, TextStyle.Default.Size(8));
                            row.RelativeColumn().Text(maxHeightDifference, TextStyle.Default.Size(8));
                            row.RelativeColumn().Text(avgStops, TextStyle.Default.Size(8));
                        });
                    });
                });
            }
        }
    }
}