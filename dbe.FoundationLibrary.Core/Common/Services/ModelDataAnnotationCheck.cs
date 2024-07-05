using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dbe.FoundationLibrary.Core.Common.Services
{
    /// <summary>
    /// 针对Model类中的字段的DataAnnotation的验证方法
    /// </summary>
    public class ModelDataAnnotationCheck : IModelDataAnnotationCheck
    {
        public ModelDataAnnotationCheck()
        {

        }

        #region       公开方法 start
        /// <summary>
        /// 验证Model类中的字段的DataAnnotation
        /// </summary>
        /// <param name="model">模型实体</param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(object model)
        {
            string errorMessage = "";
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            var isValid = Validator.TryValidateObject(model, context, results, true);
            if (!isValid)
            {
                foreach (var item in results)
                {
                    errorMessage += $"- {item.ErrorMessage}\r\n";
                }
                //throw new Exception(errorMessage);
            }
            return results;
        }
        #endregion 公开方法 end
    }
}