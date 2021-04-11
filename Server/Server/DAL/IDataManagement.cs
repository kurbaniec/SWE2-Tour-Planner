﻿using System.Collections.Generic;
using Model;

namespace Server.DAL
{
    public interface IDataManagement
    {
        List<Tour> GetTours();

        (Tour?, string) AddTour(Tour tour);

        (Tour?, string) UpdateTour(Tour tour);

        (bool, string) DeleteTour(int id);
    }
}