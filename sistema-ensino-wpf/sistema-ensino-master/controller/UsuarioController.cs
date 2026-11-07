using System.Security.Cryptography;
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

        public bool DefinirSenha(string matricula, string nome, string email, string senha)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || senha.Length < 8)
            {
                return false;
            }

            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = GerarHash(senha, salt);
            string registro = $"pbkdf2-sha256:{Convert.ToHexString(salt)}:{Convert.ToHexString(hash)}";
            return _usuarioRepository.AtivarAcesso(matricula, nome, email, registro);
        }

        public Usuario? Autenticar(string matricula, string senha)
        {
            Usuario? usuario = BuscarPorMatricula(matricula);
            if (usuario is null || !usuario.PrimeiroAcessoConcluido || !ConferirSenha(senha, usuario.SenhaHash))
            {
                return null;
            }

            _usuarioRepository.CarregarDadosDoPortal(usuario);
            return usuario;
        }

        private static bool ConferirSenha(string senha, string registro)
        {
            string[] partes = registro.Split(':');
            if (partes.Length != 3 || partes[0] != "pbkdf2-sha256" ||
                partes[1].Length != 32 || partes[2].Length != 64)
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromHexString(partes[1]);
                byte[] hash = Convert.FromHexString(partes[2]);
                return CryptographicOperations.FixedTimeEquals(GerarHash(senha, salt), hash);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static byte[] GerarHash(string senha, byte[] salt)
        {
            return Rfc2898DeriveBytes.Pbkdf2(senha, salt, 600_000, HashAlgorithmName.SHA256, 32);
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
