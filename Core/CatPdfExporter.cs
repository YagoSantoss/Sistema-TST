using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
            byte[] logoBytes = GetLogoJpegBytes(out int logoWidth, out int logoHeight);
            List<CatTestemunhaRecord> testemunhas = CadastrosRepository.GetCatTestemunhas(cat.Id);
            EmpregadoRecord empregado = null;
            try
            {
                empregado = CadastrosRepository.GetEmpregado(cat.EmpregadoId);
            }
            catch
            {
                empregado = null;
            }

            StringBuilder content = new StringBuilder();
            double y = PageHeight - 58;

            AddCatOfficialHeader(content, logoBytes != null, cat, ref y);

            AddPlainSection(content, "Informações do Emitente", ref y);
            AddFormRow(content, ref y, 16,
                "Emitente", cat.Emitente,
                "Data Emissão", cat.DataComunicacao);
            AddFormRow(content, ref y, 16,
                "Tipo de CAT", cat.TipoCat,
                "Comunicação Óbito", cat.HouveObito ? "Sim" : "Não");
            AddFormRow(content, ref y, 16,
                "Filiação", cat.FiliacaoPrevSocial,
                "E-mail", string.Empty);

            AddPlainSection(content, "Informações do Empregador", ref y);
            AddFormRow(content, ref y, 16,
                "Razão Social/Nome", EmpregadorNome(cat),
                "CNAE", cat.CnaeEmpregador);
            AddFormRow(content, ref y, 16,
                "Tipo/Num Doc", cat.TipoInscricao + " " + cat.InscricaoEstabelecimento,
                "Telefone", string.Empty);
            AddFormRow(content, ref y, 16,
                "CEP", cat.Cep,
                "Estado", cat.Uf);
            AddFormRow(content, ref y, 16,
                "Bairro", cat.Bairro,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 16,
                "Endereço", cat.Logradouro + " " + cat.Numero,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 16,
                "Município", cat.Municipio,
                string.Empty, string.Empty);

            AddPlainSection(content, "Informações do Acidentado", ref y);
            AddFormRow(content, ref y, 16,
                "Nome", cat.EmpregadoNome,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 16,
                "Nome da Mae", string.Empty,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 16,
                "Data de Nascimento", string.Empty,
                "Sexo", string.Empty);
            AddFormRow(content, ref y, 16,
                "Grau de Instrução", string.Empty,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 16,
                "Estado Civil", string.Empty,
                "Remuneração", string.Empty);
            AddFormRow(content, ref y, 16,
                "CTPS", string.Empty,
                "Identidade", string.Empty);
            AddFormRow(content, ref y, 16,
                "PIS/PASEP/NIT", string.Empty,
                "CEP", empregado == null ? string.Empty : empregado.Cpf);
            AddFormRow(content, ref y, 16,
                "Endereço", string.Empty,
                "Bairro", string.Empty);
            AddFormRow(content, ref y, 16,
                "Estado", cat.Uf,
                "Município", cat.Municipio);
            AddFormRow(content, ref y, 16,
                "Telefone", string.Empty,
                "CBO", empregado == null ? string.Empty : empregado.Cargo);
            AddFormRow(content, ref y, 16,
                "Aposentadoria", cat.Aposentado ? "Sim" : "Não",
                "Área", cat.Area);

            AddPlainSection(content, "Informações do Acidente", ref y);
            AddFormRow(content, ref y, 17,
                "Data do Acidente", cat.DataAcidente,
                "Hora do Acidente", cat.HoraAcidente);
            AddFormRow(content, ref y, 17,
                "Horas Trabalhadas", cat.HorasTrabalhadasAntes,
                "Tipo", cat.TipoAcidente);
            AddFormRow(content, ref y, 17,
                "Houve Afastamento?", cat.HouveAfastamento ? "Sim" : "Não",
                "Reg. Policial", cat.RegistroPolicia ? "Sim" : "Não");
            AddFormRow(content, ref y, 17,
                "Local do Acidente", cat.LocalAcidente,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 17,
                "Esp. Local", cat.EspecificacaoLocal,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 24,
                "CNPJ / CGC ou CEI da Prestadora", cat.InscricaoEstabelecimento,
                "UF do Acidente", cat.Uf);
            AddFormRow(content, ref y, 24,
                "Município do Acidente", cat.Municipio,
                "Último dia Trab. Dt Óbito", string.IsNullOrWhiteSpace(cat.DataObito) ? cat.UltimoDiaTrabalho : cat.DataObito);
            AddFormRow(content, ref y, 17,
                "Parte do Corpo", cat.ParteCorpoAtingida,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 17,
                "Agente Causador", cat.AgenteCausador,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 17,
                "Sit. Geradora", cat.SituacaoGeradora,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 17,
                "Morte", cat.HouveObito ? "Sim" : "Não",
                "Data Óbito", cat.DataObito);

            y -= 34;
            AddSignatureLine(content, Margin + 10, y, 230, "Local e Data");
            AddSignatureLine(content, Margin + 270, y, 255, "Assinatura e carimbo do emitente");
            y -= 34;

            AddPlainSection(content, "Informações do Atestado Médico", ref y);
            AddFormRow(content, ref y, 17,
                "Unidade", string.Empty,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 17,
                "Data Atendimento", cat.DataComunicacao,
                "Hora Atendimento", string.Empty);
            AddFormRow(content, ref y, 17,
                "Houve Internação", string.Empty,
                "Será afastado?", cat.HouveAfastamento ? "Sim" : "Não");
            AddFormRow(content, ref y, 17,
                "Nat. Lesão", cat.NaturezaLesao,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 17,
                "CID - 10", cat.Cid10,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 17,
                "Observações", cat.ObservacaoMedica,
                string.Empty, string.Empty);
            AddFormRow(content, ref y, 17,
                "CRM", cat.MedicoAssistente,
                string.Empty, string.Empty);

            y -= 34;
            AddSignatureLine(content, Margin + 10, y, 230, "Local e Data");
            AddSignatureLine(content, Margin + 270, y, 255, "Assinatura (*) e carimbo (legível) do médico com CRM/UF");
            y -= 36;

            AddPlainSection(content, "Testemunhas", ref y);
            if (testemunhas.Count == 0)
            {
                AddFormRow(content, ref y, 17, "Testemunha", "Nenhuma testemunha cadastrada.", string.Empty, string.Empty);
            }
            else
            {
                for (int i = 0; i < testemunhas.Count; i++)
                {
                    CatTestemunhaRecord testemunha = testemunhas[i];
                    AddFormRow(content, ref y, 17,
                        "Nome", testemunha.Nome,
                        "Telefone", testemunha.Telefone);
                    AddFormRow(content, ref y, 17,
                        "CPF", testemunha.Cpf,
                        "Endereço", testemunha.Endereco);
                }
            }

            AddText(content, "Cadastrada em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), Margin, 55, 7, false, "0 0 0");
            AddText(content, "* A impressão desta CAT deve ser apresentada juntamente com os documentos originais do segurado.", Margin, 42, 7, false, "0 0 0");

            EscreverPdf(arquivo, content.ToString(), logoBytes, logoWidth, logoHeight);
            return arquivo;
        }

        private static void AddCatOfficialHeader(StringBuilder content, bool hasLogo, CatRecord cat, ref double y)
        {
            if (hasLogo)
                AddImage(content, "Im1", Margin, y - 50, 72, 42);

            AddText(content, "CAT - Comunicação de Acidente de Trabalho", Margin + 154, y - 10, 16, false, "0 0 0");
            AddText(content, "Número da CAT:", Margin + 182, y - 36, 11, false, "0 0 0");
            AddText(content, cat.Id.ToString(), Margin + 275, y - 36, 11, true, "0 0 0");
            y -= 78;
        }

        private static string EmpregadorNome(CatRecord cat)
        {
            if (!string.IsNullOrWhiteSpace(cat.RazaoSocialEmpregador))
                return cat.RazaoSocialEmpregador;

            string documento = (cat.TipoInscricao + " " + cat.InscricaoEstabelecimento).Trim();
            return string.IsNullOrWhiteSpace(documento) ? string.Empty : documento;
        }

        private static void AddPlainSection(StringBuilder content, string title, ref double y)
        {
            y -= 12;
            AddText(content, title, Margin, y, 10, false, "0 0 0");
            y -= 18;
        }

        private static void AddFormRow(StringBuilder content, ref double y, double height, string label1, string value1, string label2, string value2)
        {
            double labelW = 105;
            double valueW = 190;
            double label2W = 105;
            double value2W = ContentWidth - labelW - valueW - label2W;

            AddTableCell(content, Margin, y - height, labelW, height, label1, true);
            AddTableCell(content, Margin + labelW, y - height, valueW, height, value1, false);
            AddTableCell(content, Margin + labelW + valueW, y - height, label2W, height, label2, true);
            AddTableCell(content, Margin + labelW + valueW + label2W, y - height, value2W, height, value2, false);
            y -= height;
        }

        private static void AddTableCell(StringBuilder content, double x, double y, double width, double height, string text, bool label)
        {
            AddRect(content, x, y, width, height);
            AddText(content, Fit(text, label ? 26 : 45), x + 4, y + (height / 2) - 3, 7, label, "0 0 0");
        }

        private static void AddSignatureLine(StringBuilder content, double x, double y, double width, string label)
        {
            AddLine(content, x, y, x + width, y);
            AddText(content, label, x + 70, y - 13, 7, false, "0 0 0");
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
            content.AppendLine("0 0 0 RG");
            content.AppendLine("0.6 w");
            content.AppendLine(F(x) + " " + F(y) + " " + F(width) + " " + F(height) + " re S");
        }

        private static void AddLine(StringBuilder content, double x1, double y1, double x2, double y2)
        {
            content.AppendLine("0 0 0 RG");
            content.AppendLine("0.6 w");
            content.AppendLine(F(x1) + " " + F(y1) + " m " + F(x2) + " " + F(y2) + " l S");
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
            byte[] contentBytes = Encoding.GetEncoding(1252).GetBytes(pageContent);
            string xObjects = logoBytes == null ? string.Empty : " /XObject << /Im1 7 0 R >>";
            List<byte[]> objects = new List<byte[]>
            {
                Encoding.ASCII.GetBytes("<< /Type /Catalog /Pages 2 0 R >>"),
                Encoding.ASCII.GetBytes("<< /Type /Pages /Kids [3 0 R] /Count 1 >>"),
                Encoding.ASCII.GetBytes("<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 1180] /Resources << /Font << /F1 4 0 R /F2 5 0 R >>" + xObjects + " >> /Contents 6 0 R >>"),
                Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>"),
                Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold /Encoding /WinAnsiEncoding >>"),
                Combine(
                    Encoding.ASCII.GetBytes("<< /Length " + contentBytes.Length + " >>\nstream\n"),
                    contentBytes,
                    Encoding.ASCII.GetBytes("endstream"))
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
                .Replace("Ã§", "c").Replace("Ã‡", "C")
                .Replace("Ã£", "a").Replace("Ã¡", "a").Replace("Ã ", "a").Replace("Ã¢", "a")
                .Replace("Ã©", "e").Replace("Ãª", "e")
                .Replace("Ã­", "i")
                .Replace("Ã³", "o").Replace("Ãµ", "o").Replace("Ã´", "o")
                .Replace("Ãº", "u")
                .Replace("ÃƒÂ§", "c").Replace("Ãƒâ€¡", "C")
                .Replace("ÃƒÂ£", "a").Replace("ÃƒÂ¡", "a").Replace("ÃƒÂ ", "a").Replace("ÃƒÂ¢", "a")
                .Replace("ÃƒÂ©", "e").Replace("ÃƒÂª", "e")
                .Replace("ÃƒÂ­", "i")
                .Replace("ÃƒÂ³", "o").Replace("ÃƒÂµ", "o").Replace("ÃƒÂ´", "o")
                .Replace("ÃƒÂº", "u");
        }
    }
}
