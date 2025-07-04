using FluentValidation;
using LudusGestao.Application.DTOs.Filial;

namespace LudusGestao.Application.Validators.Filial
{
    public class UpdateFilialValidator : AbstractValidator<UpdateFilialDTO>
    {
        public UpdateFilialValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome da filial é obrigatório.");
            RuleFor(x => x.Codigo).NotEmpty().WithMessage("O código é obrigatório.");
            RuleFor(x => x.Cnpj).NotEmpty().WithMessage("O CNPJ é obrigatório.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("E-mail inválido.");
        }
    }
} 