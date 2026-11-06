namespace sistema_ensino.model
{
    public class CategoriaAtividade
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }

        private CategoriaAtividade()
        {
        }

        public CategoriaAtividade(string nome)
        {
            Nome = nome;
        }
    }
}