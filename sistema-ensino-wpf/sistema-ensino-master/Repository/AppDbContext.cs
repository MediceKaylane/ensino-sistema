
using Microsoft.EntityFrameworkCore;
using sistema_ensino.model;

namespace sistema_ensino.data
{
    public class AppDbContext : DbContext
    {
        // Construtor sem parâmetros para facilitar a criação do contexto.
        public AppDbContext()
        {
        }

        // Construtor que permite receber configurações externas.
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Configuração da conexão com o SQL Server.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SistemaEnsinoDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"
                );
            }
        }

        // Tabelas do banco de dados.
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Aluno> Alunos => Set<Aluno>();
        public DbSet<Instrutor> Instrutores => Set<Instrutor>();
        public DbSet<Turma> Turmas => Set<Turma>();
        public DbSet<Materia> Materias => Set<Materia>();
        public DbSet<Atividades> Atividades => Set<Atividades>();
        public DbSet<Avaliacoes> Avaliacoes => Set<Avaliacoes>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //relacoes entre as entidades

            modelBuilder.Entity<Usuario>()
                 .ToTable("Usuario")
                 .HasKey(u => u.Matricula);

            modelBuilder.Entity<Aluno>()
                .HasOne(aluno => aluno.Turma)
                .WithMany(turma => turma.Alunos);

            modelBuilder.Entity<Instrutor>()
                .HasOne(instrutor => instrutor.Turma)
                .WithMany(turma => turma.Instrutores);

            modelBuilder.Entity<Turma>()
                 .HasMany(turma => turma.Materias)
                 .WithMany();

            modelBuilder.Entity<Instrutor>()
                .HasMany(instrutor => instrutor.Atividades)
                .WithOne(atividade => atividade.Instrutor)
                .HasForeignKey(atividade => atividade.InstrutorMatricula)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Materia>()
                .HasMany(materia => materia.Atividades)
                .WithOne(atividade => atividade.Materia)
                .HasForeignKey(atividade => atividade.MateriaId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Avaliacoes>()
                .Property(avaliacao => avaliacao.Nota)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            modelBuilder.Entity<Avaliacoes>()
                .HasOne(avaliacao => avaliacao.Aluno)
                .WithMany(aluno => aluno.Avaliacoes)
                .HasForeignKey(avaliacao => avaliacao.AlunoMatricula)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Avaliacoes>()
                .HasOne(avaliacao => avaliacao.Atividade)
                .WithMany(atividade => atividade.Avaliacoes)
                .HasForeignKey(avaliacao => avaliacao.AtividadeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
