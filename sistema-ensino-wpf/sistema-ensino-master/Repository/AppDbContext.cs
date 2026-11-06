
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
        public DbSet<Atividade> Atividades => Set<Atividade>();

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
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Materia>()
                .HasMany(materia => materia.Atividades)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
