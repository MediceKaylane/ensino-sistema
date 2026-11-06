using System;
using sistema_ensino.model;
using sistema_ensino.Repository;

namespace sistema_ensino.controller
{
    public class UsuarioController
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository
                ?? throw new ArgumentNullException(nameof(usuarioRepository));
        }

        // Consulta um usuário pela matrícula.
        public Usuario? BuscarPorMatricula(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
            {
                return null;
            }

            return _usuarioRepository.BuscarPorMatricula(matricula);
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

            _usuarioRepository.Salvar();

            return true;
        }
    }
}