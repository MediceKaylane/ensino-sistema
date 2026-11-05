
using System;
using System.Collections.Generic;
using System.Linq;
using sistema_ensino.model;
using sistema_ensino.data;

namespace sistema_ensino.controller
{
    // SRP: coordena as operações relacionadas às atividades.
    public class AtividadeController
    {
        // DIP: recebe o contexto do banco pelo construtor.
        private readonly AppDbContext _contexto;

        public AtividadeController(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Busca uma atividade pelo ID no banco de dados.
        public Atividade? BuscarAtividadePorId(int id)
        {
            return _contexto.Atividades
                .FirstOrDefault(atividade => atividade.Id == id);
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

        // Consulta todas as atividades cadastradas no banco.
        public List<Atividade> ConsultarAtividades()
        {
            return _contexto.Atividades.ToList();
        }
    }
}
