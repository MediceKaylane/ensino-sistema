
using System;
using System.Collections.Generic;
using sistema_ensino.model;
using sistema_ensino.Repository;

namespace sistema_ensino.controller
{
    // SRP: coordena as operações relacionadas às atividades.
    public class AtividadeController
    {
        private readonly AtividadeRepository _atividadeRepository;

        // DIP: recebe o repositório pelo construtor.
        public AtividadeController(AtividadeRepository atividadeRepository)
        {
            _atividadeRepository = atividadeRepository
                ?? throw new ArgumentNullException(nameof(atividadeRepository));
        }

        // Busca uma atividade pelo ID.
        public Atividade? BuscarAtividadePorId(int id)
        {
            return _atividadeRepository.BuscarAtividadePorId(id);
        }

        // Consulta os detalhes de uma atividade.
        public string VisualizarAtividade(int id)
        {
            Atividade? atividade = BuscarAtividadePorId(id);

            if (atividade is null)
            {
                throw new InvalidOperationException("Atividade não encontrada.");
            }

            return atividade.Visualizar();
        }

        // Consulta todas as atividades cadastradas.
        public List<Atividade> ConsultarAtividades()
        {
            return _atividadeRepository.ConsultarAtividades();
        }

        // Consulta atividades por categoria utilizando Entity Framework.
        public List<Atividade> ConsultarAtividadesPorCategoria(int categoriaId)
        {
            if (categoriaId <= 0)
            {
                throw new ArgumentException("O ID da categoria deve ser maior que zero.");
            }

            return _atividadeRepository.ConsultarAtividadesPorCategoria(categoriaId);
        }

        // Consulta atividades por categoria sem utilizar Entity Framework.
        public List<Atividade> ConsultarAtividadesPorCategoriaSemEF(int categoriaId)
        {
            if (categoriaId <= 0)
            {
                throw new ArgumentException("O ID da categoria deve ser maior que zero.");
            }

            return _atividadeRepository.ConsultarAtividadesPorCategoriaSemEF(categoriaId);
        }
    }
}
