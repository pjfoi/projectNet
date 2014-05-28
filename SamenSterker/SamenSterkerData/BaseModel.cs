using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    public abstract class BaseModel : INotifyDataErrorInfo
    {


        #region INotifyDataErrorInfo members
        // source : http://social.technet.microsoft.com/wiki/contents/articles/19490.validating-data-in-wpf-4-5-using-the-inotifyerrordataerror-interface.aspx

        protected void UpdateValidationErrors(string propertyKey, ICollection<string> validationErrors)
        {
            // unvalid -> add errors
            if (validationErrors != null && validationErrors.Count > 0)
            {
                /* Update the collection in the dictionary returned by the GetErrors method */
                _validationErrors[propertyKey] = validationErrors;
                /* Raise event to tell WPF to execute the GetErrors method */
                RaiseErrorsChanged(propertyKey);
            }

            // valid -> remove old errors
            else if (_validationErrors.ContainsKey(propertyKey))
            {
                /* Remove all errors for this property */
                _validationErrors.Remove(propertyKey);
                /* Raise event to tell WPF to execute the GetErrors method */
                RaiseErrorsChanged(propertyKey);
            }
        }

        protected readonly Dictionary<string, ICollection<string>>
            _validationErrors = new Dictionary<string, ICollection<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }
        #endregion
    }
}
