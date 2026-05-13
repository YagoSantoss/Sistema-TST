# Sistema TST Largo Treze

Sistema Windows Forms para apoio didatico a rotinas de Medicina do Trabalho e SST, com foco em cadastro, consulta, emissao de documentos e controle interno simulado de eventos relacionados ao eSocial SST.

## Objetivo

Centralizar informacoes de empregados, medicos, ambientes de trabalho, exames, ASO, CAT e fatores de risco, permitindo consulta, impressao em PDF e registro interno dos eventos SST.

## Fluxo recomendado de uso

1. Cadastre os empregados.
2. Cadastre os medicos responsaveis.
3. Cadastre os ambientes de trabalho.
4. Lance os exames realizados, vinculando empregado e medico responsavel.
5. Registre o ASO do empregado e vincule os exames realizados.
6. Registre a CAT quando houver acidente ou doenca ocupacional.
7. Quando houver afastamento por CAT, registre depois um ASO de retorno ao trabalho para definir se o empregado esta apto ou inapto.
8. Cadastre fatores de risco vinculados ao empregado e/ou ambiente.
9. Emita PDFs de CAT, ASO e fatores de risco quando necessario.
10. Use o eSocial SST - Simulado como painel didatico dos eventos S-2210, S-2220 e S-2240.

## Banco de dados

O projeto usa MySQL com o banco `sistema_tst`.

Arquivo principal para criacao do banco:

`Data/mysql_sistema_tst.sql`

O sistema tambem possui ajustes de schema em tempo de execucao para manter bancos antigos compativeis durante o desenvolvimento.

## Principais telas

- Empregados: cadastro, edicao, exclusao logica, busca, historico de ASO e exportacao CSV.
- Medicos: cadastro de responsaveis tecnicos, CRM/UF, contato e dados profissionais.
- Exames Realizados: exames lancados para um empregado, com medico responsavel e PDF anexado no banco de dados quando houver.
- Ambientes de Trabalho: cadastro de local/setor/status.
- CAT: dados cadastrais, testemunhas, dados complementares, listagem e PDF.
- ASO: registro de apto/inapto, medico, CAT vinculada, exames realizados e PDF.
- Fatores de Risco: registro de agente, ambiente, empregado, exposicao, EPI e PDF.
- eSocial SST - Simulado: controle interno dos eventos S-2210, S-2220 e S-2240, com log de transmissao simulado.

## Observacao sobre eSocial

A tela de eSocial SST - Simulado nao realiza envio oficial ao governo, nao gera XML oficial e nao usa certificado digital. Ela registra eventos em log interno para fins de aprendizagem, controle, simulacao e demonstracao academica do fluxo usado em sistemas de SST.

## Observacao sobre CAT e ASO

A CAT registra o acidente ou doenca ocupacional. A aptidao do empregado nao deve ser definida pela CAT. Quando houver retorno ao trabalho, o resultado apto/inapto deve ser registrado no ASO de retorno ao trabalho, assinado por medico responsavel.

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
- O eSocial SST e apenas controle interno/simulacao.
- Os dados tecnicos de SST devem ser preenchidos com base em laudos e informacoes de profissionais responsaveis.
- Em ambiente real, dados de SMTP e banco nao devem ficar fixos no `App.config`; para este projeto, eles foram mantidos simples para uso local e academico.
