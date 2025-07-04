using FluentValidation;

namespace LudusGestao.Application.Validators.Empresa
{
    public class UpdateEmpresaValidator : AbstractValidator<DTOs.Empresa.UpdateEmpresaDTO>
    {
        public UpdateEmpresaValidator()
        {
            RuleFor(x => x.Nome).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
} 