using System;

namespace sistema_ensino.model
{
    public abstract class AtividadeBase : IAtividade
    {
        public int Id { get; protected set; }
        public string Titulo { get; protected set; } = string.Empty;
        public string Descricao { get; protected set; } = string.Empty;
        public string ArquivoOuLink { get; protected set; } = string.Empty;

        public int CategoriaAtividadeId { get; protected set; }
        public CategoriaAtividade Categoria { get; protected set; } = null!;

        protected AtividadeBase()
        {
        }

        protected AtividadeBase(
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
            CategoriaAtividadeId = categoria.Id;
        }

        protected AtividadeBase(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            int categoriaAtividadeId,
            CategoriaAtividade categoria)
            : this(id, titulo, descricao, arquivoOuLink, categoria)
        {
            if (categoriaAtividadeId <= 0)
                throw new ArgumentException("O identificador da categoria é inválido.");

            CategoriaAtividadeId = categoriaAtividadeId;
        }

        public abstract string Tipo { get; }

        public virtual string Visualizar()
        {
            return $"Tipo: {Tipo}\n" +
                   $"Título: {Titulo}\n" +
                   $"Descrição: {Descricao}\n" +
                   $"Arquivo ou link: {ArquivoOuLink}\n" +
                   $"Categoria: {Categoria.Nome}";
        }
    }
}
