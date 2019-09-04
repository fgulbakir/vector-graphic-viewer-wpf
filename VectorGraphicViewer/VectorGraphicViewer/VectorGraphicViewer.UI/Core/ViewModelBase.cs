using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;


namespace VectorGraphicViewer.UI.Core
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        public string this[string columnName] => OnValidate(columnName);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public string Error { get { return null; } }

        protected virtual string OnValidate(string propertyName)
        {
            var context = new ValidationContext(this)
            {
                MemberName = propertyName

            };
            var results = new Collection<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, context, results, true);
            return !isValid ? results[0].ErrorMessage : null;
        }
    }
}
