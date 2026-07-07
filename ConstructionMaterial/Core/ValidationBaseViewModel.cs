using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConstructionMaterial.Core
{
    public class ValidationBaseViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new();
        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            return propertyName is not null
                && _errorsByPropertyName.ContainsKey(propertyName)
                ? _errorsByPropertyName[propertyName]
                : Enumerable.Empty<string>();
        }
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs propertyName)
        {
            ErrorsChanged?.Invoke(this, propertyName);
        }
        /// <summary>
        /// this method adds an error message to the specified property and raises the ErrorsChanged event if the error is not already present.
        /// It ensures that duplicate error messages are not added for the same property.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="error"></param>
        protected void AddError(string error, [CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null) return;
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();
            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            }
        }
        protected virtual void ClearError([CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null) return;
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            }
        }
    }
}
