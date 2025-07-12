using FluentValidation;

namespace LudusGestao.Application.Validators.Empresa
{
    public class CreateEmpresaValidator : AbstractValidator<DTOs.Empresa.CreateEmpresaDTO>
    {
        public CreateEmpresaValidator()
        {
            RuleFor(x => x.Nome).NotEmpty();
            RuleFor(x => x.Cnpj).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Rua).NotEmpty();
            RuleFor(x => x.Cidade).NotEmpty();
            RuleFor(x => x.Estado).NotEmpty();
            RuleFor(x => x.CEP).NotEmpty();
        }
    }
} 