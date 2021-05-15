namespace Client.Logic.DAL
{
    public interface IFilter
    {
        string Filter { get; set; }
        bool ApplyFilter(object o);
    }
}