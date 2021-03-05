using Client.Model;

namespace Client.ViewModel
{
    public class TourViewModel
    {
        private Tour tour;

        public TourViewModel(string name)
        {
            tour = new Tour(name);
        }
    }
}