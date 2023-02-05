using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace NFS.WebApp.MVC.Extensions
{
    public class CpfAttributeAdapter : AttributeAdapterBase<CpfAttribute>
    {
        public CpfAttributeAdapter(CpfAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        { }

        ///Esse método faz a validação do front-end
        public override void AddValidation(ClientModelValidationContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-cpf", GetErrorMessage(context));
        }
        
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return "CPF em formato inválido";
        }
    }
}
