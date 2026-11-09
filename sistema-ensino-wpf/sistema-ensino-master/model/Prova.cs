namespace sistema_ensino.model
{
    public class Prova : AtividadeBase
    {
        private Prova()
        {
        }

        public Prova(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            CategoriaAtividade categoria)
            : base(id, titulo, descricao, arquivoOuLink, categoria)
        {
        }

        public Prova(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            int categoriaAtividadeId,
            CategoriaAtividade categoria)
            : base(id, titulo, descricao, arquivoOuLink, categoriaAtividadeId, categoria)
        {
        }

        public override string Tipo => "Prova";
    }
}
