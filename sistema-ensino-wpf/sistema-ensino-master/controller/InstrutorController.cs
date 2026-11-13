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

        public void PublicarAtividade(string matricula, Atividades atividade, int materiaId)
        {
            if (string.IsNullOrWhiteSpace(matricula))
            {
                throw new ArgumentException("A matrícula do instrutor é obrigatória.", nameof(matricula));
            }

            if (atividade is null)
            {
                throw new ArgumentNullException(nameof(atividade));
            }

            _instrutorRepository.PublicarAtividade(matricula, atividade, materiaId);
        }

        public void PublicarAvaliacao(
            string instrutorMatricula,
            string alunoMatricula,
            int atividadeId,
            decimal nota)
        {
            if (string.IsNullOrWhiteSpace(instrutorMatricula))
            {
                throw new ArgumentException("A matrícula do instrutor é obrigatória.", nameof(instrutorMatricula));
            }

            if (string.IsNullOrWhiteSpace(alunoMatricula))
            {
                throw new ArgumentException("A matrícula do aluno é obrigatória.", nameof(alunoMatricula));
            }

            if (atividadeId <= 0)
            {
                throw new ArgumentException("O ID da atividade deve ser maior que zero.", nameof(atividadeId));
            }

            _instrutorRepository.PublicarAvaliacao(instrutorMatricula, alunoMatricula, atividadeId, nota);
        }

        public List<Atividades> ConsultarAtividades(string matricula)
        {
            Instrutor? instrutor = BuscarInstrutorPorMatricula(matricula);

            if (instrutor is null)
            {
                return new List<Atividades>();
            }

            return instrutor.Atividades;
        }
    }
}