using System;
using System.Collections.Generic;
using sistema_ensino.model;
using sistema_ensino.Repository;

namespace sistema_ensino.controller
{
    // SRP: coordena as operações relacionadas aos instrutores.
    public class InstrutorController
    {
        private readonly InstrutorRepository _instrutorRepository;

        // DIP: recebe o repositório pelo construtor.
        public InstrutorController(InstrutorRepository instrutorRepository)
        {
            _instrutorRepository = instrutorRepository
                ?? throw new ArgumentNullException(nameof(instrutorRepository));
        }

        // Busca um instrutor pela matrícula.
        public Instrutor? BuscarInstrutorPorMatricula(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
            {
                return null;
            }

            return _instrutorRepository.BuscarInstrutorPorMatricula(matricula);
        }

        // Publica uma atividade.
        public void PublicarAtividade(string matricula, Atividade atividade)
        {
            if (atividade is null)
            {
                throw new ArgumentNullException(nameof(atividade));
            }

            Instrutor? instrutor = BuscarInstrutorPorMatricula(matricula);

            if (instrutor is null)
            {
                throw new InvalidOperationException("Instrutor não encontrado.");
            }

            instrutor.PublicarAtividade(atividade);

            _instrutorRepository.Salvar();
        }

        // Consulta as atividades cadastradas pelo instrutor.
        public List<Atividade> ConsultarAtividades(string matricula)
        {
            Instrutor? instrutor = BuscarInstrutorPorMatricula(matricula);

            if (instrutor is null)
            {
                return new List<Atividade>();
            }

            return instrutor.Atividades;
        }
    }
}