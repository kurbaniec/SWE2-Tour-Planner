namespace Client.Utils.Mediators
{
    /// <summary>
    /// Enum that lists all supported operations in the Mediators used by the ViewModels.
    /// </summary>
    public enum ViewModelMessages
    {
        TourAddition,
        TourDeletion,
        TourCopy,
        SelectedTourChange,
        FilterChange,
        Import,
        ExportThis,
        ExportAll,
        TransactionBegin,
        TransactionEnd,
    };
}