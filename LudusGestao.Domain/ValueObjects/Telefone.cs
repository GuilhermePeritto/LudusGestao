using System;

namespace LudusGestao.Domain.ValueObjects
{
    public class Telefone
    {
        public string Numero { get; private set; }

        public Telefone(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("Telefone inválido");
            Numero = numero;
        }
    }
} 