
using System;

namespace sistema_ensino.model
{
    public class CategoriaAtividade
    {
        public int Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;

        private CategoriaAtividade()
        {
        }

        public CategoriaAtividade(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome da categoria é obrigatório.");
            }

            Nome = nome;
        }

        // Construtor utilizado para criar categorias a partir do SQL direto.
        public CategoriaAtividade(int id, string nome)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID da categoria é inválido.");
            }

            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome da categoria é obrigatório.");
            }

            Id = id;
            Nome = nome;
        }
    }
}
