USE sistema_tst;

ALTER TABLE asos
    ADD COLUMN cat_id INT NULL AFTER medico_id;

ALTER TABLE asos
    ADD CONSTRAINT fk_asos_cat
    FOREIGN KEY (cat_id) REFERENCES cats (id)
    ON DELETE SET NULL;

ALTER TABLE cats
    ADD COLUMN resultado_aso VARCHAR(40) NOT NULL DEFAULT 'Aguardando ASO' AFTER situacao;
