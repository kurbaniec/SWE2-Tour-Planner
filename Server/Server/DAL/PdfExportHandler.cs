using System;
using System.Collections.Generic;
using System.IO;
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
                        stack.Element().Text($"Tour: {Model.Name}", TextStyle.Default.Size(20));
                        stack.Element().Text($"#{Model.Id}");
                        stack.Element().Text($"Issue date: {DateTime.Now}");
                    });

                    row.ConstantColumn(100).Height(50).Placeholder();
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
                        stack.Element(ComposeReport);

                    else
                        stack.Element(ComposeSummary);
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
            }

            void ComposeSummary(IContainer container)
            {
            }
        }
    }
}