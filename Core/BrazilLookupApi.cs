using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace SistemaTstLargoTreze
{
    public sealed class CepLookupResult
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public bool Erro { get; set; }
    }

    public sealed class CnpjLookupResult
    {
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CnaeDescricao { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }
        public string Telefone { get; set; }
    }

    public static class BrazilLookupApi
    {
        public static CepLookupResult ConsultarCep(string cep)
        {
            string digits = InputFormatHelper.OnlyDigits(cep);
            if (digits.Length != 8)
                throw new InvalidOperationException("Informe um CEP com 8 numeros.");

            string json = Get("https://viacep.com.br/ws/" + digits + "/json/");
            CepLookupResult result = new CepLookupResult
            {
                Cep = JsonValue(json, "cep"),
                Logradouro = JsonValue(json, "logradouro"),
                Complemento = JsonValue(json, "complemento"),
                Bairro = JsonValue(json, "bairro"),
                Localidade = JsonValue(json, "localidade"),
                Uf = JsonValue(json, "uf"),
                Erro = string.Equals(JsonValue(json, "erro"), "true", StringComparison.OrdinalIgnoreCase)
            };

            if (result.Erro)
                throw new InvalidOperationException("CEP não encontrado.");

            return result;
        }

        public static CnpjLookupResult ConsultarCnpj(string cnpj)
        {
            string digits = InputFormatHelper.OnlyDigits(cnpj);
            if (digits.Length != 14)
                throw new InvalidOperationException("Informe um CNPJ com 14 numeros.");

            string json = Get("https://brasilapi.com.br/api/cnpj/v1/" + digits);
            return new CnpjLookupResult
            {
                Cnpj = JsonValue(json, "cnpj"),
                RazaoSocial = JsonValue(json, "razao_social"),
                NomeFantasia = JsonValue(json, "nome_fantasia"),
                CnaeDescricao = JsonValue(json, "cnae_fiscal_descrição"),
                Cep = JsonValue(json, "cep"),
                Logradouro = JsonValue(json, "logradouro"),
                Numero = JsonValue(json, "numero"),
                Complemento = JsonValue(json, "complemento"),
                Bairro = JsonValue(json, "bairro"),
                Municipio = JsonValue(json, "municipio"),
                Uf = JsonValue(json, "uf"),
                Telefone = JsonValue(json, "ddd_telefone_1")
            };
        }

        private static string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 20000;
            request.UserAgent = "SistemaTST/1.0";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        private static string JsonValue(string json, string key)
        {
            if (string.IsNullOrWhiteSpace(json))
                return string.Empty;

            Match match = Regex.Match(
                json,
                "\"" + Regex.Escape(key) + "\"\\s*:\\s*(null|true|false|\"(?<value>(?:\\\\.|[^\"])*)\"|(?<number>-?\\d+(\\.\\d+)?))",
                RegexOptions.IgnoreCase
            );

            if (!match.Success)
                return string.Empty;

            if (match.Groups["value"].Success)
                return Regex.Unescape(match.Groups["value"].Value);

            if (match.Groups["number"].Success)
                return match.Groups["number"].Value;

            if (match.Value.IndexOf(": true", StringComparison.OrdinalIgnoreCase) >= 0)
                return "true";

            if (match.Value.IndexOf(": false", StringComparison.OrdinalIgnoreCase) >= 0)
                return "false";

            return string.Empty;
        }
    }
}
