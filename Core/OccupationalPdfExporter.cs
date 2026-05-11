using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;

namespace SistemaTstLargoTreze
{
    public static class OccupationalPdfExporter
    {
        private const double PageWidth = 595;
        private const double PageHeight = 900;
        private const double Margin = 38;
        private const double ContentWidth = PageWidth - (Margin * 2);

        public static string ExportarAso(AsoRecord aso)
        {
            string pasta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SistemaTST", "ASOs");
            Directory.CreateDirectory(pasta);

            string arquivo = Path.Combine(pasta, "ASO_" + aso.Id + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");
            List<AsoExameRecord> exames = CadastrosRepository.GetAsoExames(aso.Id);
            byte[] logoBytes = GetLogoJpegBytes(out int logoWidth, out int logoHeight);

            StringBuilder content = new StringBuilder();
            double y = PageHeight - 42;

            AddBrandHeader(content, logoBytes != null, "ATESTADO DE SAUDE OCUPACIONAL - ASO", ref y);

            AddSection(content, "DADOS DO ASO", ref y);
            AddRow(content, "Codigo ASO", aso.Id.ToString(), "Data do ASO", aso.DataAso, ref y);
            AddRow(content, "Empregado", aso.EmpregadoNome, "Medico responsavel", aso.MedicoNome, ref y);
            AddRow(content, "Tipo de exame", aso.TipoExame, "Resultado", aso.Resultado, ref y);
            AddFullRow(content, "CAT vinculada", aso.CatId.HasValue ? "CAT " + aso.CatId.Value : "Nao vinculada", ref y);

            AddSection(content, "EXAMES COMPLEMENTARES", ref y);
            if (exames.Count == 0)
            {
                AddFullRow(content, "Exames", "Nenhum exame complementar vinculado a este ASO.", ref y);
            }
            else
            {
                foreach (AsoExameRecord exame in exames)
                {
                    AddRow(content, "Exame", exame.TipoExameNome, "Data", exame.DataExame, ref y);
                    AddRow(content, "Resultado", exame.Resultado, "Observacoes", exame.Observacoes, ref y);
                }
            }

            AddTextBox(content, "Observacoes do ASO", aso.Observacoes, ref y, 70);
            AddSignature(content, "Assinatura do medico responsavel", ref y);
            AddFooter(content, "ASO " + aso.Id);

            EscreverPdf(arquivo, content.ToString(), logoBytes, logoWidth, logoHeight);
            return arquivo;
        }

        public static string ExportarFatorRisco(RiskFactorRecord risco)
        {
            string pasta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SistemaTST", "FatoresRisco");
            Directory.CreateDirectory(pasta);

            string arquivo = Path.Combine(pasta, "FATOR_RISCO_" + risco.Id + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");
            byte[] logoBytes = GetLogoJpegBytes(out int logoWidth, out int logoHeight);

            StringBuilder content = new StringBuilder();
            double y = PageHeight - 42;

            AddBrandHeader(content, logoBytes != null, "RELATORIO DE FATORES DE RISCO - S-2240", ref y);

            AddSection(content, "VINCULOS", ref y);
            AddRow(content, "Codigo", risco.Id.ToString(), "Empregado", risco.EmpregadoNome, ref y);
            AddRow(content, "Ambiente", risco.AmbienteNome, "Tipo de fator", risco.TipoFator, ref y);

            AddSection(content, "AVALIACAO DO RISCO", ref y);
            AddFullRow(content, "Agente", risco.Agente, ref y);
            AddRow(content, "Intensidade", risco.Intensidade, "Tecnica de medicao", risco.TecnicaMedicao, ref y);
            AddRow(content, "Data de avaliacao", risco.DataAvaliacao, "Inicio da exposicao", risco.InicioExposicao, ref y);
            AddRow(content, "Fim da exposicao", risco.FimExposicao, "Usa EPI", risco.UsaEpi ? "Sim" : "Nao", ref y);
            AddFullRow(content, "EPI eficaz", risco.EpiEficaz ? "Sim" : "Nao", ref y);
            AddTextBox(content, "Descricao das atividades", risco.DescricaoAtividades, ref y, 92);
            AddSignature(content, "Responsavel tecnico", ref y);
            AddFooter(content, "Fator de risco " + risco.Id);

            EscreverPdf(arquivo, content.ToString(), logoBytes, logoWidth, logoHeight);
            return arquivo;
        }

        private static void AddBrandHeader(StringBuilder content, bool hasLogo, string title, ref double y)
        {
            AddRect(content, Margin, y - 58, ContentWidth, 58);

            if (hasLogo)
                AddImage(content, "Im1", Margin + 10, y - 49, 72, 42);

            AddText(content, "SISTEMA TST LARGO TREZE", Margin + 96, y - 24, 14, true, "0.06 0.22 0.38");
            AddText(content, title, Margin + 96, y - 41, 10, true, "0.06 0.22 0.38");
            y -= 72;
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
            foreach (string line in Wrap(value, 95, 4))
            {
                AddText(content, line, Margin + 8, lineY, 8, false, "0 0 0");
                lineY -= 12;
            }

            y -= height;
        }

        private static void AddSignature(StringBuilder content, string label, ref double y)
        {
            y -= 28;
            content.AppendLine("0.35 0.35 0.35 RG");
            content.AppendLine(F(Margin + 110) + " " + F(y) + " 300 0 m S");
            AddText(content, label, Margin + 188, y - 14, 8, false, "0.35 0.35 0.35");
            y -= 34;
        }

        private static void AddCell(StringBuilder content, double x, double y, double width, double height, string label, string value)
        {
            AddRect(content, x, y, width, height);
            AddText(content, label, x + 6, y + height - 10, 7, true, "0.33 0.33 0.33");
            AddText(content, Fit(value, width > 260 ? 58 : 28), x + 6, y + 7, 8, false, "0 0 0");
        }

        private static void AddFooter(StringBuilder content, string referencia)
        {
            AddText(content, "Gerado pelo Sistema TST - " + referencia + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), Margin, 24, 7, false, "0.35 0.35 0.35");
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

        private static void AddImage(StringBuilder content, string imageName, double x, double y, double width, double height)
        {
            content.AppendLine("q");
            content.AppendLine(F(width) + " 0 0 " + F(height) + " " + F(x) + " " + F(y) + " cm");
            content.AppendLine("/" + imageName + " Do");
            content.AppendLine("Q");
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

        private static void EscreverPdf(string arquivo, string pageContent, byte[] logoBytes, int logoWidth, int logoHeight)
        {
            byte[] contentBytes = Encoding.ASCII.GetBytes(pageContent);
            string xObjects = logoBytes == null ? string.Empty : " /XObject << /Im1 7 0 R >>";
            List<byte[]> objects = new List<byte[]>
            {
                Encoding.ASCII.GetBytes("<< /Type /Catalog /Pages 2 0 R >>"),
                Encoding.ASCII.GetBytes("<< /Type /Pages /Kids [3 0 R] /Count 1 >>"),
                Encoding.ASCII.GetBytes("<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 900] /Resources << /Font << /F1 4 0 R /F2 5 0 R >>" + xObjects + " >> /Contents 6 0 R >>"),
                Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>"),
                Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold >>"),
                Encoding.ASCII.GetBytes("<< /Length " + contentBytes.Length + " >>\nstream\n" + pageContent + "endstream")
            };

            if (logoBytes != null)
            {
                objects.Add(Combine(
                    Encoding.ASCII.GetBytes("<< /Type /XObject /Subtype /Image /Width " + logoWidth + " /Height " + logoHeight + " /ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length " + logoBytes.Length + " >>\nstream\n"),
                    logoBytes,
                    Encoding.ASCII.GetBytes("\nendstream")
                ));
            }

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

        private static byte[] GetLogoJpegBytes(out int width, out int height)
        {
            width = 0;
            height = 0;

            foreach (string path in GetLogoCandidates())
            {
                if (!File.Exists(path))
                    continue;

                using (Image source = Image.FromFile(path))
                using (Bitmap bitmap = new Bitmap(source.Width, source.Height))
                using (Graphics graphics = Graphics.FromImage(bitmap))
                using (MemoryStream stream = new MemoryStream())
                {
                    graphics.Clear(Color.White);
                    graphics.DrawImage(source, 0, 0, source.Width, source.Height);
                    bitmap.Save(stream, ImageFormat.Jpeg);
                    width = bitmap.Width;
                    height = bitmap.Height;
                    return stream.ToArray();
                }
            }

            return null;
        }

        private static IEnumerable<string> GetLogoCandidates()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            yield return Path.Combine(baseDirectory, "Assets", "logo.png");
            yield return Path.Combine(baseDirectory, "logo.png");
            yield return Path.Combine(Environment.CurrentDirectory, "Assets", "logo.png");
            yield return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Assets", "logo.png");
        }

        private static byte[] Combine(params byte[][] arrays)
        {
            int length = 0;
            foreach (byte[] array in arrays)
                length += array.Length;

            byte[] result = new byte[length];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, result, offset, array.Length);
                offset += array.Length;
            }

            return result;
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
