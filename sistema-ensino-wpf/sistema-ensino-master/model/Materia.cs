using System.Collections.Generic;
using System;


namespace sistema_ensino.model
{
    public class Materia
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Tipo { get; private set; }
        public List<Atividades> Atividades { get; private set; } = new();

        public Materia(int id, string nome, string tipo)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O identificador da matéria deve ser positivo.");
            }

            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome da matéria é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(tipo))
            {
                throw new ArgumentException("O tipo da matéria é obrigatório.");
            }

            Id = id;
            Nome = nome;
            Tipo = tipo;
        }

        public void AdicionarAtividade(Atividades atividade)
        {
            if (atividade is null)
            {
                throw new ArgumentNullException(nameof(atividade));
            }

            atividade.VincularMateria(this);
            if (!Atividades.Contains(atividade))
            {
                Atividades.Add(atividade);
            }
        }

        public void RemoverAtividade(Atividades atividade)
        {
            if (atividade is null)
            {
                throw new ArgumentNullException(nameof(atividade));
            }

            if (Atividades.Remove(atividade))
            {
                atividade.DesvincularMateria();
            }
        }
    }
}