using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace SistemaTstLargoTreze
{
    public static class Database
    {
        private const string ConnectionString =
            "Server=localhost;Port=3306;Database=sistema_tst;Uid=root;Pwd=;SslMode=None;AllowPublicKeyRetrieval=True;CharSet=utf8mb4;";
        private static bool schemaChecked;

        public static MySqlConnection OpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            EnsureRuntimeSchema(connection);
            return connection;
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder(bytes.Length * 2);

                foreach (byte value in bytes)
                {
                    builder.Append(value.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private static void EnsureRuntimeSchema(MySqlConnection connection)
        {
            if (schemaChecked)
                return;

            if (!ColumnExists(connection, "asos", "cat_id"))
            {
                using (MySqlCommand command = new MySqlCommand("ALTER TABLE asos ADD COLUMN cat_id INT NULL AFTER medico_id", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            if (!ColumnExists(connection, "cats", "resultado_aso"))
            {
                using (MySqlCommand command = new MySqlCommand("ALTER TABLE cats ADD COLUMN resultado_aso VARCHAR(40) NOT NULL DEFAULT 'Aguardando ASO de Retorno' AFTER situacao", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                using (MySqlCommand command = new MySqlCommand("UPDATE cats SET resultado_aso = 'Aguardando ASO de Retorno' WHERE resultado_aso = 'Aguardando ASO'", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            if (!ColumnExists(connection, "tipos_exames", "anexo_imagem"))
            {
                using (MySqlCommand command = new MySqlCommand("ALTER TABLE tipos_exames ADD COLUMN anexo_imagem VARCHAR(500) NULL AFTER periodicidade", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            EnsureColumn(connection, "tipos_exames", "anexo_nome", "VARCHAR(255) NULL");
            EnsureColumn(connection, "tipos_exames", "anexo_tipo", "VARCHAR(100) NULL");
            EnsureColumn(connection, "tipos_exames", "anexo_arquivo", "LONGBLOB NULL");

            if (!ColumnExists(connection, "tipos_exames", "empregado_id"))
            {
                using (MySqlCommand command = new MySqlCommand("ALTER TABLE tipos_exames ADD COLUMN empregado_id INT NULL AFTER anexo_imagem", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            if (!ColumnExists(connection, "tipos_exames", "medico_id"))
            {
                using (MySqlCommand command = new MySqlCommand("ALTER TABLE tipos_exames ADD COLUMN medico_id INT NULL AFTER empregado_id", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            if (!ConstraintExists(connection, "fk_tipos_exames_empregado"))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"ALTER TABLE tipos_exames
                      ADD CONSTRAINT fk_tipos_exames_empregado
                      FOREIGN KEY (empregado_id) REFERENCES empregados (id)
                      ON DELETE SET NULL", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            if (!ConstraintExists(connection, "fk_tipos_exames_medico"))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"ALTER TABLE tipos_exames
                      ADD CONSTRAINT fk_tipos_exames_medico
                      FOREIGN KEY (medico_id) REFERENCES medicos (id)
                      ON DELETE SET NULL", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            if (!ColumnExists(connection, "empregados", "medico_id"))
            {
                using (MySqlCommand command = new MySqlCommand("ALTER TABLE empregados ADD COLUMN medico_id INT NULL AFTER status_aso", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            if (!ConstraintExists(connection, "fk_empregados_medico"))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"ALTER TABLE empregados
                      ADD CONSTRAINT fk_empregados_medico
                      FOREIGN KEY (medico_id) REFERENCES medicos (id)
                      ON DELETE SET NULL", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            EnsureActiveColumn(connection, "ambientes_trabalho");
            EnsureActiveColumn(connection, "cats");
            EnsureActiveColumn(connection, "asos");
            EnsureActiveColumn(connection, "fatores_risco");
            EnsureColumn(connection, "medicos", "uf_expedidor", "VARCHAR(2) NULL");
            EnsureColumn(connection, "medicos", "orgao_classe", "VARCHAR(120) NULL");
            EnsureColumn(connection, "medicos", "sigla", "VARCHAR(40) NULL");
            EnsureColumn(connection, "medicos", "logradouro", "VARCHAR(180) NULL");
            EnsureColumn(connection, "medicos", "bairro", "VARCHAR(120) NULL");
            EnsureColumn(connection, "medicos", "numero", "VARCHAR(30) NULL");
            EnsureColumn(connection, "medicos", "cidade", "VARCHAR(120) NULL");
            EnsureColumn(connection, "medicos", "cep", "VARCHAR(20) NULL");
            EnsureColumn(connection, "medicos", "ddd", "VARCHAR(5) NULL");
            EnsureColumn(connection, "medicos", "nit", "VARCHAR(30) NULL");
            EnsureColumn(connection, "medicos", "cpf", "VARCHAR(20) NULL");
            EnsureColumn(connection, "medicos", "telefone", "VARCHAR(30) NULL");
            EnsureColumn(connection, "medicos", "tipo_telefone", "VARCHAR(40) NULL");
            EnsureColumn(connection, "cats", "parte_corpo_atingida", "VARCHAR(180) NULL");
            EnsureColumn(connection, "cats", "lateralidade", "VARCHAR(80) NULL");
            EnsureColumn(connection, "cats", "agente_causador", "VARCHAR(180) NULL");
            EnsureColumn(connection, "cats", "cid10", "VARCHAR(80) NULL");
            EnsureColumn(connection, "cats", "natureza_lesao", "VARCHAR(180) NULL");
            EnsureColumn(connection, "cats", "duracao_tratamento", "VARCHAR(80) NULL");
            EnsureColumn(connection, "cats", "medico_id", "INT NULL");
            EnsureColumn(connection, "cats", "medico_assistente", "VARCHAR(180) NULL");
            EnsureColumn(connection, "cats", "observacao_medica", "TEXT NULL");
            EnsureColumn(connection, "cats", "aposentado", "TINYINT(1) NOT NULL DEFAULT 0");
            EnsureColumn(connection, "cats", "area", "VARCHAR(40) NULL");
            EnsureColumn(connection, "cats", "filiacao_prev_social", "VARCHAR(80) NULL");
            EnsureColumn(connection, "cats", "emitente", "VARCHAR(80) NULL");
            EnsureColumn(connection, "cats", "tipo_acidente", "VARCHAR(80) NULL");
            EnsureColumn(connection, "cats", "horas_trabalhadas_antes", "VARCHAR(20) NULL");
            EnsureColumn(connection, "cats", "houve_obito", "TINYINT(1) NOT NULL DEFAULT 0");
            EnsureColumn(connection, "cats", "data_obito", "DATE NULL");
            EnsureColumn(connection, "cats", "houve_afastamento", "TINYINT(1) NOT NULL DEFAULT 0");
            EnsureColumn(connection, "cats", "registro_policia", "TINYINT(1) NOT NULL DEFAULT 0");
            EnsureColumn(connection, "cats", "ultimo_dia_trabalho", "DATE NULL");
            EnsureColumn(connection, "cats", "codificacao_acidente", "VARCHAR(80) NULL");
            EnsureColumn(connection, "cats", "situacao_geradora", "VARCHAR(255) NULL");
            EnsureColumn(connection, "cats", "cat_emitida_por", "VARCHAR(120) NULL");
            EnsureColumn(connection, "cats", "especificacao_local", "VARCHAR(180) NULL");
            EnsureColumn(connection, "cats", "tipo_logradouro", "VARCHAR(60) NULL");
            EnsureColumn(connection, "cats", "numero", "VARCHAR(30) NULL");
            EnsureColumn(connection, "cats", "tipo_inscricao", "VARCHAR(30) NULL");
            EnsureColumn(connection, "cats", "inscricao_estabelecimento", "VARCHAR(40) NULL");
            EnsureColumn(connection, "cats", "logradouro", "VARCHAR(180) NULL");
            EnsureColumn(connection, "cats", "municipio", "VARCHAR(120) NULL");
            EnsureColumn(connection, "cats", "uf", "VARCHAR(2) NULL");
            EnsureColumn(connection, "cats", "bairro", "VARCHAR(120) NULL");
            EnsureColumn(connection, "cats", "complemento", "VARCHAR(120) NULL");
            EnsureColumn(connection, "cats", "cep", "VARCHAR(20) NULL");
            EnsureColumn(connection, "cats", "codigo_postal", "VARCHAR(30) NULL");
            EnsureColumn(connection, "cats", "observacao_cat", "TEXT NULL");
            EnsureColumn(connection, "cats", "razao_social_empregador", "VARCHAR(255) NULL");
            EnsureColumn(connection, "cats", "cnae_empregador", "VARCHAR(255) NULL");
            EnsureColumn(connection, "fatores_risco", "epi_descricao", "TEXT NULL");
            EnsureColumn(connection, "esocial_eventos", "origem_tipo", "VARCHAR(30) NULL");
            EnsureColumn(connection, "esocial_eventos", "origem_id", "INT NULL");
            EnsureCatTestemunhasTable(connection);
            EnsureColumn(connection, "cat_testemunhas", "endereco", "VARCHAR(255) NULL");

            if (!ConstraintExists(connection, "fk_cats_medico"))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"ALTER TABLE cats
                      ADD CONSTRAINT fk_cats_medico
                      FOREIGN KEY (medico_id) REFERENCES medicos (id)
                      ON DELETE SET NULL", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            schemaChecked = true;
        }

        private static void EnsureCatTestemunhasTable(MySqlConnection connection)
        {
            using (MySqlCommand command = new MySqlCommand(
                @"CREATE TABLE IF NOT EXISTS cat_testemunhas (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    cat_id INT NOT NULL,
                    nome VARCHAR(180) NOT NULL,
                    cpf VARCHAR(20),
                    telefone VARCHAR(30),
                    endereco VARCHAR(255),
                    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    CONSTRAINT fk_cat_testemunhas_cat
                        FOREIGN KEY (cat_id) REFERENCES cats (id)
                        ON DELETE CASCADE
                ) ENGINE=InnoDB", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static void EnsureColumn(MySqlConnection connection, string tableName, string columnName, string definition)
        {
            if (ColumnExists(connection, tableName, columnName))
                return;

            using (MySqlCommand command = new MySqlCommand("ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " " + definition, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static void EnsureActiveColumn(MySqlConnection connection, string tableName)
        {
            if (ColumnExists(connection, tableName, "ativo"))
                return;

            using (MySqlCommand command = new MySqlCommand("ALTER TABLE " + tableName + " ADD COLUMN ativo TINYINT(1) NOT NULL DEFAULT 1", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static bool ColumnExists(MySqlConnection connection, string tableName, string columnName)
        {
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT COUNT(*)
                  FROM information_schema.COLUMNS
                  WHERE TABLE_SCHEMA = DATABASE()
                    AND TABLE_NAME = @table
                    AND COLUMN_NAME = @column", connection))
            {
                command.Parameters.AddWithValue("@table", tableName);
                command.Parameters.AddWithValue("@column", columnName);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private static bool ConstraintExists(MySqlConnection connection, string constraintName)
        {
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT COUNT(*)
                  FROM information_schema.TABLE_CONSTRAINTS
                  WHERE TABLE_SCHEMA = DATABASE()
                    AND CONSTRAINT_NAME = @constraint", connection))
            {
                command.Parameters.AddWithValue("@constraint", constraintName);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }
    }

    public sealed class MedicoRecord
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Crm { get; set; }
        public string OrgaoUf { get; set; }
        public string Especialidade { get; set; }
        public string Email { get; set; }
        public string UfExpedidor { get; set; }
        public string OrgaoClasse { get; set; }
        public string Sigla { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public string Ddd { get; set; }
        public string Nit { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string TipoTelefone { get; set; }
    }

    public sealed class TipoExameRecord
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Periodicidade { get; set; }
        public string AnexoImagem { get; set; }
        public string AnexoNome { get; set; }
        public string AnexoTipo { get; set; }
        public byte[] AnexoArquivo { get; set; }
        public int? EmpregadoId { get; set; }
        public string PacienteNome { get; set; }
        public int? MedicoId { get; set; }
        public string MedicoNome { get; set; }
    }

    public sealed class AmbienteTrabalhoRecord
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Ambiente { get; set; }
        public string Setor { get; set; }
        public string Status { get; set; }
    }

    public sealed class EmpregadoRecord
    {
        public int Id { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Setor { get; set; }
        public string Cargo { get; set; }
        public string DataAdmissao { get; set; }
        public string DataVencimentoAso { get; set; }
        public string StatusAso { get; set; }
        public int? MedicoId { get; set; }
        public string MedicoNome { get; set; }
    }

    public sealed class CatRecord
    {
        public int Id { get; set; }
        public int EmpregadoId { get; set; }
        public string EmpregadoNome { get; set; }
        public string DataAcidente { get; set; }
        public string HoraAcidente { get; set; }
        public string DataComunicacao { get; set; }
        public string DataObito { get; set; }
        public string LocalAcidente { get; set; }
        public string Descricao { get; set; }
        public string TipoCat { get; set; }
        public string Situacao { get; set; }
        public string ResultadoAso { get; set; }
        public string ParteCorpoAtingida { get; set; }
        public string Lateralidade { get; set; }
        public string AgenteCausador { get; set; }
        public string Cid10 { get; set; }
        public string NaturezaLesao { get; set; }
        public string DuracaoTratamento { get; set; }
        public int? MedicoId { get; set; }
        public string MedicoAssistente { get; set; }
        public string ObservacaoMedica { get; set; }
        public bool Aposentado { get; set; }
        public string Area { get; set; }
        public string FiliacaoPrevSocial { get; set; }
        public string Emitente { get; set; }
        public string TipoAcidente { get; set; }
        public string HorasTrabalhadasAntes { get; set; }
        public bool HouveObito { get; set; }
        public bool HouveAfastamento { get; set; }
        public bool RegistroPolicia { get; set; }
        public string UltimoDiaTrabalho { get; set; }
        public string CodificacaoAcidente { get; set; }
        public string SituacaoGeradora { get; set; }
        public string CatEmitidaPor { get; set; }
        public string EspecificacaoLocal { get; set; }
        public string TipoLogradouro { get; set; }
        public string Numero { get; set; }
        public string TipoInscricao { get; set; }
        public string InscricaoEstabelecimento { get; set; }
        public string Logradouro { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string CodigoPostal { get; set; }
        public string ObservacaoCat { get; set; }
        public string RazaoSocialEmpregador { get; set; }
        public string CnaeEmpregador { get; set; }
    }

    public sealed class CatTestemunhaRecord
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
    }

    public sealed class AsoRecord
    {
        public int Id { get; set; }
        public int EmpregadoId { get; set; }
        public string EmpregadoNome { get; set; }
        public int MedicoId { get; set; }
        public string MedicoNome { get; set; }
        public int? CatId { get; set; }
        public string DataAso { get; set; }
        public string TipoExame { get; set; }
        public string Resultado { get; set; }
        public string Observacoes { get; set; }
    }

    public sealed class AsoExameRecord
    {
        public int Id { get; set; }
        public int AsoId { get; set; }
        public int TipoExameId { get; set; }
        public string TipoExameNome { get; set; }
        public string DataExame { get; set; }
        public string Resultado { get; set; }
        public string Observacoes { get; set; }
    }

    public sealed class RiskFactorRecord
    {
        public int Id { get; set; }
        public int? EmpregadoId { get; set; }
        public string EmpregadoNome { get; set; }
        public int? AmbienteId { get; set; }
        public string AmbienteNome { get; set; }
        public string TipoFator { get; set; }
        public string Agente { get; set; }
        public string Intensidade { get; set; }
        public string TecnicaMedicao { get; set; }
        public string DataAvaliacao { get; set; }
        public string InicioExposicao { get; set; }
        public string FimExposicao { get; set; }
        public string DescricaoAtividades { get; set; }
        public bool UsaEpi { get; set; }
        public bool EpiEficaz { get; set; }
        public string EpisSelecionados { get; set; }
    }

    public sealed class CatStatusResumo
    {
        public int Aptos { get; set; }
        public int Inaptos { get; set; }
        public int Aguardando { get; set; }
        public int Total { get; set; }
    }

    public sealed class EsocialTransmissaoRecord
    {
        public int Id { get; set; }
        public string DataHora { get; set; }
        public string Evento { get; set; }
        public string Trabalhador { get; set; }
        public string Protocolo { get; set; }
        public string Status { get; set; }
        public string Recibo { get; set; }
    }

    public sealed class ComboItem
    {
        public int Id { get; private set; }
        public string Text { get; private set; }

        public ComboItem(int id, string text)
        {
            Id = id;
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }

    public static class UserRepository
    {
        public static UserAccount ValidateLogin(string login, string password)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT nome, email, senha_hash
                  FROM usuarios
                  WHERE ativo = 1 AND email = @login
                  LIMIT 1", connection))
            {
                command.Parameters.AddWithValue("@login", login);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    string savedHash = reader.GetString("senha_hash");
                    if (!string.Equals(savedHash, Database.HashPassword(password), StringComparison.OrdinalIgnoreCase))
                        return null;

                    return new UserAccount
                    {
                        Name = reader.GetString("nome"),
                        Email = reader.GetString("email"),
                        Login = reader.GetString("email")
                    };
                }
            }
        }

        public static void Register(string name, string email, string password)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"INSERT INTO usuarios (nome, email, senha_hash)
                  VALUES (@nome, @email, @senha_hash)
                  ON DUPLICATE KEY UPDATE
                      nome = VALUES(nome),
                      senha_hash = VALUES(senha_hash),
                      ativo = 1", connection))
            {
                command.Parameters.AddWithValue("@nome", name);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@senha_hash", Database.HashPassword(password));
                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows == 0)
                    throw new InvalidOperationException("E-mail nao encontrado no banco de dados.");
            }
        }

        public static void UpdatePassword(string email, string password)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"UPDATE usuarios
                  SET senha_hash = @senha_hash
                  WHERE email = @email", connection))
            {
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@senha_hash", Database.HashPassword(password));
                command.ExecuteNonQuery();
            }
        }
    }

    public static class CadastrosRepository
    {
        public static List<MedicoRecord> GetMedicos()
        {
            List<MedicoRecord> medicos = new List<MedicoRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT id, nome, crm, orgao_uf, especialidade, email,
                         uf_expedidor, orgao_classe, sigla, logradouro, bairro, numero, cidade, cep,
                         ddd, nit, cpf, telefone, tipo_telefone
                  FROM medicos
                  WHERE ativo = 1
                  ORDER BY nome", connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    medicos.Add(new MedicoRecord
                    {
                        Id = reader.GetInt32("id"),
                        Nome = reader.GetString("nome"),
                        Crm = reader.GetString("crm"),
                        OrgaoUf = reader.GetString("orgao_uf"),
                        Especialidade = reader.GetString("especialidade"),
                        Email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString("email"),
                        UfExpedidor = ReaderString(reader, "uf_expedidor"),
                        OrgaoClasse = ReaderString(reader, "orgao_classe"),
                        Sigla = ReaderString(reader, "sigla"),
                        Logradouro = ReaderString(reader, "logradouro"),
                        Bairro = ReaderString(reader, "bairro"),
                        Numero = ReaderString(reader, "numero"),
                        Cidade = ReaderString(reader, "cidade"),
                        Cep = ReaderString(reader, "cep"),
                        Ddd = ReaderString(reader, "ddd"),
                        Nit = ReaderString(reader, "nit"),
                        Cpf = ReaderString(reader, "cpf"),
                        Telefone = ReaderString(reader, "telefone"),
                        TipoTelefone = ReaderString(reader, "tipo_telefone")
                    });
                }
            }

            return medicos;
        }

        public static MedicoRecord GetMedico(int id)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT id, nome, crm, orgao_uf, especialidade, email,
                         uf_expedidor, orgao_classe, sigla, logradouro, bairro, numero, cidade, cep,
                         ddd, nit, cpf, telefone, tipo_telefone
                  FROM medicos
                  WHERE id = @id
                  LIMIT 1", connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new MedicoRecord
                    {
                        Id = reader.GetInt32("id"),
                        Nome = reader.GetString("nome"),
                        Crm = reader.GetString("crm"),
                        OrgaoUf = reader.GetString("orgao_uf"),
                        Especialidade = reader.GetString("especialidade"),
                        Email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString("email"),
                        UfExpedidor = ReaderString(reader, "uf_expedidor"),
                        OrgaoClasse = ReaderString(reader, "orgao_classe"),
                        Sigla = ReaderString(reader, "sigla"),
                        Logradouro = ReaderString(reader, "logradouro"),
                        Bairro = ReaderString(reader, "bairro"),
                        Numero = ReaderString(reader, "numero"),
                        Cidade = ReaderString(reader, "cidade"),
                        Cep = ReaderString(reader, "cep"),
                        Ddd = ReaderString(reader, "ddd"),
                        Nit = ReaderString(reader, "nit"),
                        Cpf = ReaderString(reader, "cpf"),
                        Telefone = ReaderString(reader, "telefone"),
                        TipoTelefone = ReaderString(reader, "tipo_telefone")
                    };
                }
            }
        }

        public static void SaveMedico(MedicoRecord medico)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                medico.Id > 0
                    ? @"UPDATE medicos
                        SET nome = @nome, crm = @crm, orgao_uf = @orgao_uf, especialidade = @especialidade, email = @email,
                            uf_expedidor = @uf_expedidor, orgao_classe = @orgao_classe, sigla = @sigla,
                            logradouro = @logradouro, bairro = @bairro, numero = @numero, cidade = @cidade, cep = @cep,
                            ddd = @ddd, nit = @nit, cpf = @cpf, telefone = @telefone, tipo_telefone = @tipo_telefone
                        WHERE id = @id"
                    : @"INSERT INTO medicos (nome, crm, orgao_uf, especialidade, email,
                            uf_expedidor, orgao_classe, sigla, logradouro, bairro, numero, cidade, cep,
                            ddd, nit, cpf, telefone, tipo_telefone)
                        VALUES (@nome, @crm, @orgao_uf, @especialidade, @email,
                            @uf_expedidor, @orgao_classe, @sigla, @logradouro, @bairro, @numero, @cidade, @cep,
                            @ddd, @nit, @cpf, @telefone, @tipo_telefone)", connection))
            {
                command.Parameters.AddWithValue("@id", medico.Id);
                command.Parameters.AddWithValue("@nome", medico.Nome);
                command.Parameters.AddWithValue("@crm", medico.Crm);
                command.Parameters.AddWithValue("@orgao_uf", medico.OrgaoUf);
                command.Parameters.AddWithValue("@especialidade", medico.Especialidade);
                command.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(medico.Email) ? (object)DBNull.Value : medico.Email);
                command.Parameters.AddWithValue("@uf_expedidor", EmptyToDbNull(medico.UfExpedidor));
                command.Parameters.AddWithValue("@orgao_classe", EmptyToDbNull(medico.OrgaoClasse));
                command.Parameters.AddWithValue("@sigla", EmptyToDbNull(medico.Sigla));
                command.Parameters.AddWithValue("@logradouro", EmptyToDbNull(medico.Logradouro));
                command.Parameters.AddWithValue("@bairro", EmptyToDbNull(medico.Bairro));
                command.Parameters.AddWithValue("@numero", EmptyToDbNull(medico.Numero));
                command.Parameters.AddWithValue("@cidade", EmptyToDbNull(medico.Cidade));
                command.Parameters.AddWithValue("@cep", EmptyToDbNull(medico.Cep));
                command.Parameters.AddWithValue("@ddd", EmptyToDbNull(medico.Ddd));
                command.Parameters.AddWithValue("@nit", EmptyToDbNull(medico.Nit));
                command.Parameters.AddWithValue("@cpf", EmptyToDbNull(medico.Cpf));
                command.Parameters.AddWithValue("@telefone", EmptyToDbNull(medico.Telefone));
                command.Parameters.AddWithValue("@tipo_telefone", EmptyToDbNull(medico.TipoTelefone));
                command.ExecuteNonQuery();
            }
        }

        public static List<TipoExameRecord> GetTiposExames()
        {
            List<TipoExameRecord> exames = new List<TipoExameRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT t.id, t.codigo, t.nome, t.tipo, t.periodicidade, t.anexo_imagem, t.anexo_nome, t.anexo_tipo,
                         t.empregado_id, e.nome AS paciente_nome, t.medico_id, m.nome AS medico_nome
                  FROM tipos_exames t
                  LEFT JOIN empregados e ON e.id = t.empregado_id
                  LEFT JOIN medicos m ON m.id = t.medico_id
                  WHERE t.ativo = 1
                  ORDER BY t.nome", connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    exames.Add(new TipoExameRecord
                    {
                        Id = reader.GetInt32("id"),
                        Codigo = reader.GetString("codigo"),
                        Nome = reader.GetString("nome"),
                        Tipo = reader.GetString("tipo"),
                        Periodicidade = reader.GetString("periodicidade"),
                        AnexoImagem = reader.IsDBNull(reader.GetOrdinal("anexo_imagem")) ? string.Empty : reader.GetString("anexo_imagem"),
                        AnexoNome = ReaderString(reader, "anexo_nome"),
                        AnexoTipo = ReaderString(reader, "anexo_tipo"),
                        EmpregadoId = reader.IsDBNull(reader.GetOrdinal("empregado_id")) ? (int?)null : reader.GetInt32("empregado_id"),
                        PacienteNome = reader.IsDBNull(reader.GetOrdinal("paciente_nome")) ? string.Empty : reader.GetString("paciente_nome"),
                        MedicoId = reader.IsDBNull(reader.GetOrdinal("medico_id")) ? (int?)null : reader.GetInt32("medico_id"),
                        MedicoNome = reader.IsDBNull(reader.GetOrdinal("medico_nome")) ? string.Empty : reader.GetString("medico_nome")
                    });
                }
            }

            return exames;
        }

        public static List<TipoExameRecord> GetTiposExamesPorEmpregado(int empregadoId)
        {
            List<TipoExameRecord> exames = new List<TipoExameRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT t.id, t.codigo, t.nome, t.tipo, t.periodicidade, t.anexo_imagem, t.anexo_nome, t.anexo_tipo,
                         t.empregado_id, e.nome AS paciente_nome, t.medico_id, m.nome AS medico_nome
                  FROM tipos_exames t
                  LEFT JOIN empregados e ON e.id = t.empregado_id
                  LEFT JOIN medicos m ON m.id = t.medico_id
                  WHERE t.ativo = 1
                    AND t.empregado_id = @empregado_id
                  ORDER BY t.nome", connection))
            {
                command.Parameters.AddWithValue("@empregado_id", empregadoId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        exames.Add(new TipoExameRecord
                        {
                            Id = reader.GetInt32("id"),
                            Codigo = reader.GetString("codigo"),
                            Nome = reader.GetString("nome"),
                            Tipo = reader.GetString("tipo"),
                            Periodicidade = reader.GetString("periodicidade"),
                            AnexoImagem = reader.IsDBNull(reader.GetOrdinal("anexo_imagem")) ? string.Empty : reader.GetString("anexo_imagem"),
                            AnexoNome = ReaderString(reader, "anexo_nome"),
                            AnexoTipo = ReaderString(reader, "anexo_tipo"),
                            EmpregadoId = reader.IsDBNull(reader.GetOrdinal("empregado_id")) ? (int?)null : reader.GetInt32("empregado_id"),
                            PacienteNome = reader.IsDBNull(reader.GetOrdinal("paciente_nome")) ? string.Empty : reader.GetString("paciente_nome"),
                            MedicoId = reader.IsDBNull(reader.GetOrdinal("medico_id")) ? (int?)null : reader.GetInt32("medico_id"),
                            MedicoNome = reader.IsDBNull(reader.GetOrdinal("medico_nome")) ? string.Empty : reader.GetString("medico_nome")
                        });
                    }
                }
            }

            return exames;
        }

        public static TipoExameRecord GetTipoExame(int id)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT t.id, t.codigo, t.nome, t.tipo, t.periodicidade, t.anexo_imagem, t.anexo_nome, t.anexo_tipo, t.anexo_arquivo,
                         t.empregado_id, e.nome AS paciente_nome, t.medico_id, m.nome AS medico_nome
                  FROM tipos_exames t
                  LEFT JOIN empregados e ON e.id = t.empregado_id
                  LEFT JOIN medicos m ON m.id = t.medico_id
                  WHERE t.id = @id
                  LIMIT 1", connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new TipoExameRecord
                    {
                        Id = reader.GetInt32("id"),
                        Codigo = reader.GetString("codigo"),
                        Nome = reader.GetString("nome"),
                        Tipo = reader.GetString("tipo"),
                        Periodicidade = reader.GetString("periodicidade"),
                        AnexoImagem = reader.IsDBNull(reader.GetOrdinal("anexo_imagem")) ? string.Empty : reader.GetString("anexo_imagem"),
                        AnexoNome = ReaderString(reader, "anexo_nome"),
                        AnexoTipo = ReaderString(reader, "anexo_tipo"),
                        AnexoArquivo = reader.IsDBNull(reader.GetOrdinal("anexo_arquivo")) ? null : (byte[])reader["anexo_arquivo"],
                        EmpregadoId = reader.IsDBNull(reader.GetOrdinal("empregado_id")) ? (int?)null : reader.GetInt32("empregado_id"),
                        PacienteNome = reader.IsDBNull(reader.GetOrdinal("paciente_nome")) ? string.Empty : reader.GetString("paciente_nome"),
                        MedicoId = reader.IsDBNull(reader.GetOrdinal("medico_id")) ? (int?)null : reader.GetInt32("medico_id"),
                        MedicoNome = reader.IsDBNull(reader.GetOrdinal("medico_nome")) ? string.Empty : reader.GetString("medico_nome")
                    };
                }
            }
        }

        public static TipoExameRecord GetTipoExameAnexo(int id)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT id, anexo_nome, anexo_tipo, anexo_arquivo
                  FROM tipos_exames
                  WHERE id = @id
                  LIMIT 1", connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new TipoExameRecord
                    {
                        Id = reader.GetInt32("id"),
                        AnexoNome = ReaderString(reader, "anexo_nome"),
                        AnexoTipo = ReaderString(reader, "anexo_tipo"),
                        AnexoArquivo = reader.IsDBNull(reader.GetOrdinal("anexo_arquivo")) ? null : (byte[])reader["anexo_arquivo"]
                    };
                }
            }
        }

        public static void SaveTipoExame(TipoExameRecord exame)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                exame.Id > 0
                    ? @"UPDATE tipos_exames
                        SET codigo = @codigo, nome = @nome, tipo = @tipo, periodicidade = @periodicidade,
                            anexo_imagem = @anexo_imagem, anexo_nome = @anexo_nome, anexo_tipo = @anexo_tipo, anexo_arquivo = @anexo_arquivo,
                            empregado_id = @empregado_id, medico_id = @medico_id
                        WHERE id = @id"
                    : @"INSERT INTO tipos_exames (codigo, nome, tipo, periodicidade, anexo_imagem, anexo_nome, anexo_tipo, anexo_arquivo, empregado_id, medico_id)
                        VALUES (@codigo, @nome, @tipo, @periodicidade, @anexo_imagem, @anexo_nome, @anexo_tipo, @anexo_arquivo, @empregado_id, @medico_id)", connection))
            {
                command.Parameters.AddWithValue("@id", exame.Id);
                command.Parameters.AddWithValue("@codigo", exame.Codigo);
                command.Parameters.AddWithValue("@nome", exame.Nome);
                command.Parameters.AddWithValue("@tipo", exame.Tipo);
                command.Parameters.AddWithValue("@periodicidade", exame.Periodicidade);
                command.Parameters.AddWithValue("@anexo_imagem", EmptyToDbNull(exame.AnexoImagem));
                command.Parameters.AddWithValue("@anexo_nome", EmptyToDbNull(exame.AnexoNome));
                command.Parameters.AddWithValue("@anexo_tipo", EmptyToDbNull(exame.AnexoTipo));
                command.Parameters.AddWithValue("@anexo_arquivo", exame.AnexoArquivo == null || exame.AnexoArquivo.Length == 0 ? (object)DBNull.Value : exame.AnexoArquivo);
                command.Parameters.AddWithValue("@empregado_id", exame.EmpregadoId.HasValue && exame.EmpregadoId.Value > 0 ? (object)exame.EmpregadoId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@medico_id", exame.MedicoId.HasValue && exame.MedicoId.Value > 0 ? (object)exame.MedicoId.Value : DBNull.Value);
                command.ExecuteNonQuery();
            }
        }

        public static List<AmbienteTrabalhoRecord> GetAmbientes()
        {
            List<AmbienteTrabalhoRecord> ambientes = new List<AmbienteTrabalhoRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT id, codigo, ambiente, setor, status
                  FROM ambientes_trabalho
                  WHERE ativo = 1
                  ORDER BY ambiente", connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ambientes.Add(new AmbienteTrabalhoRecord
                    {
                        Id = reader.GetInt32("id"),
                        Codigo = reader.GetString("codigo"),
                        Ambiente = reader.GetString("ambiente"),
                        Setor = reader.GetString("setor"),
                        Status = reader.GetString("status")
                    });
                }
            }

            return ambientes;
        }

        public static AmbienteTrabalhoRecord GetAmbiente(int id)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT id, codigo, ambiente, setor, status
                  FROM ambientes_trabalho
                  WHERE id = @id AND ativo = 1
                  LIMIT 1", connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new AmbienteTrabalhoRecord
                    {
                        Id = reader.GetInt32("id"),
                        Codigo = reader.GetString("codigo"),
                        Ambiente = reader.GetString("ambiente"),
                        Setor = reader.GetString("setor"),
                        Status = reader.GetString("status")
                    };
                }
            }
        }

        public static void SaveAmbiente(AmbienteTrabalhoRecord ambiente)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                ambiente.Id > 0
                    ? @"UPDATE ambientes_trabalho
                        SET codigo = @codigo, ambiente = @ambiente, setor = @setor, status = @status
                        WHERE id = @id"
                    : @"INSERT INTO ambientes_trabalho (codigo, ambiente, setor, status)
                        VALUES (@codigo, @ambiente, @setor, @status)", connection))
            {
                command.Parameters.AddWithValue("@id", ambiente.Id);
                command.Parameters.AddWithValue("@codigo", ambiente.Codigo);
                command.Parameters.AddWithValue("@ambiente", ambiente.Ambiente);
                command.Parameters.AddWithValue("@setor", ambiente.Setor);
                command.Parameters.AddWithValue("@status", ambiente.Status);
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteMedicos(IEnumerable<int> ids)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            {
                foreach (int id in ids)
                {
                    DeleteEsocialEventos(connection, "S-2220", "SELECT id FROM asos WHERE medico_id = @id", id);
                    ExecuteNonQuery(connection, "DELETE FROM aso_exames WHERE aso_id IN (SELECT id FROM asos WHERE medico_id = @id)", id);
                    ExecuteNonQuery(connection, "DELETE FROM aso_exames WHERE tipo_exame_id IN (SELECT id FROM tipos_exames WHERE medico_id = @id)", id);
                    ExecuteNonQuery(connection, "DELETE FROM asos WHERE medico_id = @id", id);
                    ExecuteNonQuery(connection, "DELETE FROM tipos_exames WHERE medico_id = @id", id);
                    ExecuteNonQuery(connection, "UPDATE empregados SET medico_id = NULL WHERE medico_id = @id", id);
                    ExecuteNonQuery(connection, "UPDATE cats SET medico_id = NULL WHERE medico_id = @id", id);
                    ExecuteNonQuery(connection, "DELETE FROM medicos WHERE id = @id", id);
                }
            }
        }

        public static void DeleteTiposExames(IEnumerable<int> ids)
        {
            DeleteByIdsWithChildren("tipos_exames", ids, delegate(MySqlConnection connection, int id)
            {
                ExecuteNonQuery(connection, "DELETE FROM aso_exames WHERE tipo_exame_id = @id", id);
            });
        }

        public static void DeleteAmbientes(IEnumerable<int> ids)
        {
            DeleteByIdsWithChildren("ambientes_trabalho", ids, delegate(MySqlConnection connection, int id)
            {
                DeleteEsocialEventos(connection, "S-2240", "SELECT id FROM fatores_risco WHERE ambiente_id = @id", id);
                ExecuteNonQuery(connection, "DELETE FROM fatores_risco WHERE ambiente_id = @id", id);
            });
        }

        public static List<EmpregadoRecord> GetEmpregados()
        {
            List<EmpregadoRecord> empregados = new List<EmpregadoRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT e.id, e.matricula, e.nome, e.cpf, e.setor, e.cargo, e.data_admissao,
                         e.data_vencimento_aso, e.status_aso, e.medico_id, m.nome AS medico_nome
                  FROM empregados e
                  LEFT JOIN medicos m ON m.id = e.medico_id
                  WHERE e.ativo = 1
                  ORDER BY e.nome", connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    empregados.Add(new EmpregadoRecord
                    {
                        Id = reader.GetInt32("id"),
                        Matricula = reader.GetString("matricula"),
                        Nome = reader.GetString("nome"),
                        Cpf = reader.GetString("cpf"),
                        Setor = reader.IsDBNull(reader.GetOrdinal("setor")) ? string.Empty : reader.GetString("setor"),
                        Cargo = reader.IsDBNull(reader.GetOrdinal("cargo")) ? string.Empty : reader.GetString("cargo"),
                        DataAdmissao = reader.IsDBNull(reader.GetOrdinal("data_admissao")) ? string.Empty : reader.GetDateTime("data_admissao").ToString("dd/MM/yyyy"),
                        DataVencimentoAso = reader.IsDBNull(reader.GetOrdinal("data_vencimento_aso")) ? string.Empty : reader.GetDateTime("data_vencimento_aso").ToString("dd/MM/yyyy"),
                        StatusAso = reader.GetString("status_aso"),
                        MedicoId = reader.IsDBNull(reader.GetOrdinal("medico_id")) ? (int?)null : reader.GetInt32("medico_id"),
                        MedicoNome = reader.IsDBNull(reader.GetOrdinal("medico_nome")) ? string.Empty : reader.GetString("medico_nome")
                    });
                }
            }

            return empregados;
        }

        public static EmpregadoRecord GetEmpregado(int id)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT e.id, e.matricula, e.nome, e.cpf, e.setor, e.cargo, e.data_admissao,
                         e.data_vencimento_aso, e.status_aso, e.medico_id, m.nome AS medico_nome
                  FROM empregados e
                  LEFT JOIN medicos m ON m.id = e.medico_id
                  WHERE e.id = @id
                  LIMIT 1", connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new EmpregadoRecord
                    {
                        Id = reader.GetInt32("id"),
                        Matricula = reader.GetString("matricula"),
                        Nome = reader.GetString("nome"),
                        Cpf = reader.GetString("cpf"),
                        Setor = reader.IsDBNull(reader.GetOrdinal("setor")) ? string.Empty : reader.GetString("setor"),
                        Cargo = reader.IsDBNull(reader.GetOrdinal("cargo")) ? string.Empty : reader.GetString("cargo"),
                        DataAdmissao = reader.IsDBNull(reader.GetOrdinal("data_admissao")) ? string.Empty : reader.GetDateTime("data_admissao").ToString("dd/MM/yyyy"),
                        DataVencimentoAso = reader.IsDBNull(reader.GetOrdinal("data_vencimento_aso")) ? string.Empty : reader.GetDateTime("data_vencimento_aso").ToString("dd/MM/yyyy"),
                        StatusAso = reader.GetString("status_aso"),
                        MedicoId = reader.IsDBNull(reader.GetOrdinal("medico_id")) ? (int?)null : reader.GetInt32("medico_id"),
                        MedicoNome = reader.IsDBNull(reader.GetOrdinal("medico_nome")) ? string.Empty : reader.GetString("medico_nome")
                    };
                }
            }
        }

        public static void SaveEmpregado(EmpregadoRecord empregado)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                empregado.Id > 0
                    ? @"UPDATE empregados
                        SET matricula = @matricula, nome = @nome, cpf = @cpf, setor = @setor, cargo = @cargo,
                            data_admissao = @data_admissao, data_vencimento_aso = @data_vencimento_aso,
                            status_aso = @status_aso, medico_id = @medico_id
                        WHERE id = @id"
                    : @"INSERT INTO empregados (matricula, nome, cpf, setor, cargo, data_admissao, data_vencimento_aso, status_aso, medico_id)
                        VALUES (@matricula, @nome, @cpf, @setor, @cargo, @data_admissao, @data_vencimento_aso, @status_aso, @medico_id)", connection))
            {
                command.Parameters.AddWithValue("@id", empregado.Id);
                command.Parameters.AddWithValue("@matricula", empregado.Matricula);
                command.Parameters.AddWithValue("@nome", empregado.Nome);
                command.Parameters.AddWithValue("@cpf", empregado.Cpf);
                command.Parameters.AddWithValue("@setor", EmptyToDbNull(empregado.Setor));
                command.Parameters.AddWithValue("@cargo", EmptyToDbNull(empregado.Cargo));
                command.Parameters.AddWithValue("@data_admissao", DateToDbNull(empregado.DataAdmissao));
                command.Parameters.AddWithValue("@data_vencimento_aso", DateToDbNull(empregado.DataVencimentoAso));
                command.Parameters.AddWithValue("@status_aso", string.IsNullOrWhiteSpace(empregado.StatusAso) ? "Pendente" : empregado.StatusAso);
                command.Parameters.AddWithValue("@medico_id", empregado.MedicoId.HasValue && empregado.MedicoId.Value > 0 ? (object)empregado.MedicoId.Value : DBNull.Value);
                command.ExecuteNonQuery();
            }
        }

        public static int ImportEmpregados(IEnumerable<EmpregadoRecord> empregados)
        {
            int total = 0;

            using (MySqlConnection connection = Database.OpenConnection())
            {
                foreach (EmpregadoRecord empregado in empregados)
                {
                    if (empregado == null ||
                        string.IsNullOrWhiteSpace(empregado.Matricula) ||
                        string.IsNullOrWhiteSpace(empregado.Nome) ||
                        string.IsNullOrWhiteSpace(empregado.Cpf))
                    {
                        continue;
                    }

                    using (MySqlCommand command = new MySqlCommand(
                        @"INSERT INTO empregados
                            (matricula, nome, cpf, setor, cargo, data_admissao, data_vencimento_aso, status_aso, medico_id, ativo)
                          VALUES
                            (@matricula, @nome, @cpf, @setor, @cargo, @data_admissao, @data_vencimento_aso, @status_aso, @medico_id, 1)
                          ON DUPLICATE KEY UPDATE
                            matricula = VALUES(matricula),
                            nome = VALUES(nome),
                            cpf = VALUES(cpf),
                            setor = VALUES(setor),
                            cargo = VALUES(cargo),
                            data_admissao = VALUES(data_admissao),
                            data_vencimento_aso = VALUES(data_vencimento_aso),
                            status_aso = VALUES(status_aso),
                            medico_id = VALUES(medico_id),
                            ativo = 1", connection))
                    {
                        command.Parameters.AddWithValue("@matricula", empregado.Matricula.Trim());
                        command.Parameters.AddWithValue("@nome", empregado.Nome.Trim());
                        command.Parameters.AddWithValue("@cpf", empregado.Cpf.Trim());
                        command.Parameters.AddWithValue("@setor", EmptyToDbNull(empregado.Setor));
                        command.Parameters.AddWithValue("@cargo", EmptyToDbNull(empregado.Cargo));
                        command.Parameters.AddWithValue("@data_admissao", DateToDbNull(empregado.DataAdmissao));
                        command.Parameters.AddWithValue("@data_vencimento_aso", DateToDbNull(empregado.DataVencimentoAso));
                        command.Parameters.AddWithValue("@status_aso", string.IsNullOrWhiteSpace(empregado.StatusAso) ? "Pendente" : empregado.StatusAso);
                        command.Parameters.AddWithValue("@medico_id", empregado.MedicoId.HasValue && empregado.MedicoId.Value > 0 ? (object)empregado.MedicoId.Value : DBNull.Value);
                        command.ExecuteNonQuery();
                        total++;
                    }
                }
            }

            return total;
        }

        public static void DeleteEmpregados(IEnumerable<int> ids)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            {
                foreach (int id in ids)
                {
                    DeleteEsocialEventos(connection, "S-2210", "SELECT id FROM cats WHERE empregado_id = @id", id);
                    DeleteEsocialEventos(connection, "S-2220", "SELECT id FROM asos WHERE empregado_id = @id", id);
                    DeleteEsocialEventos(connection, "S-2240", "SELECT id FROM fatores_risco WHERE empregado_id = @id", id);
                    ExecuteNonQuery(connection, "DELETE FROM cat_testemunhas WHERE cat_id IN (SELECT id FROM cats WHERE empregado_id = @id)", id);
                    ExecuteNonQuery(connection, "DELETE FROM aso_exames WHERE aso_id IN (SELECT id FROM asos WHERE empregado_id = @id)", id);
                    ExecuteNonQuery(connection, "DELETE FROM fatores_risco WHERE empregado_id = @id", id);
                    ExecuteNonQuery(connection, "DELETE FROM tipos_exames WHERE empregado_id = @id", id);
                    ExecuteNonQuery(connection, "DELETE FROM asos WHERE empregado_id = @id", id);
                    ExecuteNonQuery(connection, "DELETE FROM cats WHERE empregado_id = @id", id);
                    ExecuteNonQuery(connection, "DELETE FROM empregados WHERE id = @id", id);
                }
            }
        }

        public static List<CatRecord> GetCats(string termo)
        {
            List<CatRecord> cats = new List<CatRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT c.id, c.empregado_id, e.nome AS empregado_nome, c.data_acidente, c.hora_acidente,
                         c.data_comunicacao, c.local_acidente, c.descricao, c.tipo_cat, c.situacao, c.resultado_aso,
                         c.parte_corpo_atingida, c.lateralidade, c.agente_causador, c.cid10, c.natureza_lesao,
                         c.duracao_tratamento, c.medico_id, c.medico_assistente, c.observacao_medica,
                         c.aposentado, c.area, c.filiacao_prev_social, c.emitente, c.tipo_acidente,
                         c.horas_trabalhadas_antes, c.houve_obito, c.data_obito, c.houve_afastamento,
                         c.registro_policia, c.ultimo_dia_trabalho, c.codificacao_acidente,
                         c.situacao_geradora, c.cat_emitida_por, c.especificacao_local, c.tipo_logradouro,
                         c.numero, c.tipo_inscricao, c.inscricao_estabelecimento, c.logradouro, c.municipio,
                         c.uf, c.bairro, c.complemento, c.cep, c.codigo_postal, c.observacao_cat,
                         c.razao_social_empregador, c.cnae_empregador
                  FROM cats c
                  INNER JOIN empregados e ON e.id = c.empregado_id
                  WHERE c.ativo = 1
                    AND (@termo = ''
                     OR e.nome LIKE @busca
                     OR e.matricula LIKE @busca
                     OR c.tipo_cat LIKE @busca
                     OR c.situacao LIKE @busca)
                  ORDER BY c.data_acidente DESC, c.id DESC", connection))
            {
                command.Parameters.AddWithValue("@termo", termo ?? string.Empty);
                command.Parameters.AddWithValue("@busca", "%" + (termo ?? string.Empty) + "%");

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cats.Add(ReadCat(reader));
                    }
                }
            }

            return cats;
        }

        public static CatStatusResumo GetCatStatusResumo()
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT
                      COUNT(*) AS total,
                      COUNT(DISTINCT CASE WHEN LOWER(TRIM(COALESCE(resultado_aso, ''))) IN ('apto', 'apta') THEN empregado_id END) AS aptos,
                      COUNT(DISTINCT CASE WHEN LOWER(TRIM(COALESCE(resultado_aso, ''))) IN ('inapto', 'inapta') THEN empregado_id END) AS inaptos,
                      COUNT(DISTINCT CASE
                            WHEN resultado_aso IS NULL
                              OR TRIM(resultado_aso) = ''
                              OR LOWER(TRIM(resultado_aso)) LIKE 'aguardando%'
                              OR LOWER(TRIM(COALESCE(situacao, ''))) LIKE 'aguardando%'
                            THEN empregado_id
                          END) AS aguardando
                  FROM cats
                  WHERE ativo = 1", connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                    return new CatStatusResumo();

                return new CatStatusResumo
                {
                    Total = reader.IsDBNull(reader.GetOrdinal("total")) ? 0 : Convert.ToInt32(reader["total"]),
                    Aptos = reader.IsDBNull(reader.GetOrdinal("aptos")) ? 0 : Convert.ToInt32(reader["aptos"]),
                    Inaptos = reader.IsDBNull(reader.GetOrdinal("inaptos")) ? 0 : Convert.ToInt32(reader["inaptos"]),
                    Aguardando = reader.IsDBNull(reader.GetOrdinal("aguardando")) ? 0 : Convert.ToInt32(reader["aguardando"])
                };
            }
        }

        public static List<CatRecord> GetCatsPorEmpregado(int empregadoId)
        {
            List<CatRecord> cats = new List<CatRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT c.id, c.empregado_id, e.nome AS empregado_nome, c.data_acidente, c.hora_acidente,
                         c.data_comunicacao, c.local_acidente, c.descricao, c.tipo_cat, c.situacao, c.resultado_aso,
                         c.parte_corpo_atingida, c.lateralidade, c.agente_causador, c.cid10, c.natureza_lesao,
                         c.duracao_tratamento, c.medico_id, c.medico_assistente, c.observacao_medica,
                         c.aposentado, c.area, c.filiacao_prev_social, c.emitente, c.tipo_acidente,
                         c.horas_trabalhadas_antes, c.houve_obito, c.data_obito, c.houve_afastamento,
                         c.registro_policia, c.ultimo_dia_trabalho, c.codificacao_acidente,
                         c.situacao_geradora, c.cat_emitida_por, c.especificacao_local, c.tipo_logradouro,
                         c.numero, c.tipo_inscricao, c.inscricao_estabelecimento, c.logradouro, c.municipio,
                         c.uf, c.bairro, c.complemento, c.cep, c.codigo_postal, c.observacao_cat,
                         c.razao_social_empregador, c.cnae_empregador
                  FROM cats c
                  INNER JOIN empregados e ON e.id = c.empregado_id
                  WHERE c.ativo = 1 AND c.empregado_id = @empregado_id
                  ORDER BY c.data_acidente DESC, c.id DESC", connection))
            {
                command.Parameters.AddWithValue("@empregado_id", empregadoId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cats.Add(ReadCat(reader));
                    }
                }
            }

            return cats;
        }

        public static CatRecord GetCat(int id)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT c.id, c.empregado_id, e.nome AS empregado_nome, c.data_acidente, c.hora_acidente,
                         c.data_comunicacao, c.local_acidente, c.descricao, c.tipo_cat, c.situacao, c.resultado_aso,
                         c.parte_corpo_atingida, c.lateralidade, c.agente_causador, c.cid10, c.natureza_lesao,
                         c.duracao_tratamento, c.medico_id, c.medico_assistente, c.observacao_medica,
                         c.aposentado, c.area, c.filiacao_prev_social, c.emitente, c.tipo_acidente,
                         c.horas_trabalhadas_antes, c.houve_obito, c.data_obito, c.houve_afastamento,
                         c.registro_policia, c.ultimo_dia_trabalho, c.codificacao_acidente,
                         c.situacao_geradora, c.cat_emitida_por, c.especificacao_local, c.tipo_logradouro,
                         c.numero, c.tipo_inscricao, c.inscricao_estabelecimento, c.logradouro, c.municipio,
                         c.uf, c.bairro, c.complemento, c.cep, c.codigo_postal, c.observacao_cat,
                         c.razao_social_empregador, c.cnae_empregador
                  FROM cats c
                  INNER JOIN empregados e ON e.id = c.empregado_id
                  WHERE c.ativo = 1 AND c.id = @id
                  LIMIT 1", connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? ReadCat(reader) : null;
                }
            }
        }

        public static int SaveCat(CatRecord cat)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                cat.Id > 0
                    ? @"UPDATE cats
                        SET empregado_id = @empregado_id, data_acidente = @data_acidente, hora_acidente = @hora_acidente,
                            data_comunicacao = @data_comunicacao, local_acidente = @local_acidente,
                            descricao = @descricao, tipo_cat = @tipo_cat, situacao = @situacao, resultado_aso = @resultado_aso,
                            parte_corpo_atingida = @parte_corpo_atingida, lateralidade = @lateralidade,
                            agente_causador = @agente_causador, cid10 = @cid10, natureza_lesao = @natureza_lesao,
                            duracao_tratamento = @duracao_tratamento, medico_id = @medico_id, medico_assistente = @medico_assistente,
                            observacao_medica = @observacao_medica, aposentado = @aposentado, area = @area,
                            filiacao_prev_social = @filiacao_prev_social, emitente = @emitente, tipo_acidente = @tipo_acidente,
                            horas_trabalhadas_antes = @horas_trabalhadas_antes, houve_obito = @houve_obito,
                            data_obito = @data_obito, houve_afastamento = @houve_afastamento, registro_policia = @registro_policia,
                            ultimo_dia_trabalho = @ultimo_dia_trabalho, codificacao_acidente = @codificacao_acidente,
                            situacao_geradora = @situacao_geradora, cat_emitida_por = @cat_emitida_por,
                            especificacao_local = @especificacao_local, tipo_logradouro = @tipo_logradouro,
                            numero = @numero, tipo_inscricao = @tipo_inscricao,
                            inscricao_estabelecimento = @inscricao_estabelecimento, logradouro = @logradouro,
                            municipio = @municipio, uf = @uf, bairro = @bairro, complemento = @complemento,
                            cep = @cep, codigo_postal = @codigo_postal, observacao_cat = @observacao_cat,
                            razao_social_empregador = @razao_social_empregador, cnae_empregador = @cnae_empregador
                        WHERE id = @id"
                    : @"INSERT INTO cats (empregado_id, data_acidente, hora_acidente, data_comunicacao, local_acidente, descricao, tipo_cat, situacao, resultado_aso,
                            parte_corpo_atingida, lateralidade, agente_causador, cid10, natureza_lesao, duracao_tratamento, medico_id, medico_assistente, observacao_medica,
                            aposentado, area, filiacao_prev_social, emitente, tipo_acidente, horas_trabalhadas_antes,
                            houve_obito, data_obito, houve_afastamento, registro_policia, ultimo_dia_trabalho,
                            codificacao_acidente, situacao_geradora, cat_emitida_por, especificacao_local,
                            tipo_logradouro, numero, tipo_inscricao, inscricao_estabelecimento, logradouro, municipio,
                            uf, bairro, complemento, cep, codigo_postal, observacao_cat, razao_social_empregador, cnae_empregador)
                        VALUES (@empregado_id, @data_acidente, @hora_acidente, @data_comunicacao, @local_acidente, @descricao, @tipo_cat, @situacao, @resultado_aso,
                            @parte_corpo_atingida, @lateralidade, @agente_causador, @cid10, @natureza_lesao, @duracao_tratamento, @medico_id, @medico_assistente, @observacao_medica,
                            @aposentado, @area, @filiacao_prev_social, @emitente, @tipo_acidente, @horas_trabalhadas_antes,
                            @houve_obito, @data_obito, @houve_afastamento, @registro_policia, @ultimo_dia_trabalho,
                            @codificacao_acidente, @situacao_geradora, @cat_emitida_por, @especificacao_local,
                            @tipo_logradouro, @numero, @tipo_inscricao, @inscricao_estabelecimento, @logradouro, @municipio,
                            @uf, @bairro, @complemento, @cep, @codigo_postal, @observacao_cat, @razao_social_empregador, @cnae_empregador)", connection))
            {
                command.Parameters.AddWithValue("@id", cat.Id);
                command.Parameters.AddWithValue("@empregado_id", cat.EmpregadoId);
                command.Parameters.AddWithValue("@data_acidente", DateToDbNull(cat.DataAcidente));
                command.Parameters.AddWithValue("@hora_acidente", TimeToDbNull(cat.HoraAcidente));
                command.Parameters.AddWithValue("@data_comunicacao", DateToDbNull(cat.DataComunicacao));
                command.Parameters.AddWithValue("@local_acidente", EmptyToDbNull(cat.LocalAcidente));
                command.Parameters.AddWithValue("@descricao", EmptyToDbNull(cat.Descricao));
                command.Parameters.AddWithValue("@tipo_cat", EmptyToDbNull(cat.TipoCat));
                command.Parameters.AddWithValue("@situacao", string.IsNullOrWhiteSpace(cat.Situacao) ? "Aberta" : cat.Situacao);
                command.Parameters.AddWithValue("@resultado_aso", NormalizarResultadoAso(string.IsNullOrWhiteSpace(cat.ResultadoAso) ? "Aguardando ASO de Retorno" : cat.ResultadoAso));
                command.Parameters.AddWithValue("@parte_corpo_atingida", EmptyToDbNull(cat.ParteCorpoAtingida));
                command.Parameters.AddWithValue("@lateralidade", EmptyToDbNull(cat.Lateralidade));
                command.Parameters.AddWithValue("@agente_causador", EmptyToDbNull(cat.AgenteCausador));
                command.Parameters.AddWithValue("@cid10", EmptyToDbNull(cat.Cid10));
                command.Parameters.AddWithValue("@natureza_lesao", EmptyToDbNull(cat.NaturezaLesao));
                command.Parameters.AddWithValue("@duracao_tratamento", EmptyToDbNull(cat.DuracaoTratamento));
                command.Parameters.AddWithValue("@medico_id", cat.MedicoId.HasValue && cat.MedicoId.Value > 0 ? (object)cat.MedicoId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@medico_assistente", EmptyToDbNull(cat.MedicoAssistente));
                command.Parameters.AddWithValue("@observacao_medica", EmptyToDbNull(cat.ObservacaoMedica));
                command.Parameters.AddWithValue("@aposentado", cat.Aposentado ? 1 : 0);
                command.Parameters.AddWithValue("@area", EmptyToDbNull(cat.Area));
                command.Parameters.AddWithValue("@filiacao_prev_social", EmptyToDbNull(cat.FiliacaoPrevSocial));
                command.Parameters.AddWithValue("@emitente", EmptyToDbNull(cat.Emitente));
                command.Parameters.AddWithValue("@tipo_acidente", EmptyToDbNull(cat.TipoAcidente));
                command.Parameters.AddWithValue("@horas_trabalhadas_antes", EmptyToDbNull(cat.HorasTrabalhadasAntes));
                command.Parameters.AddWithValue("@houve_obito", cat.HouveObito ? 1 : 0);
                command.Parameters.AddWithValue("@data_obito", DateToDbNull(cat.DataObito));
                command.Parameters.AddWithValue("@houve_afastamento", cat.HouveAfastamento ? 1 : 0);
                command.Parameters.AddWithValue("@registro_policia", cat.RegistroPolicia ? 1 : 0);
                command.Parameters.AddWithValue("@ultimo_dia_trabalho", DateToDbNull(cat.UltimoDiaTrabalho));
                command.Parameters.AddWithValue("@codificacao_acidente", EmptyToDbNull(cat.CodificacaoAcidente));
                command.Parameters.AddWithValue("@situacao_geradora", EmptyToDbNull(cat.SituacaoGeradora));
                command.Parameters.AddWithValue("@cat_emitida_por", EmptyToDbNull(cat.CatEmitidaPor));
                command.Parameters.AddWithValue("@especificacao_local", EmptyToDbNull(cat.EspecificacaoLocal));
                command.Parameters.AddWithValue("@tipo_logradouro", EmptyToDbNull(cat.TipoLogradouro));
                command.Parameters.AddWithValue("@numero", EmptyToDbNull(cat.Numero));
                command.Parameters.AddWithValue("@tipo_inscricao", EmptyToDbNull(cat.TipoInscricao));
                command.Parameters.AddWithValue("@inscricao_estabelecimento", EmptyToDbNull(cat.InscricaoEstabelecimento));
                command.Parameters.AddWithValue("@logradouro", EmptyToDbNull(cat.Logradouro));
                command.Parameters.AddWithValue("@municipio", EmptyToDbNull(cat.Municipio));
                command.Parameters.AddWithValue("@uf", EmptyToDbNull(cat.Uf));
                command.Parameters.AddWithValue("@bairro", EmptyToDbNull(cat.Bairro));
                command.Parameters.AddWithValue("@complemento", EmptyToDbNull(cat.Complemento));
                command.Parameters.AddWithValue("@cep", EmptyToDbNull(cat.Cep));
                command.Parameters.AddWithValue("@codigo_postal", EmptyToDbNull(cat.CodigoPostal));
                command.Parameters.AddWithValue("@observacao_cat", EmptyToDbNull(cat.ObservacaoCat));
                command.Parameters.AddWithValue("@razao_social_empregador", EmptyToDbNull(cat.RazaoSocialEmpregador));
                command.Parameters.AddWithValue("@cnae_empregador", EmptyToDbNull(cat.CnaeEmpregador));
                command.ExecuteNonQuery();
                int catId = cat.Id > 0 ? cat.Id : (int)command.LastInsertedId;
                if (cat.Id <= 0)
                    AtualizarStatusEmpregadoPorCat(connection, cat.EmpregadoId, cat.HouveAfastamento);
                return catId;
            }
        }

        public static void SaveCatTestemunhas(int catId, IEnumerable<CatTestemunhaRecord> testemunhas)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            {
                using (MySqlCommand command = new MySqlCommand("DELETE FROM cat_testemunhas WHERE cat_id = @cat_id", connection))
                {
                    command.Parameters.AddWithValue("@cat_id", catId);
                    command.ExecuteNonQuery();
                }

                foreach (CatTestemunhaRecord testemunha in testemunhas)
                {
                    if (testemunha == null || string.IsNullOrWhiteSpace(testemunha.Nome))
                        continue;

                    using (MySqlCommand command = new MySqlCommand(
                        @"INSERT INTO cat_testemunhas (cat_id, nome, cpf, telefone, endereco)
                          VALUES (@cat_id, @nome, @cpf, @telefone, @endereco)", connection))
                    {
                        command.Parameters.AddWithValue("@cat_id", catId);
                        command.Parameters.AddWithValue("@nome", testemunha.Nome.Trim());
                        command.Parameters.AddWithValue("@cpf", EmptyToDbNull(testemunha.Cpf));
                        command.Parameters.AddWithValue("@telefone", EmptyToDbNull(testemunha.Telefone));
                        command.Parameters.AddWithValue("@endereco", EmptyToDbNull(testemunha.Endereco));
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void DeleteCats(IEnumerable<int> ids)
        {
            DeleteByIdsWithChildren("cats", ids, delegate(MySqlConnection connection, int id)
            {
                DeleteEsocialEventos(connection, "S-2210", id);
                ExecuteNonQuery(connection, "UPDATE asos SET cat_id = NULL WHERE cat_id = @id", id);
                ExecuteNonQuery(connection, "DELETE FROM cat_testemunhas WHERE cat_id = @id", id);
            });
        }

        public static List<CatTestemunhaRecord> GetCatTestemunhas(int catId)
        {
            List<CatTestemunhaRecord> testemunhas = new List<CatTestemunhaRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT id, cat_id, nome, cpf, telefone, endereco
                  FROM cat_testemunhas
                  WHERE cat_id = @cat_id
                  ORDER BY id
                  LIMIT 2", connection))
            {
                command.Parameters.AddWithValue("@cat_id", catId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        testemunhas.Add(new CatTestemunhaRecord
                        {
                            Id = reader.GetInt32("id"),
                            CatId = reader.GetInt32("cat_id"),
                            Nome = ReaderString(reader, "nome"),
                            Cpf = ReaderString(reader, "cpf"),
                            Telefone = ReaderString(reader, "telefone"),
                            Endereco = ReaderString(reader, "endereco")
                        });
                    }
                }
            }

            return testemunhas;
        }

        public static int RegistrarTransmissoesCats()
        {
            int total = 0;

            List<CatRecord> cats = GetCats(string.Empty);
            foreach (CatRecord cat in cats)
            {
                if (RegistrarTransmissaoEvento(
                    "S-2210",
                    cat.Id,
                    "CAT " + cat.Id + " - " + cat.EmpregadoNome,
                    "{\"catId\":" + cat.Id + ",\"empregado\":\"" + JsonEscape(cat.EmpregadoNome) + "\",\"resultadoAso\":\"" + JsonEscape(cat.ResultadoAso) + "\"}"
                ))
                    total++;
            }

            return total;
        }

        public static int RegistrarTransmissoesEsocial()
        {
            int total = RegistrarTransmissoesCats();

            foreach (AsoRecord aso in GetAsos(string.Empty))
            {
                if (RegistrarTransmissaoEvento(
                    "S-2220",
                    aso.Id,
                    "ASO " + aso.Id + " - " + aso.EmpregadoNome,
                    "{\"asoId\":" + aso.Id + ",\"empregado\":\"" + JsonEscape(aso.EmpregadoNome) + "\",\"medico\":\"" + JsonEscape(aso.MedicoNome) + "\",\"resultado\":\"" + JsonEscape(aso.Resultado) + "\"}"
                ))
                    total++;
            }

            foreach (RiskFactorRecord risco in GetFatoresRisco(string.Empty))
            {
                string trabalhador = string.IsNullOrWhiteSpace(risco.EmpregadoNome) ? risco.AmbienteNome : risco.EmpregadoNome;
                if (RegistrarTransmissaoEvento(
                    "S-2240",
                    risco.Id,
                    "Fator de risco " + risco.Id + " - " + trabalhador,
                    "{\"fatorRiscoId\":" + risco.Id + ",\"trabalhador\":\"" + JsonEscape(trabalhador) + "\",\"agente\":\"" + JsonEscape(risco.Agente) + "\"}"
                ))
                    total++;
            }

            return total;
        }

        private static bool RegistrarTransmissaoEvento(string codigoEvento, int origemId, string descricao, string payload)
        {
            string protocolo = "P" + DateTime.Now.ToString("yyyyMMddHHmmss") + origemId.ToString("0000");
            string recibo = "R-" + codigoEvento.Replace("-", string.Empty) + "-" + origemId.ToString("000000");

            using (MySqlConnection connection = Database.OpenConnection())
            {
                int eventoId;

                using (MySqlCommand existsCommand = new MySqlCommand(
                    @"SELECT COUNT(*)
                      FROM esocial_eventos
                      WHERE codigo_evento = @codigo_evento
                        AND origem_id = @origem_id
                        AND status = 'Transmitido'", connection))
                {
                    existsCommand.Parameters.AddWithValue("@codigo_evento", codigoEvento);
                    existsCommand.Parameters.AddWithValue("@origem_id", origemId);
                    if (Convert.ToInt32(existsCommand.ExecuteScalar()) > 0)
                        return false;
                }

                using (MySqlCommand command = new MySqlCommand(
                    @"INSERT INTO esocial_eventos (codigo_evento, descricao, status, payload, protocolo, recibo, origem_tipo, origem_id, enviado_em)
                      VALUES (@codigo_evento, @descricao, 'Transmitido', @payload, @protocolo, @recibo, @origem_tipo, @origem_id, NOW())", connection))
                {
                    command.Parameters.AddWithValue("@codigo_evento", codigoEvento);
                    command.Parameters.AddWithValue("@descricao", descricao);
                    command.Parameters.AddWithValue("@payload", payload);
                    command.Parameters.AddWithValue("@protocolo", protocolo);
                    command.Parameters.AddWithValue("@recibo", recibo);
                    command.Parameters.AddWithValue("@origem_tipo", codigoEvento);
                    command.Parameters.AddWithValue("@origem_id", origemId);
                    command.ExecuteNonQuery();
                    eventoId = (int)command.LastInsertedId;
                }

                using (MySqlCommand command = new MySqlCommand(
                    @"INSERT INTO esocial_logs (evento_id, mensagem, status, detalhes)
                      VALUES (@evento_id, @mensagem, 'Transmitido', @detalhes)", connection))
                {
                    command.Parameters.AddWithValue("@evento_id", eventoId);
                    command.Parameters.AddWithValue("@mensagem", "Evento " + codigoEvento + " registrado para transmissao interna.");
                    command.Parameters.AddWithValue("@detalhes", descricao + ".");
                    command.ExecuteNonQuery();
                }
            }

            return true;
        }

        public static List<EsocialTransmissaoRecord> GetEsocialTransmissoes()
        {
            return GetEsocialTransmissoes(string.Empty);
        }

        public static List<EsocialTransmissaoRecord> GetEsocialTransmissoes(string termo)
        {
            List<EsocialTransmissaoRecord> logs = new List<EsocialTransmissaoRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT ev.id, COALESCE(ev.enviado_em, ev.criado_em) AS data_hora, ev.codigo_evento,
                         ev.descricao, ev.protocolo, ev.status, ev.recibo
                  FROM esocial_eventos ev
                  WHERE @termo = ''
                     OR DATE_FORMAT(COALESCE(ev.enviado_em, ev.criado_em), '%d/%m/%Y') LIKE @busca
                     OR DATE_FORMAT(COALESCE(ev.enviado_em, ev.criado_em), '%Y-%m-%d') LIKE @busca
                     OR ev.protocolo LIKE @busca
                     OR ev.recibo LIKE @busca
                     OR ev.codigo_evento LIKE @busca
                     OR ev.descricao LIKE @busca
                     OR ev.status LIKE @busca
                  ORDER BY COALESCE(ev.enviado_em, ev.criado_em) DESC, ev.id DESC
                  LIMIT 200", connection))
            {
                command.Parameters.AddWithValue("@termo", termo ?? string.Empty);
                command.Parameters.AddWithValue("@busca", "%" + (termo ?? string.Empty) + "%");

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logs.Add(new EsocialTransmissaoRecord
                        {
                            Id = reader.GetInt32("id"),
                            DataHora = reader.GetDateTime("data_hora").ToString("dd/MM/yyyy HH:mm"),
                            Evento = ReaderString(reader, "codigo_evento"),
                            Trabalhador = ExtrairTrabalhadorEsocial(ReaderString(reader, "descricao")),
                            Protocolo = ReaderString(reader, "protocolo"),
                            Status = ReaderString(reader, "status"),
                            Recibo = ReaderString(reader, "recibo")
                        });
                    }
                }
            }

            return logs;
        }

        public static int SaveAso(AsoRecord aso)
        {
            int asoId;

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"INSERT INTO asos (empregado_id, medico_id, cat_id, data_aso, tipo_exame, resultado, observacoes)
                  VALUES (@empregado_id, @medico_id, @cat_id, @data_aso, @tipo_exame, @resultado, @observacoes)", connection))
            {
                command.Parameters.AddWithValue("@empregado_id", aso.EmpregadoId);
                command.Parameters.AddWithValue("@medico_id", aso.MedicoId);
                command.Parameters.AddWithValue("@cat_id", aso.CatId.HasValue && aso.CatId.Value > 0 ? (object)aso.CatId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@data_aso", DateToDbNull(aso.DataAso));
                command.Parameters.AddWithValue("@tipo_exame", aso.TipoExame);
                command.Parameters.AddWithValue("@resultado", NormalizarResultadoAso(aso.Resultado));
                command.Parameters.AddWithValue("@observacoes", EmptyToDbNull(aso.Observacoes));
                command.ExecuteNonQuery();
                asoId = (int)command.LastInsertedId;
            }

            if (aso.CatId.HasValue && aso.CatId.Value > 0 && IsAsoRetornoTrabalho(aso.TipoExame))
            {
                using (MySqlConnection connection = Database.OpenConnection())
                using (MySqlCommand command = new MySqlCommand(
                    @"UPDATE cats
                      SET resultado_aso = @resultado_aso,
                          situacao = @situacao
                      WHERE id = @cat_id", connection))
                {
                    string resultado = NormalizarResultadoAso(aso.Resultado);
                    command.Parameters.AddWithValue("@cat_id", aso.CatId.Value);
                    command.Parameters.AddWithValue("@resultado_aso", resultado);
                    command.Parameters.AddWithValue("@situacao", resultado == "Apto" ? "Encerrada - retorno apto" : "Aguardando novo ASO de retorno");
                    command.ExecuteNonQuery();
                }
            }

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"UPDATE empregados
                  SET status_aso = @status_aso,
                      medico_id = @medico_id
                  WHERE id = @empregado_id", connection))
            {
                command.Parameters.AddWithValue("@empregado_id", aso.EmpregadoId);
                command.Parameters.AddWithValue("@medico_id", aso.MedicoId);
                command.Parameters.AddWithValue("@status_aso", NormalizarResultadoAso(aso.Resultado));
                command.ExecuteNonQuery();
            }

            return asoId;
        }

        public static void SaveAsoExames(int asoId, IEnumerable<AsoExameRecord> exames)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            {
                foreach (AsoExameRecord exame in exames)
                {
                    using (MySqlCommand command = new MySqlCommand(
                        @"INSERT INTO aso_exames (aso_id, tipo_exame_id, data_exame, resultado, observacoes)
                          VALUES (@aso_id, @tipo_exame_id, @data_exame, @resultado, @observacoes)", connection))
                    {
                        command.Parameters.AddWithValue("@aso_id", asoId);
                        command.Parameters.AddWithValue("@tipo_exame_id", exame.TipoExameId);
                        command.Parameters.AddWithValue("@data_exame", DateToDbNull(exame.DataExame));
                        command.Parameters.AddWithValue("@resultado", EmptyToDbNull(exame.Resultado));
                        command.Parameters.AddWithValue("@observacoes", EmptyToDbNull(exame.Observacoes));
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static List<AsoRecord> GetAsos(string termo)
        {
            List<AsoRecord> asos = new List<AsoRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT a.id, a.empregado_id, e.nome AS empregado_nome, a.medico_id, m.nome AS medico_nome,
                         a.cat_id, a.data_aso, a.tipo_exame, a.resultado, a.observacoes
                  FROM asos a
                  INNER JOIN empregados e ON e.id = a.empregado_id
                  INNER JOIN medicos m ON m.id = a.medico_id
                  WHERE a.ativo = 1
                    AND (@termo = ''
                     OR e.nome LIKE @busca
                     OR m.nome LIKE @busca
                     OR a.tipo_exame LIKE @busca
                     OR a.resultado LIKE @busca)
                  ORDER BY a.data_aso DESC, a.id DESC", connection))
            {
                command.Parameters.AddWithValue("@termo", termo ?? string.Empty);
                command.Parameters.AddWithValue("@busca", "%" + (termo ?? string.Empty) + "%");

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        asos.Add(ReadAso(reader));
                    }
                }
            }

            return asos;
        }

        public static AsoRecord GetAso(int id)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT a.id, a.empregado_id, e.nome AS empregado_nome, a.medico_id, m.nome AS medico_nome,
                         a.cat_id, a.data_aso, a.tipo_exame, a.resultado, a.observacoes
                  FROM asos a
                  INNER JOIN empregados e ON e.id = a.empregado_id
                  INNER JOIN medicos m ON m.id = a.medico_id
                  WHERE a.ativo = 1 AND a.id = @id
                  LIMIT 1", connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? ReadAso(reader) : null;
                }
            }
        }

        public static List<AsoExameRecord> GetAsoExames(int asoId)
        {
            List<AsoExameRecord> exames = new List<AsoExameRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT ae.id, ae.aso_id, ae.tipo_exame_id, te.nome AS tipo_exame_nome,
                         ae.data_exame, ae.resultado, ae.observacoes
                  FROM aso_exames ae
                  INNER JOIN tipos_exames te ON te.id = ae.tipo_exame_id
                  WHERE ae.aso_id = @aso_id
                  ORDER BY ae.data_exame DESC, ae.id DESC", connection))
            {
                command.Parameters.AddWithValue("@aso_id", asoId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        exames.Add(new AsoExameRecord
                        {
                            Id = reader.GetInt32("id"),
                            AsoId = reader.GetInt32("aso_id"),
                            TipoExameId = reader.GetInt32("tipo_exame_id"),
                            TipoExameNome = ReaderString(reader, "tipo_exame_nome"),
                            DataExame = ReaderDate(reader, "data_exame"),
                            Resultado = ReaderString(reader, "resultado"),
                            Observacoes = ReaderString(reader, "observacoes")
                        });
                    }
                }
            }

            return exames;
        }

        public static List<AsoRecord> GetAsosPorEmpregado(int empregadoId)
        {
            List<AsoRecord> asos = new List<AsoRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT a.id, a.empregado_id, e.nome AS empregado_nome, a.medico_id, m.nome AS medico_nome,
                         a.cat_id, a.data_aso, a.tipo_exame, a.resultado, a.observacoes
                  FROM asos a
                  INNER JOIN empregados e ON e.id = a.empregado_id
                  INNER JOIN medicos m ON m.id = a.medico_id
                  WHERE a.ativo = 1 AND a.empregado_id = @empregado_id
                  ORDER BY a.data_aso DESC, a.id DESC", connection))
            {
                command.Parameters.AddWithValue("@empregado_id", empregadoId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        asos.Add(ReadAso(reader));
                    }
                }
            }

            return asos;
        }

        public static void DeleteAsos(IEnumerable<int> ids)
        {
            DeleteByIdsWithChildren("asos", ids, delegate(MySqlConnection connection, int id)
            {
                DeleteEsocialEventos(connection, "S-2220", id);
                ExecuteNonQuery(connection, "DELETE FROM aso_exames WHERE aso_id = @id", id);
            });
        }

        public static List<RiskFactorRecord> GetFatoresRisco(string termo)
        {
            List<RiskFactorRecord> riscos = new List<RiskFactorRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT f.id, f.empregado_id, e.nome AS empregado_nome, f.ambiente_id, a.ambiente AS ambiente_nome,
                         f.tipo_fator, f.agente, f.intensidade, f.tecnica_medicao, f.data_avaliacao,
                         f.inicio_exposicao, f.fim_exposicao, f.descricao_atividades, f.usa_epi, f.epi_eficaz,
                         f.epi_descricao
                  FROM fatores_risco f
                  LEFT JOIN empregados e ON e.id = f.empregado_id
                  LEFT JOIN ambientes_trabalho a ON a.id = f.ambiente_id
                  WHERE f.ativo = 1
                    AND (@termo = ''
                     OR e.nome LIKE @busca
                     OR a.ambiente LIKE @busca
                     OR f.tipo_fator LIKE @busca
                     OR f.agente LIKE @busca)
                  ORDER BY f.data_avaliacao DESC, f.id DESC", connection))
            {
                command.Parameters.AddWithValue("@termo", termo ?? string.Empty);
                command.Parameters.AddWithValue("@busca", "%" + (termo ?? string.Empty) + "%");

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        riscos.Add(new RiskFactorRecord
                        {
                            Id = reader.GetInt32("id"),
                            EmpregadoId = reader.IsDBNull(reader.GetOrdinal("empregado_id")) ? (int?)null : reader.GetInt32("empregado_id"),
                            EmpregadoNome = reader.IsDBNull(reader.GetOrdinal("empregado_nome")) ? string.Empty : reader.GetString("empregado_nome"),
                            AmbienteId = reader.IsDBNull(reader.GetOrdinal("ambiente_id")) ? (int?)null : reader.GetInt32("ambiente_id"),
                            AmbienteNome = reader.IsDBNull(reader.GetOrdinal("ambiente_nome")) ? string.Empty : reader.GetString("ambiente_nome"),
                            TipoFator = reader.GetString("tipo_fator"),
                            Agente = reader.GetString("agente"),
                            Intensidade = reader.IsDBNull(reader.GetOrdinal("intensidade")) ? string.Empty : reader.GetString("intensidade"),
                            TecnicaMedicao = reader.IsDBNull(reader.GetOrdinal("tecnica_medicao")) ? string.Empty : reader.GetString("tecnica_medicao"),
                            DataAvaliacao = reader.IsDBNull(reader.GetOrdinal("data_avaliacao")) ? string.Empty : reader.GetDateTime("data_avaliacao").ToString("dd/MM/yyyy"),
                            InicioExposicao = reader.IsDBNull(reader.GetOrdinal("inicio_exposicao")) ? string.Empty : reader.GetDateTime("inicio_exposicao").ToString("dd/MM/yyyy"),
                            FimExposicao = reader.IsDBNull(reader.GetOrdinal("fim_exposicao")) ? string.Empty : reader.GetDateTime("fim_exposicao").ToString("dd/MM/yyyy"),
                            DescricaoAtividades = reader.IsDBNull(reader.GetOrdinal("descricao_atividades")) ? string.Empty : reader.GetString("descricao_atividades"),
                            UsaEpi = !reader.IsDBNull(reader.GetOrdinal("usa_epi")) && reader.GetBoolean("usa_epi"),
                            EpiEficaz = !reader.IsDBNull(reader.GetOrdinal("epi_eficaz")) && reader.GetBoolean("epi_eficaz"),
                            EpisSelecionados = reader.IsDBNull(reader.GetOrdinal("epi_descricao")) ? string.Empty : reader.GetString("epi_descricao")
                        });
                    }
                }
            }

            return riscos;
        }

        public static void DeleteFatoresRisco(IEnumerable<int> ids)
        {
            DeleteByIdsWithChildren("fatores_risco", ids, delegate(MySqlConnection connection, int id)
            {
                DeleteEsocialEventos(connection, "S-2240", id);
            });
        }

        public static RiskFactorRecord GetFatorRisco(int id)
        {
            List<RiskFactorRecord> riscos = GetFatoresRisco(string.Empty);
            foreach (RiskFactorRecord risco in riscos)
            {
                if (risco.Id == id)
                    return risco;
            }

            return null;
        }

        public static void SaveFatorRisco(RiskFactorRecord risco)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                risco.Id > 0
                    ? @"UPDATE fatores_risco
                        SET empregado_id = @empregado_id, ambiente_id = @ambiente_id, tipo_fator = @tipo_fator,
                            agente = @agente, intensidade = @intensidade, tecnica_medicao = @tecnica_medicao,
                            data_avaliacao = @data_avaliacao, inicio_exposicao = @inicio_exposicao,
                            fim_exposicao = @fim_exposicao, descricao_atividades = @descricao_atividades,
                            usa_epi = @usa_epi, epi_eficaz = @epi_eficaz, epi_descricao = @epi_descricao
                        WHERE id = @id"
                    : @"INSERT INTO fatores_risco
                        (empregado_id, ambiente_id, tipo_fator, agente, intensidade, tecnica_medicao,
                         data_avaliacao, inicio_exposicao, fim_exposicao, descricao_atividades, usa_epi, epi_eficaz, epi_descricao)
                        VALUES
                        (@empregado_id, @ambiente_id, @tipo_fator, @agente, @intensidade, @tecnica_medicao,
                         @data_avaliacao, @inicio_exposicao, @fim_exposicao, @descricao_atividades, @usa_epi, @epi_eficaz, @epi_descricao)", connection))
            {
                command.Parameters.AddWithValue("@id", risco.Id);
                command.Parameters.AddWithValue("@empregado_id", risco.EmpregadoId.HasValue && risco.EmpregadoId.Value > 0 ? (object)risco.EmpregadoId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@ambiente_id", risco.AmbienteId.HasValue && risco.AmbienteId.Value > 0 ? (object)risco.AmbienteId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@tipo_fator", risco.TipoFator);
                command.Parameters.AddWithValue("@agente", risco.Agente);
                command.Parameters.AddWithValue("@intensidade", EmptyToDbNull(risco.Intensidade));
                command.Parameters.AddWithValue("@tecnica_medicao", EmptyToDbNull(risco.TecnicaMedicao));
                command.Parameters.AddWithValue("@data_avaliacao", DateToDbNull(risco.DataAvaliacao));
                command.Parameters.AddWithValue("@inicio_exposicao", DateToDbNull(risco.InicioExposicao));
                command.Parameters.AddWithValue("@fim_exposicao", DateToDbNull(risco.FimExposicao));
                command.Parameters.AddWithValue("@descricao_atividades", EmptyToDbNull(risco.DescricaoAtividades));
                command.Parameters.AddWithValue("@usa_epi", risco.UsaEpi);
                command.Parameters.AddWithValue("@epi_eficaz", risco.EpiEficaz);
                command.Parameters.AddWithValue("@epi_descricao", risco.UsaEpi ? EmptyToDbNull(risco.EpisSelecionados) : DBNull.Value);
                command.ExecuteNonQuery();
            }
        }

        private static void SoftDeleteByIds(string tableName, IEnumerable<int> ids)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            {
                foreach (int id in ids)
                {
                    using (MySqlCommand command = new MySqlCommand("UPDATE " + tableName + " SET ativo = 0 WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void DeleteByIdsWithChildren(string tableName, IEnumerable<int> ids, Action<MySqlConnection, int> beforeDelete)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            {
                foreach (int id in ids)
                {
                    if (beforeDelete != null)
                        beforeDelete(connection, id);

                    ExecuteNonQuery(connection, "DELETE FROM " + tableName + " WHERE id = @id", id);
                }
            }
        }

        private static void ExecuteNonQuery(MySqlConnection connection, string sql, int id)
        {
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        private static void DeleteEsocialEventos(MySqlConnection connection, string codigoEvento, int origemId)
        {
            using (MySqlCommand command = new MySqlCommand(
                @"DELETE l
                  FROM esocial_logs l
                  INNER JOIN esocial_eventos e ON e.id = l.evento_id
                  WHERE e.codigo_evento = @codigo_evento
                    AND e.origem_id = @origem_id", connection))
            {
                command.Parameters.AddWithValue("@codigo_evento", codigoEvento);
                command.Parameters.AddWithValue("@origem_id", origemId);
                command.ExecuteNonQuery();
            }

            using (MySqlCommand command = new MySqlCommand(
                @"DELETE FROM esocial_eventos
                  WHERE codigo_evento = @codigo_evento
                    AND origem_id = @origem_id", connection))
            {
                command.Parameters.AddWithValue("@codigo_evento", codigoEvento);
                command.Parameters.AddWithValue("@origem_id", origemId);
                command.ExecuteNonQuery();
            }
        }

        private static void DeleteEsocialEventos(MySqlConnection connection, string codigoEvento, string selectOrigemIdsSql, int ownerId)
        {
            List<int> origemIds = new List<int>();

            using (MySqlCommand command = new MySqlCommand(selectOrigemIdsSql, connection))
            {
                command.Parameters.AddWithValue("@id", ownerId);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        origemIds.Add(reader.GetInt32(0));
                    }
                }
            }

            foreach (int origemId in origemIds)
            {
                DeleteEsocialEventos(connection, codigoEvento, origemId);
            }
        }

        private static AsoRecord ReadAso(MySqlDataReader reader)
        {
            return new AsoRecord
            {
                Id = reader.GetInt32("id"),
                EmpregadoId = reader.GetInt32("empregado_id"),
                EmpregadoNome = reader.GetString("empregado_nome"),
                MedicoId = reader.GetInt32("medico_id"),
                MedicoNome = reader.GetString("medico_nome"),
                CatId = reader.IsDBNull(reader.GetOrdinal("cat_id")) ? (int?)null : reader.GetInt32("cat_id"),
                DataAso = reader.GetDateTime("data_aso").ToString("dd/MM/yyyy"),
                TipoExame = reader.GetString("tipo_exame"),
                Resultado = reader.GetString("resultado"),
                Observacoes = reader.IsDBNull(reader.GetOrdinal("observacoes")) ? string.Empty : reader.GetString("observacoes")
            };
        }

        private static CatRecord ReadCat(MySqlDataReader reader)
        {
            return new CatRecord
            {
                Id = reader.GetInt32("id"),
                EmpregadoId = reader.GetInt32("empregado_id"),
                EmpregadoNome = reader.GetString("empregado_nome"),
                DataAcidente = reader.GetDateTime("data_acidente").ToString("dd/MM/yyyy"),
                HoraAcidente = reader.IsDBNull(reader.GetOrdinal("hora_acidente")) ? string.Empty : reader.GetTimeSpan("hora_acidente").ToString(@"hh\:mm"),
                DataComunicacao = reader.IsDBNull(reader.GetOrdinal("data_comunicacao")) ? string.Empty : reader.GetDateTime("data_comunicacao").ToString("dd/MM/yyyy"),
                DataObito = ReaderDate(reader, "data_obito"),
                LocalAcidente = reader.IsDBNull(reader.GetOrdinal("local_acidente")) ? string.Empty : reader.GetString("local_acidente"),
                Descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? string.Empty : reader.GetString("descricao"),
                TipoCat = reader.IsDBNull(reader.GetOrdinal("tipo_cat")) ? string.Empty : reader.GetString("tipo_cat"),
                Situacao = reader.GetString("situacao"),
                ResultadoAso = reader.IsDBNull(reader.GetOrdinal("resultado_aso")) ? "Aguardando ASO de Retorno" : reader.GetString("resultado_aso"),
                ParteCorpoAtingida = ReaderString(reader, "parte_corpo_atingida"),
                Lateralidade = ReaderString(reader, "lateralidade"),
                AgenteCausador = ReaderString(reader, "agente_causador"),
                Cid10 = ReaderString(reader, "cid10"),
                NaturezaLesao = ReaderString(reader, "natureza_lesao"),
                DuracaoTratamento = ReaderString(reader, "duracao_tratamento"),
                MedicoId = reader.IsDBNull(reader.GetOrdinal("medico_id")) ? (int?)null : reader.GetInt32("medico_id"),
                MedicoAssistente = ReaderString(reader, "medico_assistente"),
                ObservacaoMedica = ReaderString(reader, "observacao_medica"),
                Aposentado = ReaderBool(reader, "aposentado"),
                Area = ReaderString(reader, "area"),
                FiliacaoPrevSocial = ReaderString(reader, "filiacao_prev_social"),
                Emitente = ReaderString(reader, "emitente"),
                TipoAcidente = ReaderString(reader, "tipo_acidente"),
                HorasTrabalhadasAntes = ReaderString(reader, "horas_trabalhadas_antes"),
                HouveObito = ReaderBool(reader, "houve_obito"),
                HouveAfastamento = ReaderBool(reader, "houve_afastamento"),
                RegistroPolicia = ReaderBool(reader, "registro_policia"),
                UltimoDiaTrabalho = ReaderDate(reader, "ultimo_dia_trabalho"),
                CodificacaoAcidente = ReaderString(reader, "codificacao_acidente"),
                SituacaoGeradora = ReaderString(reader, "situacao_geradora"),
                CatEmitidaPor = ReaderString(reader, "cat_emitida_por"),
                EspecificacaoLocal = ReaderString(reader, "especificacao_local"),
                TipoLogradouro = ReaderString(reader, "tipo_logradouro"),
                Numero = ReaderString(reader, "numero"),
                TipoInscricao = ReaderString(reader, "tipo_inscricao"),
                InscricaoEstabelecimento = ReaderString(reader, "inscricao_estabelecimento"),
                Logradouro = ReaderString(reader, "logradouro"),
                Municipio = ReaderString(reader, "municipio"),
                Uf = ReaderString(reader, "uf"),
                Bairro = ReaderString(reader, "bairro"),
                Complemento = ReaderString(reader, "complemento"),
                Cep = ReaderString(reader, "cep"),
                CodigoPostal = ReaderString(reader, "codigo_postal"),
                ObservacaoCat = ReaderString(reader, "observacao_cat"),
                RazaoSocialEmpregador = ReaderString(reader, "razao_social_empregador"),
                CnaeEmpregador = ReaderString(reader, "cnae_empregador")
            };
        }

        private static string ReaderString(MySqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }

        private static bool ReaderBool(MySqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);
            return !reader.IsDBNull(ordinal) && Convert.ToInt32(reader.GetValue(ordinal)) == 1;
        }

        private static string ReaderDate(MySqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetDateTime(ordinal).ToString("dd/MM/yyyy");
        }

        private static string JsonEscape(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string ExtrairTrabalhadorEsocial(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                return string.Empty;

            int index = descricao.IndexOf(" - ", StringComparison.Ordinal);
            if (index >= 0 && index < descricao.Length - 3)
                return descricao.Substring(index + 3);

            return descricao;
        }

        private static object EmptyToDbNull(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value.Trim();
        }

        private static string NormalizarResultadoAso(string resultado)
        {
            if (string.IsNullOrWhiteSpace(resultado))
                return "Aguardando ASO de Retorno";

            string valor = resultado.Trim();

            if (string.Equals(valor, "apto", StringComparison.OrdinalIgnoreCase))
                return "Apto";

            if (string.Equals(valor, "inapto", StringComparison.OrdinalIgnoreCase))
                return "Inapto";

            if (valor.StartsWith("aguardando", StringComparison.OrdinalIgnoreCase))
                return "Aguardando ASO de Retorno";

            return valor;
        }

        private static bool IsAsoRetornoTrabalho(string tipoExame)
        {
            return !string.IsNullOrWhiteSpace(tipoExame) &&
                   tipoExame.IndexOf("retorno", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void AtualizarStatusEmpregadoPorCat(MySqlConnection connection, int empregadoId, bool houveAfastamento)
        {
            using (MySqlCommand command = new MySqlCommand(
                @"UPDATE empregados
                  SET status_aso = @status_aso
                  WHERE id = @empregado_id
                    AND (status_aso IS NULL
                      OR status_aso = ''
                      OR status_aso IN ('Pendente', 'Vigente', 'A vencer', 'Vencido', 'Apto'))", connection))
            {
                command.Parameters.AddWithValue("@empregado_id", empregadoId);
                command.Parameters.AddWithValue("@status_aso", houveAfastamento ? "Aguardando retorno" : "CAT aberta");
                command.ExecuteNonQuery();
            }
        }

        private static object DateToDbNull(string value)
        {
            DateTime parsed;
            if (DateTime.TryParseExact(value, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out parsed))
                return parsed.ToString("yyyy-MM-dd");

            return DBNull.Value;
        }

        private static object TimeToDbNull(string value)
        {
            TimeSpan parsed;
            if (TimeSpan.TryParseExact(value, @"hh\:mm", null, out parsed))
                return parsed.ToString(@"hh\:mm\:ss");

            return DBNull.Value;
        }
    }
}
