using sistema_ensino.model;

namespace sistema_ensino.model
{
    public class Atividade : IAtividade
    {
        public int Id { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public string ArquivoOuLink { get; private set; }
        public CategoriaAtividade Categoria { get; private set; }

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
        }

        public string Visualizar()
        {
            return $"Título: {Titulo}\n" +
                   $"Descrição: {Descricao}\n" +
                   $"Arquivo ou link: {ArquivoOuLink}\n" +
                   $"Categoria: {Categoria}";
        }
    }
}


//SRP — Responsabilidade Única: CategoriaAtividade fica responsável por representar as categorias.
//OCP — Aberto/Fechado: a interface IAtividade estabelece um contrato que pode ser implementado por outras classes.
//DIP — Inversão de Dependência: outras partes do sistema podem depender da abstração IAtividade, em vez de depender diretamente da classe concreta Atividade.