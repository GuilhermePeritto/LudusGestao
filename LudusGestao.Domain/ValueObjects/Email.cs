using System;
using System.Text.RegularExpressions;

namespace LudusGestao.Domain.ValueObjects
{
    public class Email
    {
        public string Endereco { get; private set; }

        public Email(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco))
                throw new ArgumentException("E-mail não pode ser vazio");

            var emailLimpo = endereco.Trim().ToLower();
            
            if (!ValidarEmail(emailLimpo))
                throw new ArgumentException("E-mail inválido");
                
            Endereco = emailLimpo;
        }

        private static bool ValidarEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Endereco;
        }
    }
} 