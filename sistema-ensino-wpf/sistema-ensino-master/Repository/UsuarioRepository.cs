using Microsoft.EntityFrameworkCore;
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

        public bool AtivarAcesso(string matricula, string nome, string email, string senhaHash)
        {
            return _contexto.Usuarios
                .Where(usuario => usuario.Matricula == matricula && usuario.SenhaHash == "")
                .ExecuteUpdate(atualizar => atualizar
                    .SetProperty(usuario => usuario.Nome, nome)
                    .SetProperty(usuario => usuario.Email, email)
                    .SetProperty(usuario => usuario.SenhaHash, senhaHash)
                    .SetProperty(usuario => usuario.PrimeiroAcessoConcluido, true)) == 1;
        }

        public void CarregarDadosDoPortal(Usuario usuario)
        {
            if (usuario is Instrutor instrutor)
            {
                _contexto.Entry(instrutor).Reference(item => item.Turma).Load();
                _contexto.Entry(instrutor).Collection(item => item.Atividades).Query()
                    .Include(item => item.Categoria).Load();
            }

            if (usuario is Aluno aluno)
            {
                _contexto.Entry(aluno).Reference(item => item.Turma).Load();
                if (aluno.Turma is Turma turma)
                {
                    _contexto.Entry(turma).Collection(item => item.Materias).Load();
                    _contexto.Entry(turma).Collection(item => item.Instrutores).Query()
                        .Include(item => item.Atividades).ThenInclude(item => item.Categoria).Load();
                }
            }
        }

        // Salva as alterações do usuário no banco.
        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
