using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.GriffithsBen.BloggableMVC.Markup {

    public class MarkupValidator : IMarkupValidator {

        private List<string> ErrorMessageCollection { get; set; }

        private List<string> WarningMessageCollection { get; set; }

        public MarkupValidator() {
            this.ErrorMessageCollection = new List<string>();
            this.WarningMessageCollection = new List<string>();
        }

        public bool IsValid {
            get {
                return this.ErrorMessages.Count() == 0;
            }
        }

        public IEnumerable<string> ErrorMessages {
            get {
                return this.ErrorMessageCollection;
            }
        }

        public IEnumerable<string> WarningMessages {
            get {
                return this.WarningMessageCollection;
            }
        }

        public void AddError(string message) {
            this.ErrorMessageCollection.Add(message);
        }

        public void AddError(string messageFormat, params object[] paramsArray) {
            this.AddError(string.Format(messageFormat, paramsArray));
        }

        public void AddWarning(string message) {
            this.WarningMessageCollection.Add(message);
        }

        public void AddWarning(string messageFormat, params object[] paramsArray) {
            this.AddWarning(string.Format(messageFormat, paramsArray));
        }

        public void Reset() {
            this.ErrorMessageCollection = new List<string>();
            this.WarningMessageCollection = new List<string>();
        }
        
    }

}
