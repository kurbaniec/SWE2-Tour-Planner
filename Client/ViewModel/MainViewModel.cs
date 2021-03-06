namespace Client.ViewModel
{
    public class MainViewModel
    {
        private TourListModel tourList;

        public TourListModel TourList => tourList;

        public MainViewModel()
        {
            tourList = new TourListModel();
        }
    }
}