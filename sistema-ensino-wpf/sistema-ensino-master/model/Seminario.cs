namespace sistema_ensino.model
{
    public class Seminario : AtividadeBase
    {
        private Seminario()
        {
        }

        public Seminario(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            CategoriaAtividade categoria)
            : base(id, titulo, descricao, arquivoOuLink, categoria)
        {
        }

        public Seminario(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            int categoriaAtividadeId,
            CategoriaAtividade categoria)
            : base(id, titulo, descricao, arquivoOuLink, categoriaAtividadeId, categoria)
        {
        }

        public override string Tipo => "Seminário";
    }
}
