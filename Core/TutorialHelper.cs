using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public static class TutorialHelper
    {
        public static void Show(DashboardMenu menu, IWin32Window owner)
        {
            MessageBox.Show(GetText(menu), "Tutorial rapido", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string GetText(DashboardMenu menu)
        {
            switch (menu)
            {
                case DashboardMenu.Risk:
                    return "Fatores de Risco (S-2240)\n\n1. Cadastre o empregado e o ambiente antes.\n2. Informe data de avaliação, exposição, agente e atividades.\n3. Se o trabalhador usa EPI, marque o checkbox e selecione os EPIs.\n4. Salve para o registro aparecer na lista e no controle eSocial.";

                case DashboardMenu.Cat:
                    return "CAT - Comunicação de Acidente (S-2210)\n\n1. Comece por Dados Cadastrais.\n2. Preencha todos os campos com asterisco vermelho.\n3. Avance para Testemunhas e depois Dados Complementares.\n4. Ao finalizar, a CAT fica salva, pode gerar PDF e pode ser acompanhada pelo ASO de retorno.";

                case DashboardMenu.Aso:
                    return "ASO - Monitoramento da Saúde (S-2220)\n\n1. Escolha o empregado e o médico responsável.\n2. Se for retorno de CAT, vincule a CAT correspondente.\n3. Informe o resultado: Apto ou Inapto.\n4. Salve o ASO para atualizar a situacao do empregado e da CAT.";

                case DashboardMenu.Employees:
                    return "Empregados\n\n1. Cadastre ou importe empregados pela planilha.\n2. Use Buscar/Atualizar para recarregar a lista.\n3. Selecione um empregado para editar.\n4. Para excluir, você pode selecionar um ou mais registros.";

                case DashboardMenu.Doctors:
                    return "Médicos / Responsaveis Tecnicos\n\n1. Cadastre nome, CRM, UF, órgão de classe e contato.\n2. Esses dados sao usados na CAT, ASO e relatorios.\n3. Mantenha CRM e UF preenchidos para deixar os documentos mais completos.";

                case DashboardMenu.ExamTypes:
                    return "Exames Realizados\n\n1. Cadastre o exame feito pelo paciente.\n2. Vincule o empregado e o médico responsável.\n3. Se existir laudo, anexe o PDF.\n4. Depois o exame pode ser usado no ASO do paciente.";

                case DashboardMenu.WorkEnvironments:
                    return "Ambientes de Trabalho\n\n1. Cadastre o ambiente, setor e status.\n2. Use ambientes ativos nos fatores de risco.\n3. Isso ajuda a mostrar onde o empregado está exposto aos riscos.";

                case DashboardMenu.Esocial:
                    return "Integração eSocial Simulada\n\n1. Esta tela organiza os eventos SST do projeto.\n2. S-2210 representa CAT, S-2220 representa ASO e S-2240 representa fatores de risco.\n3. Use Registrar no Log para simular transmissão.\n4. Pesquise logs por data, protocolo ou número de recibo.";

                default:
                    return "Use os menus laterais para navegar pelos cadastros e fluxos SST.";
            }
        }
    }
}
