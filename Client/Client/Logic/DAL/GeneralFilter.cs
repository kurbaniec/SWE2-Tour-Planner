﻿using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using Client.Utils.Logging;
using Microsoft.Extensions.Logging;

namespace Client.Logic.DAL
{
    /// <summary>
    /// Concrete implementation of <c>IFilter</c>.
    /// Uses a more general approach for filtering and is not only limited to Tours
    /// of type <c>TourWrapper</c>.
    /// </summary>
    public class GeneralFilter : IFilter
    {
        public string Filter { get; set; } = string.Empty;
        private readonly NumberFormatInfo nfi;
        private readonly ILogger logger = ApplicationLogging.CreateLogger<IFilter>();

        public GeneralFilter()
        {
            // WPF uses "." for decimal points so configure it for comparisons
            // See: https://stackoverflow.com/a/3135598/12347616
            // And: https://stackoverflow.com/a/3139803/12347616
            // And: https://stackoverflow.com/a/54024451/12347616
            nfi = new NumberFormatInfo {NumberDecimalSeparator = "."};
        }

        /// <summary>
        /// Method that can be used to filter objects in an <c>CollectionView</c>.
        /// </summary>
        /// <param name="o">
        /// Object that will be checked with the filter criteria.
        /// </param>
        /// <returns>
        /// True, if the object should be displayed, else false.
        /// </returns>
        public bool ApplyFilter(object o)
        {
            try
            {
                if (string.IsNullOrEmpty(Filter))
                    return true;

                foreach (var property in o.GetType().GetProperties())
                {
                    // Default property of objects? Results in ReflectionExceptions...
                    if (property.Name == "Item") 
                        continue;
                    
                    // Check if is IEnumerable and iterate over
                    // Note: Strings are IEnumerable but in this context not useful
                    // See: https://stackoverflow.com/a/6735081/12347616
                    if (property.GetValue(o) is IEnumerable enumerable and not string)
                    {
                        foreach (var oEnumerable in enumerable)
                        {
                            foreach (var oEnumProperty in oEnumerable.GetType().GetProperties())
                            {
                                if (Convert.ToString(oEnumProperty.GetValue(oEnumerable), nfi) is { } str
                                    && str.ToLower().Contains(Filter.ToLower()))
                                    return true;
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToString(property.GetValue(o), nfi) is { } str
                            && str.ToLower().Contains(Filter.ToLower()))
                            return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.StackTrace);
                return false;
            }
        }
    }
}