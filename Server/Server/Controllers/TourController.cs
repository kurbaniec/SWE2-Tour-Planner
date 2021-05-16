using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Model;
using Newtonsoft.Json;
using Server.BL;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Logging;
using WebService_Lib.Server;

namespace Server.Controllers
{
    /// <summary>
    /// <c>Controller</c> class that manages all Client requests.
    /// </summary>
    [Controller]
    public class TourController
    {
        [Autowired] private readonly TourPlannerServer tp = null!;
        private readonly ILogger logger = WebServiceLogging.CreateLogger<TourController>();

        /// <summary>
        /// Endpoint used to query all Tours.
        /// </summary>
        /// <returns>
        /// JSON representation of all Tours on valid requests.
        /// Else Plaintext response with error message with an unsuccessful status code.
        /// </returns>
        [Get("/api/tours")]
        public Response GetTours()
        {
            try
            {
                var (tours, errorMsg) = tp.GetTours();
                if (tours is null) return Response.PlainText(errorMsg, Status.BadRequest);
                string jsonString = JsonConvert.SerializeObject(tours);
                return Response.Json(jsonString);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, $"Encountered exception in {MethodBase.GetCurrentMethod()}:");
                logger.Log(LogLevel.Error, ex.StackTrace);
                return Response.PlainText("Invalid request", Status.BadRequest);
            }
        }

        /// <summary>
        /// Endpoint used to request Route Information Image.
        /// </summary>
        /// <param name="id">
        /// Id of the Tour of whom the image is requested. 
        /// </param>
        /// <returns>
        /// Image payload.
        /// Else Plaintext response with error message with an unsuccessful status code.
        /// </returns>
        [Get("/api/route")]
        public Response GetRouteImage(PathVariable<int> id)
        {
            if (id.Ok)
            {
                var (imagePath, _) = tp.GetRouteImage(id.Value!);
                if (imagePath is { } path)
                {
                    var response = Response.File(path);
                    if (response is { }) return response;
                }
            }

            return Response.PlainText("Invalid request", Status.BadRequest);
        }

        /// <summary>
        /// Endpoint used to add new Tours.
        /// </summary>
        /// <param name="json">
        /// JSON representation of the Tour to add.
        /// </param>
        /// <returns>
        /// JSON representation of the newly added Tour.
        /// Else Plaintext response with error message with an unsuccessful status code.
        /// </returns>
        [Post("/api/tour")]
        public Response AddTour([JsonString] string json)
        {
            try
            {
                // Parse received tour
                Tour tour;
                var parsedTour = JsonConvert.DeserializeObject<Tour>(json);
                if (parsedTour is { } actualTour)
                    tour = actualTour;
                else
                {
                    return Response.PlainText("Invalid payload", Status.BadRequest);
                }

                // Add tour
                var (newTour, errorMsg) = tp.AddTour(tour);
                // Check result
                if (newTour is null) return Response.PlainText(errorMsg, Status.BadRequest);
                var jsonString = JsonConvert.SerializeObject(newTour);
                return Response.Json(jsonString);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, $"Encountered exception in {MethodBase.GetCurrentMethod()}:");
                logger.Log(LogLevel.Error, ex.StackTrace);
                return Response.PlainText("Invalid payload", Status.BadRequest);
            }
        }

        /// <summary>
        /// Endpoint used to add multiple new Tours.
        /// </summary>
        /// <param name="json">
        /// JSON representation of the Tours to add.
        /// </param>
        /// <returns>
        /// JSON representation of the newly added Tours.
        /// Else Plaintext response with error message with an unsuccessful status code.
        /// </returns>
        [Post("/api/tours")]
        public Response AddTours([JsonString] string json)
        {
            try
            {
                // Parse received tour
                List<Tour> tours;
                var parsedTours = JsonConvert.DeserializeObject<List<Tour>>(json);
                if (parsedTours is { } actualTours)
                    tours = actualTours;
                else
                {
                    return Response.PlainText("Invalid payload", Status.BadRequest);
                }

                // Add tours
                var (newTour, errorMsg) = tp.AddTours(tours);
                // Check result
                if (newTour is null) return Response.PlainText(errorMsg, Status.BadRequest);
                var jsonString = JsonConvert.SerializeObject(newTour);
                return Response.Json(jsonString);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, $"Encountered exception in {MethodBase.GetCurrentMethod()}:");
                logger.Log(LogLevel.Error, ex.StackTrace);
                return Response.PlainText("Invalid payload", Status.BadRequest);
            }
        }

        /// <summary>
        /// Endpoint used to update Tours.
        /// </summary>
        /// <param name="json">
        /// JSON representation of the Tours to update.
        /// </param>
        /// <returns>
        /// JSON representation of the updated Tour.
        /// Else Plaintext response with error message with an unsuccessful status code.
        /// </returns>
        [Put("/api/tour")]
        public Response UpdateTour([JsonString] string json)
        {
            try
            {
                // Parse received tour
                Tour tour;
                var parsedTour = JsonConvert.DeserializeObject<Tour>(json);
                if (parsedTour is { } actualTour)
                    tour = actualTour;
                else
                {
                    return Response.PlainText("Invalid payload", Status.BadRequest);
                }

                // Update tour
                var (updatedTour, errorMsg) = tp.UpdateTour(tour);
                // Check result
                if (updatedTour is null) return Response.PlainText(errorMsg, Status.BadRequest);
                var jsonString = JsonConvert.SerializeObject(updatedTour);
                return Response.Json(jsonString);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, $"Encountered exception in {MethodBase.GetCurrentMethod()}:");
                logger.Log(LogLevel.Error, ex.StackTrace);
                return Response.PlainText("Invalid payload", Status.BadRequest);
            }
        }

        /// <summary>
        /// Endpoint used to delete Tours.
        /// </summary>
        /// <param name="id">
        /// Id of the Tour which should be deleted.
        /// </param>
        /// <returns>
        /// Successful status code with no content.
        /// Else Plaintext response with error message with an unsuccessful status code.
        /// </returns>
        [Delete("/api/tour")]
        public Response DeleteTour(PathVariable<int> id)
        {
            if (id.Ok)
            {
                var (isDeleted, errorMsg) = tp.DeleteTour(id.Value!);
                return isDeleted ? Response.Status(Status.NoContent) : Response.PlainText(errorMsg, Status.BadRequest);
            }

            return Response.PlainText("Not valid Route id given", Status.BadRequest);
        }

        /// <summary>
        /// Endpoint used to request pdf reports for a given Tour.
        /// </summary>
        /// <param name="id">
        /// Id of the Tour for which the report should be generated.
        /// </param>
        /// <returns>
        /// File payload.
        /// Else Plaintext response with error message with an unsuccessful status code.
        /// </returns>
        [Get("/api/export/full")]
        public Response GetPdfExportFull(PathVariable<int> id)
        {
            if (id.Ok)
            {
                var (filePath, errorMsg) = tp.GetPdfExport(id.Value!);
                if (filePath is { } path)
                {
                    var response = Response.File(path);
                    return response ?? Response.PlainText("Could not locate export", Status.InternalServerError);
                }

                return Response.PlainText(errorMsg, Status.BadRequest);
            }

            return Response.PlainText("Invalid request", Status.BadRequest);
        }

        /// <summary>
        /// Endpoint used to request pdf summary-reports for a given Tour.
        /// </summary>
        /// <param name="id">
        /// Id of the Tour for which the report should be generated.
        /// </param>
        /// <returns>
        /// File payload.
        /// Else Plaintext response with error message with an unsuccessful status code.
        /// </returns>
        [Get("/api/export/summary")]
        public Response GetPdfExportSummary(PathVariable<int> id)
        {
            if (id.Ok)
            {
                var (filePath, errorMsg) = tp.GetPdfExport(id.Value!, true);
                if (filePath is { } path)
                {
                    var response = Response.File(path);
                    return response ?? Response.PlainText("Could not locate export", Status.InternalServerError);
                }

                return Response.PlainText(errorMsg, Status.BadRequest);
            }

            return Response.PlainText("Invalid request", Status.BadRequest);
        }
    }
}