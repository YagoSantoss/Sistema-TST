# Apresentacao do Projeto - Sistema TST Largo Treze

## 1. Abertura

Bom dia/boa tarde.

Meu projeto se chama **Sistema TST Largo Treze**.

Ele foi desenvolvido para ajudar no aprendizado e na organizacao das rotinas de **Saude e Seguranca do Trabalho**, tambem chamada de **SST**.

A ideia principal do sistema e facilitar o cadastro, consulta e acompanhamento de informacoes importantes para a area de seguranca do trabalho, como empregados, medicos, exames, CAT, ASO, fatores de risco e eventos simulados do eSocial.

## 2. O que e SST?

SST significa **Saude e Seguranca do Trabalho**.

Essa area cuida da prevencao de acidentes, do acompanhamento da saude dos trabalhadores e do controle dos riscos existentes no ambiente de trabalho.

Na pratica, a SST ajuda a responder perguntas como:

- O trabalhador esta apto para exercer a funcao?
- Existe algum risco no ambiente de trabalho?
- Ocorreu algum acidente?
- A empresa registrou corretamente esse acidente?
- O funcionario precisa fazer exames?
- Existem documentos que precisam ser guardados ou enviados ao eSocial?

## 3. Objetivo do sistema

O objetivo do Sistema TST e criar uma ferramenta simples e didatica para simular como sistemas reais de SST funcionam.

Ele foi inspirado em sistemas usados no mercado, como SOC, TOTVS SST e outras plataformas de medicina e seguranca do trabalho.

O sistema nao substitui esses sistemas profissionais, mas ajuda os alunos a entenderem os principais fluxos de uma rotina real de SST.

## 4. Principais modulos do sistema

O sistema foi organizado em modulos.

### Empregados

Nesta tela sao cadastrados os funcionarios.

O sistema guarda informacoes como:

- matricula;
- nome;
- CPF;
- setor;
- cargo;
- data de admissao;
- status do ASO;
- medico responsavel, quando houver.

Esse cadastro e importante porque os outros documentos do sistema, como CAT e ASO, precisam estar ligados a um empregado.

### Medicos

Nesta tela sao cadastrados os medicos ou responsaveis tecnicos.

O sistema permite registrar:

- nome;
- CRM;
- UF;
- orgao de classe;
- contato;
- endereco;
- dados profissionais.

Esses medicos podem ser usados nos ASOs e nos dados medicos da CAT.

### Exames Realizados

Nesta parte sao cadastrados os exames realizados pelos empregados.

Cada exame pode ser ligado a:

- um empregado;
- um medico responsavel;
- um tipo de exame;
- uma data;
- um resultado;
- um anexo, como imagem ou documento.

Isso ajuda a organizar o historico de saude ocupacional do trabalhador.

### Ambientes de Trabalho

Aqui sao cadastrados os locais de trabalho.

Exemplos:

- setor administrativo;
- laboratorio;
- oficina;
- sala de aula;
- area operacional.

Esse cadastro e usado principalmente para relacionar os ambientes aos fatores de risco.

### Fatores de Risco

Fatores de risco sao situacoes do ambiente de trabalho que podem prejudicar a saude ou a seguranca do trabalhador.

Exemplos:

- ruido;
- calor;
- produtos quimicos;
- agentes biologicos;
- postura inadequada;
- risco de queda.

No sistema, o fator de risco pode ser vinculado ao empregado e ao ambiente de trabalho.

Esse modulo representa, de forma simplificada, o evento **S-2240** do eSocial.

### CAT

CAT significa **Comunicacao de Acidente de Trabalho**.

Ela deve ser registrada quando ocorre um acidente de trabalho ou uma doenca ocupacional.

No sistema, a CAT possui:

- dados cadastrais;
- informacoes do acidente;
- local do acidente;
- testemunhas;
- dados complementares;
- medico assistente;
- PDF para impressao.

Um ponto importante:

**A CAT nao define se o funcionario esta apto ou inapto.**

A CAT apenas registra o acidente.

Quem define se o funcionario pode voltar ao trabalho e o **ASO de retorno ao trabalho**.

### ASO

ASO significa **Atestado de Saude Ocupacional**.

Ele e o documento medico que informa se o trabalhador esta **apto** ou **inapto** para exercer a funcao.

No sistema, o ASO pode ser usado para:

- exame admissional;
- exame periodico;
- exame demissional;
- retorno ao trabalho;
- mudanca de risco.

Quando existe uma CAT com afastamento, o sistema permite vincular essa CAT a um ASO de retorno.

Se o medico marcar o resultado como **Apto**, a CAT fica encerrada com retorno apto.

Se o medico marcar como **Inapto**, a CAT continua aguardando novo ASO de retorno.

### eSocial SST - Simulado

O sistema possui uma tela chamada **eSocial SST - Simulado**.

Ela nao envia dados oficialmente para o governo.

Ela serve para aprendizado e demonstracao.

Nessa tela, o aluno consegue visualizar eventos como:

- **S-2210**: CAT;
- **S-2220**: ASO;
- **S-2240**: fatores de risco.

Tambem existe um log interno, simulando protocolos e recibos.

Isso ajuda a entender como os dados de SST se organizam antes de serem enviados em sistemas reais.

## 5. Fluxo principal do sistema

O fluxo recomendado de uso e:

1. Cadastrar o empregado.
2. Cadastrar o medico.
3. Cadastrar o ambiente de trabalho.
4. Registrar exames realizados.
5. Criar ASO quando necessario.
6. Criar CAT se houver acidente.
7. Adicionar testemunhas e dados complementares da CAT.
8. Se houve afastamento, criar ASO de retorno ao trabalho.
9. Marcar o empregado como apto ou inapto pelo ASO.
10. Consultar o eSocial SST Simulado.
11. Gerar PDFs para estudo, impressao ou apresentacao.

## 6. Exemplo simples de uso

Imagine que um funcionario sofreu um acidente no trabalho.

Primeiro, o usuario cadastra ou seleciona esse empregado.

Depois, cria uma CAT informando:

- data do acidente;
- hora;
- local;
- tipo de acidente;
- parte do corpo atingida;
- testemunhas;
- medico assistente.

Se o trabalhador ficou afastado, ele nao deve ser considerado automaticamente apto.

Depois do tratamento, ele passa por um **ASO de retorno ao trabalho**.

Nesse ASO, o medico define:

- **Apto**, se ele pode voltar;
- **Inapto**, se ele ainda nao pode voltar.

Assim o sistema fica mais parecido com a rotina real da SST.

## 7. Recursos importantes do projeto

O sistema possui:

- login e cadastro de usuario;
- recuperacao de senha por e-mail;
- conexao com MySQL;
- cadastro de empregados;
- cadastro de medicos;
- cadastro de exames realizados;
- cadastro de ambientes;
- cadastro de fatores de risco;
- criacao de CAT;
- criacao de ASO;
- vinculacao entre CAT e ASO;
- geracao de PDF;
- uso da logo do Senac;
- consulta ViaCEP;
- consulta de CNPJ;
- painel eSocial simulado.

## 8. Por que o projeto e util?

O projeto e util porque ajuda alunos a entenderem, de forma visual e pratica, como as informacoes de SST se conectam.

Em vez de estudar apenas teoria, o aluno consegue ver:

- como um empregado e cadastrado;
- como um acidente e registrado;
- como o medico avalia a aptidao;
- como os riscos sao relacionados ao ambiente;
- como documentos podem ser gerados;
- como o eSocial organiza eventos de SST.

## 9. Limites do sistema

E importante explicar que este sistema e um projeto academico.

Ele nao faz envio oficial ao governo.

Ele nao substitui sistemas profissionais como SOC ou TOTVS.

Ele nao usa certificado digital.

Ele nao gera XML oficial do eSocial.

O objetivo e ser uma ferramenta didatica, simples e funcional para ensino e demonstracao.

## 10. Conclusao

Concluindo, o Sistema TST Largo Treze foi criado para apoiar o aprendizado de rotinas de Saude e Seguranca do Trabalho.

Ele organiza informacoes importantes de empregados, medicos, exames, CAT, ASO, fatores de risco e eSocial simulado.

O principal diferencial do projeto e mostrar o fluxo completo de forma simples:

**cadastro do empregado, registro do acidente, avaliacao medica, controle de retorno e simulacao dos eventos de SST.**

Assim, o sistema ajuda os alunos a entenderem melhor como funciona uma rotina real de SST dentro de uma empresa.

## 11. Fala final sugerida

Este projeto foi desenvolvido com foco academico, mas buscando seguir a logica de sistemas reais de SST.

Ele permite que alunos pratiquem os principais cadastros e fluxos da area, entendendo como as informacoes se conectam e como documentos como CAT e ASO fazem parte da rotina de seguranca do trabalho.

Obrigado.

