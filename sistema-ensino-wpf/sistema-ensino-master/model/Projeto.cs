namespace sistema_ensino.model
{
    public class Projeto : AtividadeBase
    {
        private Projeto()
        {
        }

        public Projeto(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            CategoriaAtividade categoria)
            : base(id, titulo, descricao, arquivoOuLink, categoria)
        {
        }

        public Projeto(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            int categoriaAtividadeId,
            CategoriaAtividade categoria)
            : base(id, titulo, descricao, arquivoOuLink, categoriaAtividadeId, categoria)
        {
        }

        public override string Tipo => "Projeto";
    }
}
