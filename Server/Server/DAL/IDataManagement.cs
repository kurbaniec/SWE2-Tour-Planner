using System.Collections.Generic;
using Model;

namespace Server.DAL
{
    /// <summary>
    /// Interface that describes method to interact with the TourPlanner database.
    /// </summary>
    public interface IDataManagement
    {
        /// <summary>
        /// Query all Tours from the database.
        /// </summary>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the queried Tours as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        (List<Tour>?, string) GetTours();

        /// <summary>
        /// Query specific Tours from the database.
        /// </summary>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the queried Tour as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        (Tour?, string) GetTour(int id);

        /// <summary>
        /// Adds a new Tour in the database.
        /// </summary>
        /// <param name="tour">
        /// Tour which should be added.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the added Tour as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        (Tour?, string) AddTour(Tour tour);

        /// <summary>
        /// Updates an existing Tour in the database.
        /// </summary>
        /// <param name="tour">
        /// Tour which should be updated.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the updated Tour as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        (Tour?, string) UpdateTour(Tour tour);

        /// <summary>
        /// Deletes an existing Tour in the database.
        /// </summary>
        /// <param name="id">
        /// The id of the Tour which should be deleted.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the true as item1 and a empty string as item2 is returned.
        /// On failure item1 is false and item2 will contain the error message.
        /// </returns>
        (bool, string) DeleteTour(int id);
    }
}