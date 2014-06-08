using System.Collections.Generic;

namespace Com.GriffithsBen.BloggableMVC.Markup {
    public interface IMarkupValidator {

        bool IsValid { get; }

        IEnumerable<string> ErrorMessages { get; }

        IEnumerable<string> WarningMessages { get; }

        void AddError(string message);

        void AddError(string message, params object[] paramsArray);

        void AddWarning(string message);

        void AddWarning(string message, params object[] paramsArray);

        void Reset();
    }
}
