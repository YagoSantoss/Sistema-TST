USE sistema_tst;

ALTER TABLE tipos_exames
    ADD COLUMN anexo_imagem VARCHAR(500) NULL AFTER periodicidade;

ALTER TABLE empregados
    ADD COLUMN medico_id INT NULL AFTER status_aso;

ALTER TABLE empregados
    ADD CONSTRAINT fk_empregados_medico
    FOREIGN KEY (medico_id) REFERENCES medicos (id)
    ON DELETE SET NULL;
