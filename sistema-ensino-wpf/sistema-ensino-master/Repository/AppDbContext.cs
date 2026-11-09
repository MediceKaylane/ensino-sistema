
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
        public DbSet<AtividadeBase> Atividades => Set<AtividadeBase>();
        public DbSet<CategoriaAtividade> CategoriasAtividade => Set<CategoriaAtividade>();

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


            modelBuilder.Entity<CategoriaAtividade>()
                .HasKey(categoria => categoria.Id);

            modelBuilder.Entity<CategoriaAtividade>()
                .Property(categoria => categoria.Nome)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<CategoriaAtividade>()
                .HasData(
                    new { Id = 1, Nome = "Prova" },
                    new { Id = 2, Nome = "Trabalho" },
                    new { Id = 3, Nome = "Exercicio" },
                    new { Id = 4, Nome = "Seminario" },
                    new { Id = 5, Nome = "Projeto" }
                );

            modelBuilder.Entity<AtividadeBase>()
                .HasDiscriminator<string>("TipoAtividade")
                .HasValue<Prova>("Prova")
                .HasValue<Trabalho>("Trabalho")
                .HasValue<Seminario>("Seminario")
                .HasValue<Exercicio>("Exercicio")
                .HasValue<Projeto>("Projeto");

            modelBuilder.Entity<AtividadeBase>()
                .HasOne(atividade => atividade.Categoria)
                .WithMany()
                .HasForeignKey(atividade => atividade.CategoriaAtividadeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
