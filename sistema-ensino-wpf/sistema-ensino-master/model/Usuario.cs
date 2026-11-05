namespace sistema_ensino.model
{
    public abstract class Usuario
    {
        public string Matricula { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public bool PrimeiroAcessoConcluido { get; private set; }

        protected Usuario(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
            {
                throw new ArgumentException("A matrícula é obrigatória.");
            }

            Matricula = matricula;
            Nome = string.Empty;
            Email = string.Empty;
        }

        public void ConcluirPrimeiroAcesso(string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("O e-mail é obrigatório.");
            }

            Nome = nome;
            Email = email;
            PrimeiroAcessoConcluido = true;
        }

        public string ConsultarPerfil()
        {
            return $"Matrícula: {Matricula}\nNome: {Nome}\nE-mail: {Email}";
        }

        public string SenhaHash { get; private set; } = string.Empty;

    }

}