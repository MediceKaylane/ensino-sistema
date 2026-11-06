
namespace sistema_ensino.model
{
    public class Atividade : IAtividade
    {
        public int Id { get; private set; }
        public string Titulo { get; private set; } = string.Empty;
        public string Descricao { get; private set; } = string.Empty;
        public string ArquivoOuLink { get; private set; } = string.Empty;

        public int CategoriaAtividadeId { get; private set; }
        public CategoriaAtividade Categoria { get; private set; } = null!;

        // Construtor utilizado pelo Entity Framework.
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
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("O título é obrigatório.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("A descrição é obrigatória.");

            if (string.IsNullOrWhiteSpace(arquivoOuLink))
                throw new ArgumentException("O arquivo ou link é obrigatório.");

            Categoria = categoria
                ?? throw new ArgumentNullException(nameof(categoria));

            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            ArquivoOuLink = arquivoOuLink;
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
