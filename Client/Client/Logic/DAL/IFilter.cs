namespace Client.Logic.DAL
{
    /// <summary>
    /// Interfaces that defines a way to filter object in an <c>CollectionView</c>.
    /// </summary>
    public interface IFilter
    {
        string Filter { get; set; }
        
        /// <summary>
        /// Method that can be used to filter objects in an <c>CollectionView</c>.
        /// </summary>
        /// <param name="o">
        /// Object that will be checked with the filter criteria.
        /// </param>
        /// <returns>
        /// True, if the object should be displayed, else false.
        /// </returns>
        bool ApplyFilter(object o);
    }
}