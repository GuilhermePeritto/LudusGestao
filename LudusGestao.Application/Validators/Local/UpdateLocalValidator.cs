using FluentValidation;
using LudusGestao.Application.DTOs.Local;

namespace LudusGestao.Application.Validators.Local
{
    public class UpdateLocalValidator : AbstractValidator<UpdateLocalDTO>
    {
        public UpdateLocalValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome do local é obrigatório.");
            RuleFor(x => x.Tipo).NotEmpty().WithMessage("O tipo do local é obrigatório.");
            RuleFor(x => x.Intervalo).GreaterThan(0).WithMessage("O intervalo deve ser maior que zero.");
            RuleFor(x => x.ValorHora).GreaterThanOrEqualTo(0).WithMessage("O valor/hora deve ser positivo.");
            RuleFor(x => x.Situacao).NotEmpty().WithMessage("A situação é obrigatória.");
            RuleFor(x => x.FilialId).NotEmpty().WithMessage("A filial é obrigatória.");
        }
    }
} 