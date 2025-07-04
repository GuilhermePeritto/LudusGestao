using System;
using System.Text.RegularExpressions;

namespace LudusGestao.Domain.ValueObjects
{
    public class Telefone
    {
        public string Numero { get; private set; }
        public TipoTelefone Tipo { get; private set; }

        public Telefone(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("Telefone não pode ser vazio");

            var numeroLimpo = LimparTelefone(numero);
            
            if (!ValidarTelefone(numeroLimpo))
                throw new ArgumentException("Telefone inválido");

            Numero = numeroLimpo;
            Tipo = DeterminarTipo(numeroLimpo);
        }

        private static string LimparTelefone(string telefone)
        {
            return Regex.Replace(telefone, @"[^\d]", "");
        }

        private static bool ValidarTelefone(string numero)
        {
            // Valida telefones fixos (10 dígitos) e celulares (11 dígitos)
            return numero.Length == 10 || numero.Length == 11;
        }

        private static TipoTelefone DeterminarTipo(string numero)
        {
            if (numero.Length == 11 && numero.StartsWith("9"))
                return TipoTelefone.Celular;
            
            return TipoTelefone.Fixo;
        }

        public override string ToString()
        {
            if (Numero.Length == 10)
            {
                return $"({Numero.Substring(0, 2)}) {Numero.Substring(2, 4)}-{Numero.Substring(6, 4)}";
            }
            else
            {
                return $"({Numero.Substring(0, 2)}) {Numero.Substring(2, 5)}-{Numero.Substring(7, 4)}";
            }
        }
    }

    public enum TipoTelefone
    {
        Fixo,
        Celular
    }
} 