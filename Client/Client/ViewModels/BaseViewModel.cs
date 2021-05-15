using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.ViewModels
{
    /// <summary>
    /// Abstract ViewModel that implements <c>OnPropertyChanged</c>
    /// which all ViewModels use.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}