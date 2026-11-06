using sistema_ensino.data;
using sistema_ensino.model;
using sistema_ensino.Repository;
using System;

namespace sistema_ensino.Repository
{
    public class UsuarioRepository
    {
        private readonly AppDbContext _contexto;

        public UsuarioRepository(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Consulta um usuário pela matrícula.
        public Usuario? BuscarPorMatricula(string matricula)
        {
            return _contexto.Usuarios.Find(matricula);
        }

        // Salva as alterações do usuário no banco.
        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}