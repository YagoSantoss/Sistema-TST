# Sistema TST Largo Treze

Sistema Windows Forms para apoio a rotinas de Medicina do Trabalho e SST, com foco em cadastro, consulta, emissao de documentos e controle interno de eventos relacionados ao eSocial.

## Objetivo

Centralizar informacoes de empregados, medicos, ambientes de trabalho, exames, ASO, CAT e fatores de risco, permitindo consulta, impressao em PDF e registro interno dos eventos SST.

## Fluxo recomendado de uso

1. Cadastre os empregados.
2. Cadastre os medicos responsaveis.
3. Cadastre os ambientes de trabalho.
4. Cadastre os exames do paciente, vinculando empregado e medico.
5. Registre o ASO do empregado e vincule os exames realizados.
6. Registre a CAT quando houver acidente ou doenca ocupacional.
7. Cadastre fatores de risco vinculados ao empregado e/ou ambiente.
8. Emita PDFs de CAT, ASO e fatores de risco quando necessario.
9. Use a Integracao eSocial como controle interno/simulacao de eventos SST.

## Banco de dados

O projeto usa MySQL com o banco `sistema_tst`.

Arquivo principal para criacao do banco:

`Data/mysql_sistema_tst.sql`

O sistema tambem possui ajustes de schema em tempo de execucao para criar colunas ou tabelas complementares quando necessario.

## Principais telas

- Empregados: cadastro, edicao, exclusao logica, busca, historico de ASO e exportacao CSV.
- Medicos: cadastro de responsaveis tecnicos, CRM/UF, contato e dados profissionais.
- Exames do Paciente: exames cadastrados para um empregado e medico responsavel.
- Ambientes de Trabalho: cadastro de local/setor/status.
- CAT: dados cadastrais, testemunhas, dados complementares, listagem e PDF.
- ASO: registro de apto/inapto, medico, CAT vinculada, exames realizados e PDF.
- Fatores de Risco: registro de agente, ambiente, empregado, exposicao, EPI e PDF.
- Integracao eSocial: controle interno/simulacao de eventos e log de transmissao.

## Observacao sobre eSocial

A tela de Integracao eSocial nao realiza envio oficial ao governo. Ela registra eventos em log interno para fins de controle, simulacao e demonstracao academica do fluxo.

## Documentos gerados

Os PDFs e relatorios sao salvos em:

- `Documentos/SistemaTST/CATs`
- `Documentos/SistemaTST/ASOs`
- `Documentos/SistemaTST/FatoresRisco`
- `Documentos/SistemaTST/Relatorios`

## Requisitos para executar

- Windows
- .NET Framework 4.8
- MySQL em execucao, por exemplo pelo XAMPP
- Banco `sistema_tst` criado com o script SQL principal

## Pontos de atencao

- A recuperacao de senha por e-mail depende de SMTP configurado no `App.config`.
- A integracao eSocial e apenas controle interno/simulacao.
- Os dados tecnicos de SST devem ser preenchidos com base em laudos e informacoes de profissionais responsaveis.
