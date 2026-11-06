using sistema_ensino.data;
using sistema_ensino.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace sistema_ensino.Repository
{
    public class InstrutorRepository
    {
        private readonly AppDbContext _contexto;

        public InstrutorRepository(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Busca um instrutor pela matrícula, incluindo suas atividades.
        public Instrutor? BuscarInstrutorPorMatricula(string matricula)
        {
            return _contexto.Instrutores
                .Include(instrutor => instrutor.Atividades)
                .FirstOrDefault(instrutor => instrutor.Matricula == matricula);
        }

        // Salva as alterações no banco.
        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}