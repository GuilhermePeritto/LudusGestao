using System;
using System.Text.RegularExpressions;

namespace LudusGestao.Domain.ValueObjects
{
    public class Documento
    {
        public string Numero { get; private set; }
        public TipoDocumento Tipo { get; private set; }

        public Documento(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("Documento não pode ser vazio");

            var numeroLimpo = LimparDocumento(numero);
            
            if (IsCpf(numeroLimpo))
            {
                if (!ValidarCpf(numeroLimpo))
                    throw new ArgumentException("CPF inválido");
                
                Tipo = TipoDocumento.CPF;
            }
            else if (IsCnpj(numeroLimpo))
            {
                if (!ValidarCnpj(numeroLimpo))
                    throw new ArgumentException("CNPJ inválido");
                
                Tipo = TipoDocumento.CNPJ;
            }
            else
            {
                throw new ArgumentException("Documento deve ser um CPF ou CNPJ válido");
            }

            Numero = numeroLimpo;
        }

        private static string LimparDocumento(string documento)
        {
            return Regex.Replace(documento, @"[^\d]", "");
        }

        private static bool IsCpf(string numero)
        {
            return numero.Length == 11;
        }

        private static bool IsCnpj(string numero)
        {
            return numero.Length == 14;
        }

        private static bool ValidarCpf(string cpf)
        {
            if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
                return false;

            var soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            var resto = soma % 11;
            var digito1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digito1)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            var digito2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cpf[10].ToString()) == digito2;
        }

        private static bool ValidarCnpj(string cnpj)
        {
            if (cnpj.Length != 14 || cnpj.All(c => c == cnpj[0]))
                return false;

            var multiplicadores1 = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicadores2 = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(cnpj[i].ToString()) * multiplicadores1[i];

            var resto = soma % 11;
            var digito1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cnpj[12].ToString()) != digito1)
                return false;

            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(cnpj[i].ToString()) * multiplicadores2[i];

            resto = soma % 11;
            var digito2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cnpj[13].ToString()) == digito2;
        }

        public override string ToString()
        {
            return Tipo == TipoDocumento.CPF 
                ? FormatarCpf(Numero) 
                : FormatarCnpj(Numero);
        }

        private static string FormatarCpf(string cpf)
        {
            return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
        }

        private static string FormatarCnpj(string cnpj)
        {
            return $"{cnpj.Substring(0, 2)}.{cnpj.Substring(2, 3)}.{cnpj.Substring(5, 3)}/{cnpj.Substring(8, 4)}-{cnpj.Substring(12, 2)}";
        }
    }

    public enum TipoDocumento
    {
        CPF,
        CNPJ
    }
} 