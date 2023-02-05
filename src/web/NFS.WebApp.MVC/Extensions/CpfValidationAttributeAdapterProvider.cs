using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace NFS.WebApp.MVC.Extensions
{
    public class CpfValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            //Aqui ele pergunta se o seu atributo é do tipo CpfAttribute então ele vai prover a classe CpfAttributeAdapter que
            //por sua vez se tem a instancia da classe CpfAttribute que realiza a validação e repassa o resultado para o front-end 
            if (attribute is CpfAttribute CpfAttribute)
            {
                return new CpfAttributeAdapter(CpfAttribute, stringLocalizer);
            }

            return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
