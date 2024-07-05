
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dbe.FoundationLibrary.Core.Common.Services
{
    public interface IModelDataAnnotationCheck
    {
        IEnumerable<ValidationResult> Validate(object model);
    }
}