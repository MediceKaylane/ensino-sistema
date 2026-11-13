using sistema_ensino.data;
using sistema_ensino.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace sistema_ensino.Repository
{
    public class MateriaRepository
    {
        private readonly AppDbContext _contexto;

        public MateriaRepository(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Busca uma matéria pelo ID, incluindo suas atividades.
        public Materia? BuscarMateriaPorId(int id)
        {
            return _contexto.Materias
                .Include(materia => materia.Atividades)
                .FirstOrDefault(materia => materia.Id == id);
        }

        // Busca uma atividade pelo ID.
        public Atividades? BuscarAtividadePorId(int id)
        {
            return _contexto.Atividades.Find(id);
        }

        // Salva as alterações no banco.
        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}