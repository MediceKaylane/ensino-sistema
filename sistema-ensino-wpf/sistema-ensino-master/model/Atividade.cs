namespace sistema_ensino.model
{
    public class Atividade
    {
        public int Id { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public string ArquivoOuLink { get; private set; }

        public Atividade(int id, string titulo, string descricao, string arquivoOuLink)
        {
            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            ArquivoOuLink = arquivoOuLink;
        }

        public string Visualizar()
        {
            return $"Título: {Titulo}\nDescrição: {Descricao}\nArquivo ou link: {ArquivoOuLink}";
        }
    }
}