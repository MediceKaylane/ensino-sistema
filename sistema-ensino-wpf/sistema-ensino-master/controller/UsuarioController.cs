
using System;
using System.Linq;
using sistema_ensino.model;
using sistema_ensino.data;

namespace sistema_ensino.controller
{
    public class UsuarioController
    {
        private readonly AppDbContext _contexto;

        public UsuarioController(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Consulta um usuário pela matrícula no banco de dados.
        public Usuario? BuscarPorMatricula(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
            {
                return null;
            }

            return _contexto.Usuarios.Find(matricula);
        }

        // Verifica se o usuário ainda não concluiu o primeiro acesso.
        public bool VerificarPrimeiroAcesso(string matricula)
        {
            Usuario? usuario = BuscarPorMatricula(matricula);

            return usuario is not null && !usuario.PrimeiroAcessoConcluido;
        }

        // Salva as informações do primeiro acesso no banco de dados.
        public bool SalvarPrimeiroAcesso(string matricula, string nome, string email)
        {
            Usuario? usuario = BuscarPorMatricula(matricula);

            if (usuario is null || usuario.PrimeiroAcessoConcluido)
            {
                return false;
            }

            usuario.ConcluirPrimeiroAcesso(nome, email);

            _contexto.SaveChanges();

            return true;
        }
    }
}
