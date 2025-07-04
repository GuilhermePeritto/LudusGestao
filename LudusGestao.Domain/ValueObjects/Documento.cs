using System;

namespace LudusGestao.Domain.ValueObjects
{
    public class Documento
    {
        public string Numero { get; private set; }

        public Documento(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("Documento inválido");
            Numero = numero;
        }
    }
} 