using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;


namespace Client.Utils.Behaviors
{
    /// <summary>
    /// Used to get Validation Error from Datagrid.
    /// Based on https://stackoverflow.com/a/50305939/12347616
    /// </summary>
    public class DataGridValidationErrorBehavior : Behavior<DataGrid>
    {
        public static readonly DependencyProperty ValidationErrorsProperty =
            DependencyProperty.Register("ValidationErrors", typeof(ObservableCollection<ValidationError>),
                typeof(DataGridValidationErrorBehavior),
                new PropertyMetadata(new ObservableCollection<ValidationError>()));

        public ObservableCollection<ValidationError> ValidationErrors
        {
            get => (ObservableCollection<ValidationError>) GetValue(ValidationErrorsProperty);
            set => SetValue(ValidationErrorsProperty, value);
        }

        public static readonly DependencyProperty HasValidationErrorProperty =
            DependencyProperty.Register("HasValidationError", typeof(bool), typeof(DataGridValidationErrorBehavior),
                new PropertyMetadata(false));

        public bool HasValidationError
        {
            get => (bool) GetValue(HasValidationErrorProperty);
            set => SetValue(HasValidationErrorProperty, value);
        }

        // ReSharper disable once EmptyConstructor
        public DataGridValidationErrorBehavior()
            // ReSharper disable once RedundantBaseConstructorCall
            : base()
        {
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                ValidationErrors.Add(e.Error);
            }
            else
            {
                ValidationErrors.Remove(e.Error);
            }

            HasValidationError = ValidationErrors.Count > 0;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            Validation.AddErrorHandler(AssociatedObject, Validation_Error!);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            Validation.RemoveErrorHandler(AssociatedObject, Validation_Error!);
        }
    }
}