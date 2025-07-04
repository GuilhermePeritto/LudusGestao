using FluentValidation;
using LudusGestao.Application.DTOs.Recebivel;

namespace LudusGestao.Application.Validators.Recebivel
{
    public class UpdateRecebivelValidator : AbstractValidator<UpdateRecebivelDTO>
    {
        public UpdateRecebivelValidator()
        {
            RuleFor(x => x.ClienteId).NotEmpty().WithMessage("O cliente é obrigatório.");
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A descrição é obrigatória.");
            RuleFor(x => x.Valor).GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
            RuleFor(x => x.DataVencimento).NotEmpty().WithMessage("A data de vencimento é obrigatória.");
            RuleFor(x => x.Situacao).NotEmpty().WithMessage("A situação é obrigatória.");
        }
    }
} 