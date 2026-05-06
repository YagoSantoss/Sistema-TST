CREATE DATABASE IF NOT EXISTS sistema_tst
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE sistema_tst;

CREATE TABLE IF NOT EXISTS usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    email VARCHAR(180) NOT NULL UNIQUE,
    senha_hash VARCHAR(255) NOT NULL,
    perfil VARCHAR(50) NOT NULL DEFAULT 'Usuario',
    ativo TINYINT(1) NOT NULL DEFAULT 1,
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME NULL ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS empregados (
    id INT AUTO_INCREMENT PRIMARY KEY,
    matricula VARCHAR(30) NOT NULL UNIQUE,
    nome VARCHAR(180) NOT NULL,
    cpf VARCHAR(20) NOT NULL UNIQUE,
    setor VARCHAR(100),
    cargo VARCHAR(100),
    data_admissao DATE,
    data_vencimento_aso DATE,
    status_aso VARCHAR(40) NOT NULL DEFAULT 'Pendente',
    medico_id INT NULL,
    ativo TINYINT(1) NOT NULL DEFAULT 1,
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_empregados_nome (nome)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS medicos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(180) NOT NULL,
    crm VARCHAR(30) NOT NULL,
    orgao_uf VARCHAR(20) NOT NULL,
    especialidade VARCHAR(120) NOT NULL,
    email VARCHAR(180),
    ativo TINYINT(1) NOT NULL DEFAULT 1,
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY uk_medicos_crm_orgao (crm, orgao_uf),
    INDEX idx_medicos_nome (nome)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tipos_exames (
    id INT AUTO_INCREMENT PRIMARY KEY,
    codigo VARCHAR(30) NOT NULL UNIQUE,
    nome VARCHAR(180) NOT NULL,
    tipo VARCHAR(80) NOT NULL,
    periodicidade VARCHAR(80) NOT NULL,
    anexo_imagem VARCHAR(500),
    empregado_id INT NULL,
    medico_id INT NULL,
    ativo TINYINT(1) NOT NULL DEFAULT 1,
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_tipos_exames_nome (nome),
    CONSTRAINT fk_tipos_exames_empregado
        FOREIGN KEY (empregado_id) REFERENCES empregados (id)
        ON DELETE SET NULL,
    CONSTRAINT fk_tipos_exames_medico
        FOREIGN KEY (medico_id) REFERENCES medicos (id)
        ON DELETE SET NULL
) ENGINE=InnoDB;

ALTER TABLE empregados
    ADD CONSTRAINT fk_empregados_medico
    FOREIGN KEY (medico_id) REFERENCES medicos (id)
    ON DELETE SET NULL;

CREATE TABLE IF NOT EXISTS ambientes_trabalho (
    id INT AUTO_INCREMENT PRIMARY KEY,
    codigo VARCHAR(30) NOT NULL UNIQUE,
    ambiente VARCHAR(180) NOT NULL,
    setor VARCHAR(100) NOT NULL,
    status VARCHAR(40) NOT NULL DEFAULT 'Ativo',
    ativo TINYINT(1) NOT NULL DEFAULT 1,
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_ambientes_setor (setor)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS asos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    empregado_id INT NOT NULL,
    medico_id INT NOT NULL,
    cat_id INT NULL,
    data_aso DATE NOT NULL,
    tipo_exame VARCHAR(80) NOT NULL,
    resultado VARCHAR(40) NOT NULL,
    observacoes TEXT,
    ativo TINYINT(1) NOT NULL DEFAULT 1,
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_asos_empregado (empregado_id),
    CONSTRAINT fk_asos_empregado
        FOREIGN KEY (empregado_id) REFERENCES empregados (id),
    CONSTRAINT fk_asos_medico
        FOREIGN KEY (medico_id) REFERENCES medicos (id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS aso_exames (
    id INT AUTO_INCREMENT PRIMARY KEY,
    aso_id INT NOT NULL,
    tipo_exame_id INT NOT NULL,
    data_exame DATE,
    resultado VARCHAR(80),
    observacoes TEXT,
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_aso_exames_aso
        FOREIGN KEY (aso_id) REFERENCES asos (id)
        ON DELETE CASCADE,
    CONSTRAINT fk_aso_exames_tipo
        FOREIGN KEY (tipo_exame_id) REFERENCES tipos_exames (id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS cats (
    id INT AUTO_INCREMENT PRIMARY KEY,
    empregado_id INT NOT NULL,
    data_acidente DATE NOT NULL,
    hora_acidente TIME,
    data_comunicacao DATE,
    local_acidente VARCHAR(180),
    descricao TEXT,
    tipo_cat VARCHAR(80),
    situacao VARCHAR(40) NOT NULL DEFAULT 'Aberta',
    resultado_aso VARCHAR(40) NOT NULL DEFAULT 'Aguardando ASO',
    ativo TINYINT(1) NOT NULL DEFAULT 1,
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_cats_empregado (empregado_id),
    CONSTRAINT fk_cats_empregado
        FOREIGN KEY (empregado_id) REFERENCES empregados (id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS cat_testemunhas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cat_id INT NOT NULL,
    nome VARCHAR(180) NOT NULL,
    cpf VARCHAR(20),
    telefone VARCHAR(30),
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_cat_testemunhas_cat
        FOREIGN KEY (cat_id) REFERENCES cats (id)
        ON DELETE CASCADE
) ENGINE=InnoDB;

ALTER TABLE asos
    ADD CONSTRAINT fk_asos_cat
    FOREIGN KEY (cat_id) REFERENCES cats (id)
    ON DELETE SET NULL;

CREATE TABLE IF NOT EXISTS fatores_risco (
    id INT AUTO_INCREMENT PRIMARY KEY,
    empregado_id INT,
    ambiente_id INT,
    tipo_fator VARCHAR(80) NOT NULL,
    agente VARCHAR(120) NOT NULL,
    intensidade VARCHAR(80),
    tecnica_medicao VARCHAR(120),
    data_avaliacao DATE,
    inicio_exposicao DATE,
    fim_exposicao DATE,
    descricao_atividades TEXT,
    usa_epi TINYINT(1) NOT NULL DEFAULT 0,
    epi_eficaz TINYINT(1) NOT NULL DEFAULT 0,
    epi_descricao TEXT,
    ativo TINYINT(1) NOT NULL DEFAULT 1,
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT fk_fatores_empregado
        FOREIGN KEY (empregado_id) REFERENCES empregados (id),
    CONSTRAINT fk_fatores_ambiente
        FOREIGN KEY (ambiente_id) REFERENCES ambientes_trabalho (id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS esocial_eventos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    codigo_evento VARCHAR(20) NOT NULL,
    descricao VARCHAR(255) NOT NULL,
    status VARCHAR(40) NOT NULL DEFAULT 'Pendente',
    payload JSON,
    protocolo VARCHAR(100),
    recibo VARCHAR(100),
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    enviado_em DATETIME,
    atualizado_em DATETIME NULL ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS esocial_logs (
    id INT AUTO_INCREMENT PRIMARY KEY,
    evento_id INT,
    data_hora DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    mensagem TEXT NOT NULL,
    status VARCHAR(40) NOT NULL,
    detalhes TEXT,
    CONSTRAINT fk_esocial_logs_evento
        FOREIGN KEY (evento_id) REFERENCES esocial_eventos (id)
        ON DELETE SET NULL
) ENGINE=InnoDB;
