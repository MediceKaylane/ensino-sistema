
using sistema_ensino.data;
using sistema_ensino.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sistema_ensino.Repository
{
    public class AtividadeRepository
    {
        private readonly AppDbContext _contexto;

        public AtividadeRepository(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Busca uma atividade pelo ID.
        public Atividade? BuscarAtividadePorId(int id)
        {
            return _contexto.Atividades
                .FirstOrDefault(atividade => atividade.Id == id);
        }

        // Consulta todas as atividades cadastradas.
        public List<Atividade> ConsultarAtividades()
        {
            return _contexto.Atividades.ToList();
        }

        // Consulta atividades por categoria utilizando SQL direto.
        public List<Atividade> ConsultarAtividadesPorCategoria(int categoriaId)
        {
            return _contexto.Atividades
                .FromSqlInterpolated(
                    $"SELECT * FROM Atividades WHERE CategoriaAtividadeId = {categoriaId}"
                )
                .Include(atividade => atividade.Categoria)
                .ToList();
        }
    }
}
