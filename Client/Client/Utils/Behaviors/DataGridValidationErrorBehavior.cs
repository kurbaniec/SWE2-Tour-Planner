using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;


namespace Client.Utils.Behaviors
{
    // Used to get Validation Error from Datagrid
    // Based on https://stackoverflow.com/a/50305939/12347616
    public class DataGridValidationErrorBehavior : Behavior<DataGrid>
    {
        public static readonly DependencyProperty ValidationErrorsProperty = DependencyProperty.Register("ValidationErrors", typeof(ObservableCollection<ValidationError>), typeof(DataGridValidationErrorBehavior), new PropertyMetadata(new ObservableCollection<ValidationError>()));

        public ObservableCollection<ValidationError> ValidationErrors
        {
            get { return (ObservableCollection<ValidationError>)this.GetValue(ValidationErrorsProperty); }
            set { this.SetValue(ValidationErrorsProperty, value); }
        }

        public static readonly DependencyProperty HasValidationErrorProperty = DependencyProperty.Register("HasValidationError", typeof(bool), typeof(DataGridValidationErrorBehavior), new PropertyMetadata(false));

        public bool HasValidationError
        {
            get { return (bool)this.GetValue(HasValidationErrorProperty); }
            set { this.SetValue(HasValidationErrorProperty, value); }
        }

        public DataGridValidationErrorBehavior()
            : base()
        { }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                this.ValidationErrors.Add(e.Error);
            }
            else
            {
                this.ValidationErrors.Remove(e.Error);
            }

            this.HasValidationError = this.ValidationErrors.Count > 0;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            Validation.AddErrorHandler(this.AssociatedObject, Validation_Error);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            Validation.RemoveErrorHandler(this.AssociatedObject, Validation_Error);
        }
    }
}