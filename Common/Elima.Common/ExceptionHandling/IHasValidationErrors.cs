using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elima.Common.ExceptionHandling;

public interface IHasValidationErrors
{
    IList<ValidationResult> ValidationErrors { get; }
}
