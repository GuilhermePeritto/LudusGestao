using System;
using System.Text.RegularExpressions;

namespace LudusGestao.Domain.ValueObjects
{
    public class Email
    {
        public string Endereco { get; private set; }

        public Email(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco) || !Regex.IsMatch(endereco, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("E-mail inválido");
            Endereco = endereco;
        }
    }
} 