ALTER TABLE tipos_exames ADD COLUMN empregado_id INT NULL AFTER anexo_imagem;
ALTER TABLE tipos_exames ADD COLUMN medico_id INT NULL AFTER empregado_id;

ALTER TABLE tipos_exames
    ADD CONSTRAINT fk_tipos_exames_empregado
    FOREIGN KEY (empregado_id) REFERENCES empregados (id)
    ON DELETE SET NULL;

ALTER TABLE tipos_exames
    ADD CONSTRAINT fk_tipos_exames_medico
    FOREIGN KEY (medico_id) REFERENCES medicos (id)
    ON DELETE SET NULL;
