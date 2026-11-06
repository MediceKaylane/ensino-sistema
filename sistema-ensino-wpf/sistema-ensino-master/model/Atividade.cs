namespace sistema_ensino.model
{
    public class Atividade : IAtividade
    {
        public int Id { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public string ArquivoOuLink { get; private set; }

        public int CategoriaAtividadeId { get; private set; }
        public CategoriaAtividade Categoria { get; private set; }

        private Atividade()
        {
        }

        public Atividade(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            CategoriaAtividade categoria)
        {
            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            ArquivoOuLink = arquivoOuLink;
            Categoria = categoria;
            CategoriaAtividadeId = categoria.Id;
        }

        public string Visualizar()
        {
            return $"Título: {Titulo}\n" +
                   $"Descrição: {Descricao}\n" +
                   $"Arquivo ou link: {ArquivoOuLink}\n" +
                   $"Categoria: {Categoria.Nome}";
        }
    }
}