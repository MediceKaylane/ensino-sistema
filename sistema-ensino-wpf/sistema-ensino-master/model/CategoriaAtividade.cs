
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
    }
}
