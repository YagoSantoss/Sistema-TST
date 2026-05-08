using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SistemaTstLargoTreze
{
    public static class CatPdfExporter
    {
        private const double PageWidth = 595;
        private const double PageHeight = 1180;
        private const double Margin = 38;
        private const double ContentWidth = PageWidth - (Margin * 2);

        public static string Exportar(CatRecord cat)
        {
            string pasta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SistemaTST", "CATs");
            Directory.CreateDirectory(pasta);

            string arquivo = Path.Combine(pasta, "CAT_" + cat.Id + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

            StringBuilder content = new StringBuilder();
            double y = PageHeight - 42;

            AddTitle(content, "COMUNICACAO DE ACIDENTE DE TRABALHO - CAT", ref y);

            AddSection(content, "DADOS CADASTRAIS", ref y);
            AddRow(content, "Codigo CAT", cat.Id.ToString(), "Empregado", cat.EmpregadoNome, ref y);
            AddRow(content, "Data do comunicado", cat.DataComunicacao, "Emitente", cat.Emitente, ref y);
            AddRow(content, "Aposentado", cat.Aposentado ? "Sim" : "Nao", "Filiacao Prev. Social", cat.FiliacaoPrevSocial, ref y);
            AddRow(content, "Area", cat.Area, "Tipo da CAT", cat.TipoCat, ref y);
            AddRow(content, "Data do acidente", cat.DataAcidente, "Tipo do acidente", cat.TipoAcidente, ref y);
            AddRow(content, "Hora do acidente", cat.HoraAcidente, "Horas trabalhadas antes", cat.HorasTrabalhadasAntes, ref y);
            AddRow(content, "Houve obito", cat.HouveObito ? "Sim" : "Nao", "Data do obito", cat.DataObito, ref y);
            AddRow(content, "Houve afastamento", cat.HouveAfastamento ? "Sim" : "Nao", "Registro policia", cat.RegistroPolicia ? "Sim" : "Nao", ref y);
            AddRow(content, "Ultimo dia trabalho", cat.UltimoDiaTrabalho, "Codificacao acidente", cat.CodificacaoAcidente, ref y);
            AddFullRow(content, "Situacao geradora", cat.SituacaoGeradora, ref y);
            AddFullRow(content, "CAT emitida por", cat.CatEmitidaPor, ref y);
            AddSection(content, "LOCAL DO ACIDENTE", ref y);
            AddRow(content, "Tipo do local", cat.LocalAcidente, "Especificacao", cat.EspecificacaoLocal, ref y);
            AddRow(content, "Tipo logradouro", cat.TipoLogradouro, "Numero", cat.Numero, ref y);
            AddRow(content, "Tipo inscricao", cat.TipoInscricao, "Inscricao estabelecimento", cat.InscricaoEstabelecimento, ref y);
            AddRow(content, "Logradouro", cat.Logradouro, "Municipio", cat.Municipio, ref y);
            AddRow(content, "UF", cat.Uf, "Bairro", cat.Bairro, ref y);
            AddRow(content, "Complemento", cat.Complemento, "CEP", cat.Cep, ref y);
            AddFullRow(content, "Codigo postal", cat.CodigoPostal, ref y);
            AddRow(content, "Situacao", cat.Situacao, "Resultado ASO", cat.ResultadoAso, ref y);
            AddTextBox(content, "Observacao da CAT", string.IsNullOrWhiteSpace(cat.ObservacaoCat) ? cat.Descricao : cat.ObservacaoCat, ref y, 46);

            AddSection(content, "DADOS COMPLEMENTARES", ref y);
            AddRow(content, "Parte do corpo atingida", cat.ParteCorpoAtingida, "Lateralidade", cat.Lateralidade, ref y);
            AddRow(content, "Agente causador", cat.AgenteCausador, "CID-10", cat.Cid10, ref y);
            AddRow(content, "Natureza da lesao", cat.NaturezaLesao, "Duracao do tratamento", cat.DuracaoTratamento, ref y);
            AddFullRow(content, "Medico / Dentista", cat.MedicoAssistente, ref y);
            AddTextBox(content, "Observacao medica", cat.ObservacaoMedica, ref y, 58);

            AddFooter(content, cat.Id);

            EscreverPdf(arquivo, content.ToString());
            return arquivo;
        }

        private static void AddTitle(StringBuilder content, string title, ref double y)
        {
            AddFilledRect(content, Margin, y - 30, ContentWidth, 30, "0.06 0.22 0.38");
            AddText(content, title, Margin + 12, y - 20, 14, true, "1 1 1");
            y -= 44;
        }

        private static void AddSection(StringBuilder content, string title, ref double y)
        {
            AddFilledRect(content, Margin, y - 20, ContentWidth, 20, "0.91 0.95 0.99");
            AddRect(content, Margin, y - 20, ContentWidth, 20);
            AddText(content, title, Margin + 8, y - 14, 9, true, "0.06 0.22 0.38");
            y -= 20;
        }

        private static void AddRow(StringBuilder content, string label1, string value1, string label2, string value2, ref double y)
        {
            double half = ContentWidth / 2;
            AddCell(content, Margin, y - 28, half, 28, label1, value1);
            AddCell(content, Margin + half, y - 28, half, 28, label2, value2);
            y -= 28;
        }

        private static void AddFullRow(StringBuilder content, string label, string value, ref double y)
        {
            AddCell(content, Margin, y - 28, ContentWidth, 28, label, value);
            y -= 28;
        }

        private static void AddTextBox(StringBuilder content, string label, string value, ref double y, double height)
        {
            AddRect(content, Margin, y - height, ContentWidth, height);
            AddText(content, label, Margin + 8, y - 13, 7, true, "0.33 0.33 0.33");

            double lineY = y - 28;
            foreach (string line in Wrap(value, 95, 3))
            {
                AddText(content, line, Margin + 8, lineY, 8, false, "0 0 0");
                lineY -= 12;
            }

            y -= height;
        }

        private static void AddCell(StringBuilder content, double x, double y, double width, double height, string label, string value)
        {
            AddRect(content, x, y, width, height);
            AddText(content, label, x + 6, y + height - 10, 7, true, "0.33 0.33 0.33");
            AddText(content, Fit(value, width > 260 ? 58 : 28), x + 6, y + 7, 8, false, "0 0 0");
        }

        private static void AddFooter(StringBuilder content, int catId)
        {
            AddText(content, "Gerado pelo Sistema TST - CAT " + catId + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), Margin, 24, 7, false, "0.35 0.35 0.35");
        }

        private static void AddRect(StringBuilder content, double x, double y, double width, double height)
        {
            content.AppendLine("0.75 0.78 0.82 RG");
            content.AppendLine(F(x) + " " + F(y) + " " + F(width) + " " + F(height) + " re S");
        }

        private static void AddFilledRect(StringBuilder content, double x, double y, double width, double height, string rgb)
        {
            content.AppendLine(rgb + " rg");
            content.AppendLine(F(x) + " " + F(y) + " " + F(width) + " " + F(height) + " re f");
        }

        private static void AddText(StringBuilder content, string text, double x, double y, int size, bool bold, string rgb)
        {
            content.AppendLine("BT");
            content.AppendLine(rgb + " rg");
            content.AppendLine((bold ? "/F2 " : "/F1 ") + size + " Tf");
            content.AppendLine(F(x) + " " + F(y) + " Td");
            content.AppendLine("(" + Escape(Fit(RemoverCaracteresInvalidos(text ?? string.Empty), 110)) + ") Tj");
            content.AppendLine("ET");
        }

        private static IEnumerable<string> Wrap(string value, int max, int maxLines)
        {
            string text = RemoverCaracteresInvalidos(value ?? string.Empty).Trim();
            if (text.Length == 0)
            {
                yield return string.Empty;
                yield break;
            }

            int count = 0;
            while (text.Length > 0 && count < maxLines)
            {
                if (text.Length <= max)
                {
                    yield return text;
                    yield break;
                }

                int cut = text.LastIndexOf(' ', Math.Min(max, text.Length - 1));
                if (cut < 20)
                    cut = max;

                yield return text.Substring(0, cut).Trim();
                text = text.Substring(cut).Trim();
                count++;
            }
        }

        private static string Fit(string value, int max)
        {
            string text = RemoverCaracteresInvalidos(value ?? string.Empty).Trim();
            if (text.Length <= max)
                return text;

            return text.Substring(0, Math.Max(0, max - 3)) + "...";
        }

        private static string Escape(string text)
        {
            return text.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
        }

        private static string F(double value)
        {
            return value.ToString("0.##", CultureInfo.InvariantCulture);
        }

        private static void EscreverPdf(string arquivo, string pageContent)
        {
            byte[] contentBytes = Encoding.ASCII.GetBytes(pageContent);
            List<byte[]> objects = new List<byte[]>
            {
                Encoding.ASCII.GetBytes("<< /Type /Catalog /Pages 2 0 R >>"),
                Encoding.ASCII.GetBytes("<< /Type /Pages /Kids [3 0 R] /Count 1 >>"),
                Encoding.ASCII.GetBytes("<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 1180] /Resources << /Font << /F1 4 0 R /F2 5 0 R >> >> /Contents 6 0 R >>"),
                Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>"),
                Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold >>"),
                Encoding.ASCII.GetBytes("<< /Length " + contentBytes.Length + " >>\nstream\n" + pageContent + "endstream")
            };

            using (FileStream stream = new FileStream(arquivo, FileMode.Create, FileAccess.Write))
            {
                WriteAscii(stream, "%PDF-1.4\n");
                List<long> offsets = new List<long> { 0 };

                for (int i = 0; i < objects.Count; i++)
                {
                    offsets.Add(stream.Position);
                    WriteAscii(stream, (i + 1) + " 0 obj\n");
                    stream.Write(objects[i], 0, objects[i].Length);
                    WriteAscii(stream, "\nendobj\n");
                }

                long xref = stream.Position;
                WriteAscii(stream, "xref\n0 " + (objects.Count + 1) + "\n");
                WriteAscii(stream, "0000000000 65535 f \n");
                for (int i = 1; i < offsets.Count; i++)
                    WriteAscii(stream, offsets[i].ToString("0000000000") + " 00000 n \n");
                WriteAscii(stream, "trailer\n<< /Size " + (objects.Count + 1) + " /Root 1 0 R >>\nstartxref\n" + xref + "\n%%EOF");
            }
        }

        private static void WriteAscii(Stream stream, string text)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
        }

        private static string RemoverCaracteresInvalidos(string text)
        {
            return text
                .Replace("ç", "c").Replace("Ç", "C")
                .Replace("ã", "a").Replace("á", "a").Replace("à", "a").Replace("â", "a")
                .Replace("é", "e").Replace("ê", "e")
                .Replace("í", "i")
                .Replace("ó", "o").Replace("õ", "o").Replace("ô", "o")
                .Replace("ú", "u")
                .Replace("Ã§", "c").Replace("Ã‡", "C")
                .Replace("Ã£", "a").Replace("Ã¡", "a").Replace("Ã ", "a").Replace("Ã¢", "a")
                .Replace("Ã©", "e").Replace("Ãª", "e")
                .Replace("Ã­", "i")
                .Replace("Ã³", "o").Replace("Ãµ", "o").Replace("Ã´", "o")
                .Replace("Ãº", "u");
        }
    }
}
