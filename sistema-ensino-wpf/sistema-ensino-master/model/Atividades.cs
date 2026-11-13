using System;
using System.Collections.Generic;

namespace sistema_ensino.model
{
    public class Atividades : IPublicar
    {
        public int Id { get; private set; }
        public string Titulo { get; private set; } = string.Empty;
        public string Descricao { get; private set; } = string.Empty;
        public string ArquivoOuLink { get; private set; } = string.Empty;
        public string? InstrutorMatricula { get; private set; }
        public Instrutor? Instrutor { get; private set; }
        public int? MateriaId { get; private set; }
        public Materia? Materia { get; private set; }
        public List<Avaliacoes> Avaliacoes { get; private set; } = new();

        private Atividades()
        {
        }

        public Atividades(string titulo, string descricao, string arquivoOuLink)
        {
            Titulo = titulo;
            Descricao = descricao;
            ArquivoOuLink = arquivoOuLink;
        }

        public Atividades(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            Instrutor? instrutor,
            Materia? materia)
            : this(titulo, descricao, arquivoOuLink)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O identificador da atividade deve ser positivo.", nameof(id));
            }

            Id = id;
            if (instrutor is not null)
            {
                VincularInstrutor(instrutor);
            }

            if (materia is not null)
            {
                VincularMateria(materia);
            }
        }

        public void Publicar()
        {
            if (string.IsNullOrWhiteSpace(Titulo))
            {
                throw new InvalidOperationException("O título da atividade é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(Descricao))
            {
                throw new InvalidOperationException("A descrição da atividade é obrigatória.");
            }

            if (string.IsNullOrWhiteSpace(ArquivoOuLink))
            {
                throw new InvalidOperationException("O arquivo ou link da atividade é obrigatório.");
            }

            if (Instrutor is null || string.IsNullOrWhiteSpace(InstrutorMatricula))
            {
                throw new InvalidOperationException("A atividade deve estar vinculada a um instrutor.");
            }

            if (Materia is null || MateriaId is null or <= 0)
            {
                throw new InvalidOperationException("A atividade deve estar vinculada a uma matéria.");
            }
        }

        public string Visualizar()
        {
            return $"Título: {Titulo}\n" +
                   $"Descrição: {Descricao}\n" +
                   $"Arquivo ou link: {ArquivoOuLink}";
        }

        internal void VincularInstrutor(Instrutor instrutor)
        {
            Instrutor = instrutor ?? throw new ArgumentNullException(nameof(instrutor));
            InstrutorMatricula = instrutor.Matricula;
        }

        internal void VincularMateria(Materia materia)
        {
            Materia = materia ?? throw new ArgumentNullException(nameof(materia));
            MateriaId = materia.Id;
        }

        internal void DesvincularMateria()
        {
            Materia = null;
            MateriaId = null;
        }
    }
}
