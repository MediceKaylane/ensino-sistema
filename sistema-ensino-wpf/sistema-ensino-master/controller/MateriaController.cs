
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using sistema_ensino.model;
using sistema_ensino.data;

namespace sistema_ensino.controller
{
    // SRP: coordena as operações relacionadas às matérias.
    public class MateriaController
    {
        // DIP: recebe o contexto do banco pelo construtor.
        private readonly AppDbContext _contexto;

        public MateriaController(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Busca uma matéria pelo ID no banco de dados.
        public Materia? BuscarMateriaPorId(int id)
        {
            return _contexto.Materias
                .Include(materia => materia.Atividades)
                .FirstOrDefault(materia => materia.Id == id);
        }

        // Adiciona uma atividade existente a uma matéria.
        public void AdicionarAtividade(int idMateria, Atividade atividade)
        {
            if (atividade is null)
                throw new ArgumentNullException(nameof(atividade));

            Materia? materia = BuscarMateriaPorId(idMateria);

            if (materia is null)
                throw new InvalidOperationException("Matéria não encontrada.");

            Atividade? atividadeBanco = _contexto.Atividades
                .Find(atividade.Id);

            if (atividadeBanco is null)
                throw new InvalidOperationException("Atividade não encontrada.");

            materia.AdicionarAtividade(atividadeBanco);

            _contexto.SaveChanges();
        }

        // Remove uma atividade de uma matéria.
        public void RemoverAtividade(int idMateria, Atividade atividade)
        {
            if (atividade is null)
                throw new ArgumentNullException(nameof(atividade));

            Materia? materia = BuscarMateriaPorId(idMateria);

            if (materia is null)
                throw new InvalidOperationException("Matéria não encontrada.");

            Atividade? atividadeAssociada = materia.Atividades
                .FirstOrDefault(a => a.Id == atividade.Id);

            if (atividadeAssociada is null)
                throw new InvalidOperationException(
                    "Atividade não associada à matéria.");

            materia.RemoverAtividade(atividadeAssociada);

            _contexto.SaveChanges();
        }

        // Consulta as atividades de uma matéria.
        public List<Atividade> ConsultarAtividades(int idMateria)
        {
            Materia? materia = BuscarMateriaPorId(idMateria);

            if (materia is null)
                return new List<Atividade>();

            return materia.Atividades;
        }
    }
}
