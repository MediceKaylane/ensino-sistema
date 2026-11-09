namespace sistema_ensino.model
{
    public class Trabalho : AtividadeBase
    {
        private Trabalho()
        {
        }

        public Trabalho(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            CategoriaAtividade categoria)
            : base(id, titulo, descricao, arquivoOuLink, categoria)
        {
        }

        public Trabalho(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            int categoriaAtividadeId,
            CategoriaAtividade categoria)
            : base(id, titulo, descricao, arquivoOuLink, categoriaAtividadeId, categoria)
        {
        }

        public override string Tipo => "Trabalho";
    }
}
