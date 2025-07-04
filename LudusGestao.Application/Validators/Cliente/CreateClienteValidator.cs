using FluentValidation;
using LudusGestao.Application.DTOs.Cliente;

namespace LudusGestao.Application.Validators.Cliente
{
    public class CreateClienteValidator : AbstractValidator<CreateClienteDTO>
    {
        public CreateClienteValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome do cliente é obrigatório.");
            RuleFor(x => x.Documento).NotEmpty().WithMessage("O documento é obrigatório.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("E-mail inválido.");
            RuleFor(x => x.Situacao).NotEmpty().WithMessage("A situação é obrigatória.");
        }
    }
} 