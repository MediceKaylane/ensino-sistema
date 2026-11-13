using System;
using System.Collections.Generic;
using System.Linq;
using sistema_ensino.model;
using sistema_ensino.Repository;

namespace sistema_ensino.controller
{
    // SRP: coordena as operações relacionadas às matérias.
    public class MateriaController
    {
        private readonly MateriaRepository _materiaRepository;

        // DIP: recebe o repositório pelo construtor.
        public MateriaController(MateriaRepository materiaRepository)
        {
            _materiaRepository = materiaRepository
                ?? throw new ArgumentNullException(nameof(materiaRepository));
        }

        // Busca uma matéria pelo ID.
        public Materia? BuscarMateriaPorId(int id)
        {
            return _materiaRepository.BuscarMateriaPorId(id);
        }

        // Adiciona uma atividade existente a uma matéria.
        public void AdicionarAtividade(int idMateria, Atividades atividade)
        {
            if (atividade is null)
                throw new ArgumentNullException(nameof(atividade));

            Materia? materia = BuscarMateriaPorId(idMateria);

            if (materia is null)
                throw new InvalidOperationException("Matéria não encontrada.");

            Atividades? atividadeBanco =
                _materiaRepository.BuscarAtividadePorId(atividade.Id);

            if (atividadeBanco is null)
                throw new InvalidOperationException("Atividade não encontrada.");

            materia.AdicionarAtividade(atividadeBanco);

            _materiaRepository.Salvar();
        }

        // Remove uma atividade de uma matéria.
        public void RemoverAtividade(int idMateria, Atividades atividade)
        {
            if (atividade is null)
                throw new ArgumentNullException(nameof(atividade));

            Materia? materia = BuscarMateriaPorId(idMateria);

            if (materia is null)
                throw new InvalidOperationException("Matéria não encontrada.");

            Atividades? atividadeAssociada = materia.Atividades
                .FirstOrDefault(a => a.Id == atividade.Id);

            if (atividadeAssociada is null)
                throw new InvalidOperationException(
                    "Atividade não associada à matéria.");

            materia.RemoverAtividade(atividadeAssociada);

            _materiaRepository.Salvar();
        }

        // Consulta as atividades de uma matéria.
        public List<Atividades> ConsultarAtividades(int idMateria)
        {
            Materia? materia = BuscarMateriaPorId(idMateria);

            if (materia is null)
                return new List<Atividades>();

            return materia.Atividades;
        }
    }
}