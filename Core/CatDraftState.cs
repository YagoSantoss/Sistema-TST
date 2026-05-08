namespace SistemaTstLargoTreze
{
    public sealed class CatDraft
    {
        public string DataAcidente { get; set; }
        public string HoraAcidente { get; set; }
        public string DataComunicacao { get; set; }
        public string DataObito { get; set; }
        public string TipoCat { get; set; }
        public int EmpregadoId { get; set; }
        public bool Aposentado { get; set; }
        public string Area { get; set; }
        public string FiliacaoPrevSocial { get; set; }
        public string Emitente { get; set; }
        public string TipoAcidente { get; set; }
        public string HorasTrabalhadasAntes { get; set; }
        public bool HouveObito { get; set; }
        public bool HouveAfastamento { get; set; }
        public bool RegistroPolicia { get; set; }
        public string UltimoDiaTrabalho { get; set; }
        public string CodificacaoAcidente { get; set; }
        public string SituacaoGeradora { get; set; }
        public string CatEmitidaPor { get; set; }
        public string LocalAcidente { get; set; }
        public string EspecificacaoLocal { get; set; }
        public string TipoLogradouro { get; set; }
        public string Numero { get; set; }
        public string TipoInscricao { get; set; }
        public string InscricaoEstabelecimento { get; set; }
        public string Logradouro { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string CodigoPostal { get; set; }
        public string EntradaTrabalho { get; set; }
        public string SaidaTrabalho { get; set; }
        public string Descricao { get; set; }
        public string ObservacaoCat { get; set; }
    }

    public static class CatDraftState
    {
        public static CatDraft Current { get; set; }

        public static void Clear()
        {
            Current = null;
        }
    }
}
