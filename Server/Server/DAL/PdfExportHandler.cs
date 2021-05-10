using System;
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
                var filePath = $"{ExportPath}{Path.DirectorySeparatorChar}{tour.Id}-{Guid.NewGuid()}.pdf";
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

        private class TourReport : IDocument
        {
            private Tour Model { get; }
            private string? ImagePath { get; }
            private bool IsSummary { get; }


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

            private void ComposeHeader(IContainer container)
            {
                container.Row(row =>
                {
                    row.RelativeColumn().Stack(stack =>
                    {
                        stack.Element().Text(!IsSummary ? "Tour Report" : "Tour Report - Summary",
                            TextStyle.Default.Size(20));
                        stack.Element().Text($"\"{Model.Name}\" | #{Model.Id}");
                        stack.Element().Text($"{DateTime.Now:yyyy/MM/dd}");
                    });
                });
            }

            private void ComposeContent(IContainer container)
            {
                container.PaddingVertical(40).PageableStack(stack =>
                {
                    stack.Spacing(5);

                    stack.Element().Row(row =>
                    {
                        row.RelativeColumn(2).BorderBottom(1).Text("From");
                        row.ConstantColumn(30);
                        row.RelativeColumn(2).BorderBottom(1).Text("To");
                        row.ConstantColumn(30);
                        row.RelativeColumn().BorderBottom(1).Text("Distance");
                    });
                    stack.Element().Row(row =>
                    {
                        row.RelativeColumn(2).Text(Model.From);
                        row.ConstantColumn(30);
                        row.RelativeColumn(2).Text(Model.To);
                        row.ConstantColumn(30);
                        row.RelativeColumn().Text($"{Model.Distance} km");
                    });

                    if (!string.IsNullOrEmpty(Model.Description))
                    {
                        stack.Element().PaddingTop(12).BorderBottom(1).PaddingBottom(5).Text("Description");
                        stack.Element().Text(Model.Description);
                    }

                    if (ImagePath != null)
                    {
                        stack.Element(ComposeRouteImage);
                    }

                    if (!IsSummary)
                    {
                        stack.Element().PageBreak();
                        stack.Element(ComposeReport);
                    }
                    else
                    {
                        stack.Element(ComposeSummary);
                    }
                });
            }

            private void ComposeRouteImage(IContainer container)
            {
                if (!File.Exists(ImagePath)) return;
                var imageData = File.ReadAllBytes(ImagePath!);
                container.Stack(stack =>
                {
                    stack.Element().PaddingTop(12).BorderBottom(1).Text("Route Information");
                    // Add image
                    // See: https://www.questpdf.com/documentation/api-reference.html#static-images
                    stack.Element().PaddingTop(10).Image(imageData, ImageScaling.FitArea);
                });
            }

            private void ComposeReport(IContainer container)
            {
                container.Section(section =>
                {
                    // header
                    section.Header().Stack(stack =>
                    {
                        stack.Element().BorderBottom(1).Text("Logs");
                        stack.Element().BorderBottom(1).Padding(5).PaddingTop(10).Row(row =>
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
                        });
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
                                    row.RelativeColumn().Text($"{log.Date:yyyy/MM/dd}", TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.Type, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.Duration, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.Distance, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.Rating, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.AvgSpeed, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.MaxSpeed, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.HeightDifference, TextStyle.Default.Size(8));
                                    row.RelativeColumn().Text(log.Stops, TextStyle.Default.Size(8));
                                });
                                stack.Element().BorderBottom(1).BorderColor("CCC").Padding(5)
                                    .Text($"Report: {log.Report}", TextStyle.Default.Size(8));
                            }
                        });
                });
            }

            private void ComposeSummary(IContainer container)
            {
                var overallDistance = Model.Logs.Sum(log => log.Distance);
                // Sum TimeSpans
                // See: https://stackoverflow.com/a/4703056/12347616
                var overallDuration = new TimeSpan(Model.Logs.Sum(log => log.Duration.Ticks));
                // Get most often used rating
                // See: https://stackoverflow.com/q/28828407/12347616
                var avgRating = Model.Logs.GroupBy(log => log.Rating)
                    .OrderByDescending(r => r.Count())
                    .Select(rt => rt.Key).First();
                var avgSpeed = Model.Logs.Sum(log => log.AvgSpeed) / Model.Logs.Count;
                var maxSpeed = Model.Logs.Max(log => log.MaxSpeed);
                var maxHeightDifference = Model.Logs.Max(log => log.HeightDifference);
                var avgStops = Model.Logs.Sum(log => log.Stops) / (double) Model.Logs.Count;

                container.Section(section =>
                {
                    // header
                    section.Header().Stack(stack =>
                    {
                        stack.Element().PaddingTop(12).BorderBottom(1).Text("Summary");
                        stack.Element().BorderBottom(1).Padding(5).PaddingTop(10).Row(row =>
                        {
                            row.RelativeColumn().Text("Overall distance", TextStyle.Default.Size(8));
                            row.RelativeColumn().Text("Overall duration", TextStyle.Default.Size(8));
                            row.RelativeColumn().Text("Avg. Rating", TextStyle.Default.Size(8));
                            row.RelativeColumn().Text("Avg. Speed", TextStyle.Default.Size(8));
                            row.RelativeColumn().Text("Max Speed", TextStyle.Default.Size(8));
                            row.RelativeColumn().Text("Max Height Diff.", TextStyle.Default.Size(8));
                            row.RelativeColumn().Text("Avg. Stops", TextStyle.Default.Size(8));
                        });
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