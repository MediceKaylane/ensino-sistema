using System;
using System.Collections.Generic;
using sistema_ensino.model;
using sistema_ensino.Repository;

namespace sistema_ensino.controller
{
    public class AtividadeController
    {
        private readonly AtividadeRepository _atividadeRepository;

        public AtividadeController(AtividadeRepository atividadeRepository)
        {
            _atividadeRepository = atividadeRepository
                ?? throw new ArgumentNullException(nameof(atividadeRepository));
        }

        public Atividades? BuscarAtividadePorId(int id)
        {
            return _atividadeRepository.BuscarAtividadePorId(id);
        }

        public string VisualizarAtividade(int id)
        {
            Atividades atividade = BuscarAtividadePorId(id)
                ?? throw new InvalidOperationException("Atividade não encontrada.");

            return atividade.Visualizar();
        }

        public List<Atividades> ConsultarAtividades()
        {
            return _atividadeRepository.ConsultarAtividades();
        }

        public List<Atividades> ConsultarAtividadesPorMateria(int materiaId)
        {
            if (materiaId <= 0)
            {
                throw new ArgumentException("O ID da matéria deve ser maior que zero.");
            }

            return _atividadeRepository.ConsultarAtividadesPorMateria(materiaId);
        }

        public List<Atividades> ConsultarAtividadesPorMateriaSemEF(int materiaId)
        {
            if (materiaId <= 0)
            {
                throw new ArgumentException("O ID da matéria deve ser maior que zero.");
            }

            return _atividadeRepository.ConsultarAtividadesPorMateriaSemEF(materiaId);
        }
    }
}
