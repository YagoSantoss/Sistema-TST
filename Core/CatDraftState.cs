namespace SistemaTstLargoTreze
{
    public sealed class CatDraft
    {
        public string DataAcidente { get; set; }
        public string HoraAcidente { get; set; }
        public string DataObito { get; set; }
        public string TipoCat { get; set; }
        public int EmpregadoId { get; set; }
        public string Emitente { get; set; }
        public string LocalAcidente { get; set; }
        public string EntradaTrabalho { get; set; }
        public string SaidaTrabalho { get; set; }
        public string Descricao { get; set; }
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
