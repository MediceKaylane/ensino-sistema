namespace sistema_ensino.model
{
    public interface IAtividade
    {
        int Id { get; }
        string Titulo { get; }
        string Descricao { get; }
        string ArquivoOuLink { get; }
        CategoriaAtividade Categoria { get; }

        string Visualizar();
    }
}