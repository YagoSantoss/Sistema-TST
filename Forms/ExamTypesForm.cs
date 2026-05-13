using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class ExamTypesForm : DashboardFormBase
    {
        public ExamTypesForm()
        {
            InitializeComponent();
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            AbrirCadastro(false, 0);
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            int id = control != null && control.Tag is int ? (int)control.Tag : 0;
            AbrirCadastro(true, id);
        }

        private void AbrirCadastro(bool edicao, int id)
        {
            using (CadastroBaseForm form = new CadastroBaseForm(CadastroBaseTipo.TipoExame, edicao, id))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    MontarConteudoTiposExame();
                }
            }
        }

        private void Selecionado_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            if (check == null || !(check.Tag is int))
                return;

            int id = (int)check.Tag;
            if (check.Checked)
                _selecionados.Add(id);
            else
                _selecionados.Remove(id);

            if (check.Parent != null)
                check.Parent.BackColor = check.Checked ? System.Drawing.Color.FromArgb(255, 244, 229) : System.Drawing.Color.White;
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (_selecionados.Count == 0)
            {
                MessageBox.Show("Selecione um ou mais exames realizados para excluir.", "Exames Realizados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Deseja excluir os exames realizados selecionados?", "Exames Realizados", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                CadastrosRepository.DeleteTiposExames(new List<int>(_selecionados));
                _selecionados.Clear();
                MontarConteudoTiposExame();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel excluir no MySQL.\n\n" + ex.Message, "Exames Realizados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
