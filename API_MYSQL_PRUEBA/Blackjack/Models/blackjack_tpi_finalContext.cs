using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Blackjack.Models
{
    public partial class blackjack_tpi_finalContext : DbContext
    {
        public blackjack_tpi_finalContext()
        {
        }

        public blackjack_tpi_finalContext(DbContextOptions<blackjack_tpi_finalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CartaJugadum> CartaJugada { get; set; }
        public virtual DbSet<Cartum> Carta { get; set; }
        public virtual DbSet<Jugadum> Jugada { get; set; }
        public virtual DbSet<Partidum> Partida { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseMySql("server=localhost;uid=root;pwd=Federico123;database=blackjack_tpi_final", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<CartaJugadum>(entity =>
            {
                entity.HasKey(e => e.IdCartaJugada)
                    .HasName("PRIMARY");

                entity.ToTable("carta_jugada");

                entity.HasIndex(e => e.IdCarta, "fk_cartaJug_carta");

                entity.HasIndex(e => e.IdPartida, "fk_cartaJug_partida");

                entity.Property(e => e.IdCartaJugada).HasColumnName("id_carta_jugada");

                entity.Property(e => e.IdCarta).HasColumnName("id_carta");

                entity.Property(e => e.IdPartida).HasColumnName("id_partida");

                entity.Property(e => e.Jugador)
                    .HasMaxLength(100)
                    .HasColumnName("jugador");

                entity.HasOne(d => d.IdCartaNavigation)
                    .WithMany(p => p.CartaJugada)
                    .HasForeignKey(d => d.IdCarta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cartaJug_carta");

                entity.HasOne(d => d.IdPartidaNavigation)
                    .WithMany(p => p.CartaJugada)
                    .HasForeignKey(d => d.IdPartida)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cartaJug_partida");
            });

            modelBuilder.Entity<Cartum>(entity =>
            {
                entity.HasKey(e => e.IdCarta)
                    .HasName("PRIMARY");

                entity.ToTable("carta");

                entity.Property(e => e.IdCarta).HasColumnName("id_carta");

                entity.Property(e => e.Carta)
                    .HasMaxLength(200)
                    .HasColumnName("carta");

                entity.Property(e => e.Imagen)
                    .HasMaxLength(500)
                    .HasColumnName("imagen");

                entity.Property(e => e.IsAs).HasColumnName("isAs");

                entity.Property(e => e.Valor).HasColumnName("valor");
            });

            modelBuilder.Entity<Jugadum>(entity =>
            {
                entity.HasKey(e => e.IdJugada)
                    .HasName("PRIMARY");

                entity.ToTable("jugada");

                entity.HasIndex(e => e.IdUsuario, "fk_jugada_usuario");

                entity.Property(e => e.IdJugada).HasColumnName("id_jugada");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Jugada)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_jugada_usuario");
            });

            modelBuilder.Entity<Partidum>(entity =>
            {
                entity.HasKey(e => e.IdPartida)
                    .HasName("PRIMARY");

                entity.ToTable("partida");

                entity.HasIndex(e => e.IdJugada, "fk_jugada_partida");

                entity.Property(e => e.IdPartida).HasColumnName("id_partida");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.IdJugada).HasColumnName("id_jugada");

                entity.Property(e => e.PuntosCroupier).HasColumnName("puntos_croupier");

                entity.Property(e => e.PuntosJugador).HasColumnName("puntos_jugador");

                entity.Property(e => e.Resultado)
                    .HasMaxLength(200)
                    .HasColumnName("resultado");

                entity.HasOne(d => d.IdJugadaNavigation)
                    .WithMany(p => p.Partida)
                    .HasForeignKey(d => d.IdJugada)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_jugada_partida");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("clave");

                entity.Property(e => e.Usuario1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("usuario");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
