using FluentValidation;
using LudusGestao.Application.DTOs.Usuario;

namespace LudusGestao.Application.Validators.Usuario
{
    public class UpdateUsuarioValidator : AbstractValidator<UpdateUsuarioDTO>
    {
        public UpdateUsuarioValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome do usuário é obrigatório.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("E-mail inválido.");
            RuleFor(x => x.EmpresaId).NotEmpty().WithMessage("A empresa é obrigatória.");
        }
    }
} 