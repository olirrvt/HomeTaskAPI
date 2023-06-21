using Microsoft.EntityFrameworkCore;

namespace HomeTaskerAPI
{
    public partial class HomeTaskerDbContext : DbContext
    {
        public HomeTaskerDbContext()
        {
        }

        public HomeTaskerDbContext(DbContextOptions<HomeTaskerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aviso> Avisos { get; set; }

        public virtual DbSet<Conta> Contas { get; set; }

        public virtual DbSet<Moradore> Moradores { get; set; }

        public virtual DbSet<Ocorrencia> Ocorrencias { get; set; }

        public virtual DbSet<Produto> Produtos { get; set; }

        public virtual DbSet<Reserva> Reservas { get; set; }

        public virtual DbSet<Servico> Servicos { get; set; }

        public virtual DbSet<Visitante> Visitantes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=HomeTaskerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aviso>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Avisos__3213E83F307E8B34");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DataPublicacao)
                    .HasColumnType("date")
                    .HasColumnName("dataPublicacao");
                entity.Property(e => e.Mensagem)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("mensagem");
                entity.Property(e => e.Titulo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("titulo");
            });

            modelBuilder.Entity<Conta>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Contas__3213E83F8F8156A7");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DataVencimento)
                    .HasColumnType("date")
                    .HasColumnName("dataVencimento");
                entity.Property(e => e.MoradorId).HasColumnName("moradorID");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Valor)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("valor");

                entity.HasOne(d => d.Morador).WithMany(p => p.Conta)
                    .HasForeignKey(d => d.MoradorId)
                    .HasConstraintName("FK__Contas__moradorI__398D8EEE");
            });

            modelBuilder.Entity<Moradore>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Moradore__3213E83F89661E3F");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Apartamento)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apartamento");
                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");
                entity.Property(e => e.IsAdministrador).HasColumnName("isAdministrador");
                entity.Property(e => e.Nome)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nome");
                entity.Property(e => e.Senha)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("senha");
            });

            modelBuilder.Entity<Ocorrencia>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Ocorrenc__3213E83F0470EC4C");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DataHoraRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dataHoraRegistro");
                entity.Property(e => e.Descricao)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("descricao");
                entity.Property(e => e.MoradorId).HasColumnName("moradorID");

                entity.HasOne(d => d.Morador).WithMany(p => p.Ocorrencia)
                    .HasForeignKey(d => d.MoradorId)
                    .HasConstraintName("FK__Ocorrenci__morad__46E78A0C");
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Produtos__3213E83FD7B3C3E0");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DescricaoProduto)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("descricaoProduto");
                entity.Property(e => e.MoradorId).HasColumnName("moradorID");
                entity.Property(e => e.NomeProduto)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nomeProduto");
                entity.Property(e => e.Preco)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("preco");

                entity.HasOne(d => d.Morador).WithMany(p => p.Produtos)
                    .HasForeignKey(d => d.MoradorId)
                    .HasConstraintName("FK__Produtos__morado__412EB0B6");
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Reservas__3213E83F90F9A397");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DataHoraReserva)
                    .HasColumnType("datetime")
                    .HasColumnName("dataHoraReserva");
                entity.Property(e => e.EspacoComum)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("espacoComum");
                entity.Property(e => e.MoradorId).HasColumnName("moradorID");

                entity.HasOne(d => d.Morador).WithMany(p => p.Reservas)
                    .HasForeignKey(d => d.MoradorId)
                    .HasConstraintName("FK__Reservas__morado__3C69FB99");
            });

            modelBuilder.Entity<Servico>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Servicos__3213E83F4703F818");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DataHoraSolicitacao)
                    .HasColumnType("datetime")
                    .HasColumnName("dataHoraSolicitacao");
                entity.Property(e => e.Descricao)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("descricao");
                entity.Property(e => e.MoradorId).HasColumnName("moradorID");
                entity.Property(e => e.StatusDoServico)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("statusDoServico");
                entity.Property(e => e.TipoServico)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipoServico");

                entity.HasOne(d => d.Morador).WithMany(p => p.Servicos)
                    .HasForeignKey(d => d.MoradorId)
                    .HasConstraintName("FK__Servicos__morado__440B1D61");
            });

            modelBuilder.Entity<Visitante>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Visitant__3213E83F7AED5933");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DataHoraEntrada)
                    .HasColumnType("datetime")
                    .HasColumnName("dataHoraEntrada");
                entity.Property(e => e.DataHoraSaida)
                    .HasColumnType("datetime")
                    .HasColumnName("dataHoraSaida");
                entity.Property(e => e.MoradorId).HasColumnName("moradorID");
                entity.Property(e => e.NomeVisitante)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nomeVisitante");

                entity.HasOne(d => d.Morador).WithMany(p => p.Visitantes)
                    .HasForeignKey(d => d.MoradorId)
                    .HasConstraintName("FK__Visitante__morad__49C3F6B7");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}