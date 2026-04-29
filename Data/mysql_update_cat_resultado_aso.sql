USE sistema_tst;

ALTER TABLE cats
    ADD COLUMN resultado_aso VARCHAR(40) NOT NULL DEFAULT 'Aguardando ASO' AFTER situacao;
