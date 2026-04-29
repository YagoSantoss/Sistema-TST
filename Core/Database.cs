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
                using (MySqlCommand command = new MySqlCommand("ALTER TABLE cats ADD COLUMN resultado_aso VARCHAR(40) NOT NULL DEFAULT 'Aguardando ASO' AFTER situacao", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            schemaChecked = true;
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
    }

    public sealed class MedicoRecord
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Crm { get; set; }
        public string OrgaoUf { get; set; }
        public string Especialidade { get; set; }
        public string Email { get; set; }
    }

    public sealed class TipoExameRecord
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Periodicidade { get; set; }
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
    }

    public sealed class CatRecord
    {
        public int Id { get; set; }
        public int EmpregadoId { get; set; }
        public string EmpregadoNome { get; set; }
        public string DataAcidente { get; set; }
        public string HoraAcidente { get; set; }
        public string DataComunicacao { get; set; }
        public string LocalAcidente { get; set; }
        public string Descricao { get; set; }
        public string TipoCat { get; set; }
        public string Situacao { get; set; }
        public string ResultadoAso { get; set; }
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
                command.ExecuteNonQuery();
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
                @"SELECT id, nome, crm, orgao_uf, especialidade, email
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
                        Email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString("email")
                    });
                }
            }

            return medicos;
        }

        public static MedicoRecord GetMedico(int id)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT id, nome, crm, orgao_uf, especialidade, email
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
                        Email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString("email")
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
                        SET nome = @nome, crm = @crm, orgao_uf = @orgao_uf, especialidade = @especialidade, email = @email
                        WHERE id = @id"
                    : @"INSERT INTO medicos (nome, crm, orgao_uf, especialidade, email)
                        VALUES (@nome, @crm, @orgao_uf, @especialidade, @email)", connection))
            {
                command.Parameters.AddWithValue("@id", medico.Id);
                command.Parameters.AddWithValue("@nome", medico.Nome);
                command.Parameters.AddWithValue("@crm", medico.Crm);
                command.Parameters.AddWithValue("@orgao_uf", medico.OrgaoUf);
                command.Parameters.AddWithValue("@especialidade", medico.Especialidade);
                command.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(medico.Email) ? (object)DBNull.Value : medico.Email);
                command.ExecuteNonQuery();
            }
        }

        public static List<TipoExameRecord> GetTiposExames()
        {
            List<TipoExameRecord> exames = new List<TipoExameRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT id, codigo, nome, tipo, periodicidade
                  FROM tipos_exames
                  WHERE ativo = 1
                  ORDER BY nome", connection))
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
                        Periodicidade = reader.GetString("periodicidade")
                    });
                }
            }

            return exames;
        }

        public static TipoExameRecord GetTipoExame(int id)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT id, codigo, nome, tipo, periodicidade
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
                        Codigo = reader.GetString("codigo"),
                        Nome = reader.GetString("nome"),
                        Tipo = reader.GetString("tipo"),
                        Periodicidade = reader.GetString("periodicidade")
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
                        SET codigo = @codigo, nome = @nome, tipo = @tipo, periodicidade = @periodicidade
                        WHERE id = @id"
                    : @"INSERT INTO tipos_exames (codigo, nome, tipo, periodicidade)
                        VALUES (@codigo, @nome, @tipo, @periodicidade)", connection))
            {
                command.Parameters.AddWithValue("@id", exame.Id);
                command.Parameters.AddWithValue("@codigo", exame.Codigo);
                command.Parameters.AddWithValue("@nome", exame.Nome);
                command.Parameters.AddWithValue("@tipo", exame.Tipo);
                command.Parameters.AddWithValue("@periodicidade", exame.Periodicidade);
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
                  WHERE id = @id
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

        public static List<EmpregadoRecord> GetEmpregados()
        {
            List<EmpregadoRecord> empregados = new List<EmpregadoRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT id, matricula, nome, cpf, setor, cargo, data_admissao, data_vencimento_aso, status_aso
                  FROM empregados
                  WHERE ativo = 1
                  ORDER BY nome", connection))
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
                        StatusAso = reader.GetString("status_aso")
                    });
                }
            }

            return empregados;
        }

        public static void SaveEmpregado(EmpregadoRecord empregado)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                empregado.Id > 0
                    ? @"UPDATE empregados
                        SET matricula = @matricula, nome = @nome, cpf = @cpf, setor = @setor, cargo = @cargo,
                            data_admissao = @data_admissao, data_vencimento_aso = @data_vencimento_aso, status_aso = @status_aso
                        WHERE id = @id"
                    : @"INSERT INTO empregados (matricula, nome, cpf, setor, cargo, data_admissao, data_vencimento_aso, status_aso)
                        VALUES (@matricula, @nome, @cpf, @setor, @cargo, @data_admissao, @data_vencimento_aso, @status_aso)", connection))
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
                command.ExecuteNonQuery();
            }
        }

        public static List<CatRecord> GetCats(string termo)
        {
            List<CatRecord> cats = new List<CatRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT c.id, c.empregado_id, e.nome AS empregado_nome, c.data_acidente, c.hora_acidente,
                         c.data_comunicacao, c.local_acidente, c.descricao, c.tipo_cat, c.situacao, c.resultado_aso
                  FROM cats c
                  INNER JOIN empregados e ON e.id = c.empregado_id
                  WHERE @termo = ''
                     OR e.nome LIKE @busca
                     OR e.matricula LIKE @busca
                     OR c.tipo_cat LIKE @busca
                     OR c.situacao LIKE @busca
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

        public static List<CatRecord> GetCatsPorEmpregado(int empregadoId)
        {
            List<CatRecord> cats = new List<CatRecord>();

            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                @"SELECT c.id, c.empregado_id, e.nome AS empregado_nome, c.data_acidente, c.hora_acidente,
                         c.data_comunicacao, c.local_acidente, c.descricao, c.tipo_cat, c.situacao, c.resultado_aso
                  FROM cats c
                  INNER JOIN empregados e ON e.id = c.empregado_id
                  WHERE c.empregado_id = @empregado_id
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
                         c.data_comunicacao, c.local_acidente, c.descricao, c.tipo_cat, c.situacao, c.resultado_aso
                  FROM cats c
                  INNER JOIN empregados e ON e.id = c.empregado_id
                  WHERE c.id = @id
                  LIMIT 1", connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? ReadCat(reader) : null;
                }
            }
        }

        public static void SaveCat(CatRecord cat)
        {
            using (MySqlConnection connection = Database.OpenConnection())
            using (MySqlCommand command = new MySqlCommand(
                cat.Id > 0
                    ? @"UPDATE cats
                        SET empregado_id = @empregado_id, data_acidente = @data_acidente, hora_acidente = @hora_acidente,
                            data_comunicacao = @data_comunicacao, local_acidente = @local_acidente,
                            descricao = @descricao, tipo_cat = @tipo_cat, situacao = @situacao, resultado_aso = @resultado_aso
                        WHERE id = @id"
                    : @"INSERT INTO cats (empregado_id, data_acidente, hora_acidente, data_comunicacao, local_acidente, descricao, tipo_cat, situacao, resultado_aso)
                        VALUES (@empregado_id, @data_acidente, @hora_acidente, @data_comunicacao, @local_acidente, @descricao, @tipo_cat, @situacao, @resultado_aso)", connection))
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
                command.Parameters.AddWithValue("@resultado_aso", string.IsNullOrWhiteSpace(cat.ResultadoAso) ? "Aguardando ASO" : cat.ResultadoAso);
                command.ExecuteNonQuery();
            }
        }

        public static void SaveAso(AsoRecord aso)
        {
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
                command.Parameters.AddWithValue("@resultado", aso.Resultado);
                command.Parameters.AddWithValue("@observacoes", EmptyToDbNull(aso.Observacoes));
                command.ExecuteNonQuery();
            }

            if (aso.CatId.HasValue && aso.CatId.Value > 0)
            {
                using (MySqlConnection connection = Database.OpenConnection())
                using (MySqlCommand command = new MySqlCommand(
                    @"UPDATE cats
                      SET resultado_aso = @resultado_aso,
                          situacao = 'Avaliada'
                      WHERE id = @cat_id", connection))
                {
                    command.Parameters.AddWithValue("@cat_id", aso.CatId.Value);
                    command.Parameters.AddWithValue("@resultado_aso", aso.Resultado);
                    command.ExecuteNonQuery();
                }
            }
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
                  WHERE a.empregado_id = @empregado_id
                  ORDER BY a.data_aso DESC, a.id DESC", connection))
            {
                command.Parameters.AddWithValue("@empregado_id", empregadoId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        asos.Add(new AsoRecord
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
                        });
                    }
                }
            }

            return asos;
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
                LocalAcidente = reader.IsDBNull(reader.GetOrdinal("local_acidente")) ? string.Empty : reader.GetString("local_acidente"),
                Descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? string.Empty : reader.GetString("descricao"),
                TipoCat = reader.IsDBNull(reader.GetOrdinal("tipo_cat")) ? string.Empty : reader.GetString("tipo_cat"),
                Situacao = reader.GetString("situacao"),
                ResultadoAso = reader.IsDBNull(reader.GetOrdinal("resultado_aso")) ? "Aguardando ASO" : reader.GetString("resultado_aso")
            };
        }

        private static object EmptyToDbNull(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value.Trim();
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
