using System;
using System.Collections.Generic;
using sistema_ensino.model;
using sistema_ensino.Repository;

namespace sistema_ensino.controller
{
    public class AlunoController
    {
        private readonly AlunoRepository _alunoRepository;

        public AlunoController(AlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository
                ?? throw new ArgumentNullException(nameof(alunoRepository));
        }

        // Busca um aluno pela matrícula.
        public Aluno? BuscarAlunoPorMatricula(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
            {
                return null;
            }

            return _alunoRepository.BuscarAlunoPorMatricula(matricula);
        }

        // Consulta as atividades disponibilizadas pelos instrutores da turma.
        public List<Atividades> VisualizarAtividades(string matricula)
        {
            Aluno? aluno = BuscarAlunoPorMatricula(matricula);

            if (aluno is null)
            {
                return new List<Atividades>();
            }

            return aluno.VisualizarAtividades();
        }

        public List<Avaliacoes> VisualizarAvaliacoes(Aluno alunoAutenticado)
        {
            if (alunoAutenticado is null)
            {
                throw new ArgumentNullException(nameof(alunoAutenticado));
            }

            return _alunoRepository.BuscarAvaliacoesDoAluno(alunoAutenticado);
        }

        // Verifica se o aluno existe antes de enviar uma resposta.
        public string EnviarResposta(string matricula)
        {
            Aluno? aluno = BuscarAlunoPorMatricula(matricula);

            if (aluno is null)
            {
                return "Aluno não encontrado.";
            }

            return aluno.EnviarResposta();
        }
    }
}