using FluentValidation;

namespace LudusGestao.Application.Validators.Empresa
{
    public class UpdateEmpresaValidator : AbstractValidator<DTOs.Empresa.UpdateEmpresaDTO>
    {
        public UpdateEmpresaValidator()
        {
            RuleFor(x => x.Nome).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Rua).NotEmpty();
            RuleFor(x => x.Cidade).NotEmpty();
            RuleFor(x => x.Estado).NotEmpty();
            RuleFor(x => x.CEP).NotEmpty();
        }
    }
} 