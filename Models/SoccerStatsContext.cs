using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_FutbolStats.Models;

public partial class SoccerStatsContext : DbContext
{
    public SoccerStatsContext()
    {
    }

    public SoccerStatsContext(DbContextOptions<SoccerStatsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClasificacionTemporadum> ClasificacionTemporada { get; set; }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<Gole> Goles { get; set; }

    public virtual DbSet<HistorialRefreshToken> HistorialRefreshTokens { get; set; }

    public virtual DbSet<Jugador> Jugadors { get; set; }

    public virtual DbSet<Partido> Partidos { get; set; }

    public virtual DbSet<PartidosJugado> PartidosJugados { get; set; }

    public virtual DbSet<PosicionJugador> PosicionJugadors { get; set; }

    public virtual DbSet<ResultadoPartido> ResultadoPartidos { get; set; }

    public virtual DbSet<Tarjetum> Tarjeta { get; set; }

    public virtual DbSet<Temporadum> Temporada { get; set; }

    public virtual DbSet<TipoPartido> TipoPartidos { get; set; }

    public virtual DbSet<TipoTarjetum> TipoTarjeta { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=JAIR-PC; Initial Catalog=SoccerStats; Integrated Security=True; Connect Timeout=30; Encrypt=False; TrustServerCertificate=False; ApplicationIntent=ReadWrite; MultisubnetFailOver=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClasificacionTemporadum>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Clasificacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("clasificacion");
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.ToTable("Equipo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Escudo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("escudo");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Lugar)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lugar");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Equipos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Equipo_Usuario");
        });

        modelBuilder.Entity<Gole>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Goles).HasColumnName("goles");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdJugador).HasColumnName("id_jugador");
            entity.Property(e => e.IdPartido).HasColumnName("id_partido");
            entity.Property(e => e.IdTemporada).HasColumnName("id_temporada");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.Goles)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("FK_Goles_Equipo");

            entity.HasOne(d => d.IdJugadorNavigation).WithMany(p => p.Goles)
                .HasForeignKey(d => d.IdJugador)
                .HasConstraintName("FK_Goles_Jugador");

            entity.HasOne(d => d.IdPartidoNavigation).WithMany(p => p.Goles)
                .HasForeignKey(d => d.IdPartido)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Goles_Partido");

            entity.HasOne(d => d.IdTemporadaNavigation).WithMany(p => p.Goles)
                .HasForeignKey(d => d.IdTemporada)
                .HasConstraintName("FK_Goles_Temporada");
        });

        modelBuilder.Entity<HistorialRefreshToken>(entity =>
        {
            entity.HasKey(e => e.IdHistorialToken).HasName("PK__Historia__9BD4D67292CC672E");

            entity.ToTable("HistorialRefreshToken");

            entity.Property(e => e.IdHistorialToken).HasColumnName("id_historial_token");
            entity.Property(e => e.EsActivo)
                .HasComputedColumnSql("(case when [fecha_creacion]<getdate() then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", false)
                .HasColumnName("es_activo");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaExpiracion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_expiracion");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("refresh_token");
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("token");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistorialRefreshTokens)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Historial__id_us__5441852A");
        });

        modelBuilder.Entity<Jugador>(entity =>
        {
            entity.ToTable("Jugador");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Dorsal)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("dorsal");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdPosicion).HasColumnName("id_posicion");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.Jugadors)
                .HasForeignKey(d => d.IdEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Jugador_Equipo");

            entity.HasOne(d => d.IdPosicionNavigation).WithMany(p => p.Jugadors)
                .HasForeignKey(d => d.IdPosicion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Jugador_PosicionJugador");
        });

        modelBuilder.Entity<Partido>(entity =>
        {
            entity.ToTable("Partido");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.GolesContra).HasColumnName("goles_contra");
            entity.Property(e => e.GolesFavor).HasColumnName("goles_favor");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdResultado).HasColumnName("id_resultado");
            entity.Property(e => e.IdTemporada).HasColumnName("id_temporada");
            entity.Property(e => e.IdTipoPartido).HasColumnName("id_tipo_partido");
            entity.Property(e => e.NombreRival)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_rival");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.Partidos)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("FK_Partido_Equipo");

            entity.HasOne(d => d.IdResultadoNavigation).WithMany(p => p.Partidos)
                .HasForeignKey(d => d.IdResultado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Partido_ResultadoPartido");

            entity.HasOne(d => d.IdTemporadaNavigation).WithMany(p => p.Partidos)
                .HasForeignKey(d => d.IdTemporada)
                .HasConstraintName("FK_Partido_Temporada");

            entity.HasOne(d => d.IdTipoPartidoNavigation).WithMany(p => p.Partidos)
                .HasForeignKey(d => d.IdTipoPartido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Partido_TipoPartido");
        });

        modelBuilder.Entity<PartidosJugado>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdJugador).HasColumnName("id_jugador");
            entity.Property(e => e.IdPartido).HasColumnName("id_partido");
            entity.Property(e => e.IdTemporada).HasColumnName("id_temporada");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.PartidosJugados)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("FK_PartidosJugados_Equipo");

            entity.HasOne(d => d.IdJugadorNavigation).WithMany(p => p.PartidosJugados)
                .HasForeignKey(d => d.IdJugador)
                .HasConstraintName("FK_PartidosJugados_Jugador");

            entity.HasOne(d => d.IdPartidoNavigation).WithMany(p => p.PartidosJugados)
                .HasForeignKey(d => d.IdPartido)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PartidosJugados_Partido");

            entity.HasOne(d => d.IdTemporadaNavigation).WithMany(p => p.PartidosJugados)
                .HasForeignKey(d => d.IdTemporada)
                .HasConstraintName("FK_PartidosJugados_Temporada");
        });

        modelBuilder.Entity<PosicionJugador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PosicionesJugador");

            entity.ToTable("PosicionJugador");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Posicion)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("posicion");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
        });

        modelBuilder.Entity<ResultadoPartido>(entity =>
        {
            entity.ToTable("ResultadoPartido");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Resultado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("resultado");
        });

        modelBuilder.Entity<Tarjetum>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdJugador).HasColumnName("id_jugador");
            entity.Property(e => e.IdPartido).HasColumnName("id_partido");
            entity.Property(e => e.IdTemporada).HasColumnName("id_temporada");
            entity.Property(e => e.IdTipoTarjeta).HasColumnName("id_tipo_tarjeta");
            entity.Property(e => e.Tarjetas).HasColumnName("tarjetas");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.Tarjeta)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("FK_Tarjeta_Equipo");

            entity.HasOne(d => d.IdJugadorNavigation).WithMany(p => p.Tarjeta)
                .HasForeignKey(d => d.IdJugador)
                .HasConstraintName("FK_Tarjeta_Jugador");

            entity.HasOne(d => d.IdPartidoNavigation).WithMany(p => p.Tarjeta)
                .HasForeignKey(d => d.IdPartido)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Tarjeta_Partido");

            entity.HasOne(d => d.IdTemporadaNavigation).WithMany(p => p.Tarjeta)
                .HasForeignKey(d => d.IdTemporada)
                .HasConstraintName("FK_Tarjeta_Temporada");

            entity.HasOne(d => d.IdTipoTarjetaNavigation).WithMany(p => p.TarjetaNavigation)
                .HasForeignKey(d => d.IdTipoTarjeta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tarjeta_TipoTarjeta");
        });

        modelBuilder.Entity<Temporadum>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaFinal).HasColumnName("fecha_final");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.IdClasificacion).HasColumnName("id_clasificacion");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.NoTemporada).HasColumnName("no_temporada");
            entity.Property(e => e.NombreTemporada)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_temporada");
            entity.Property(e => e.Posicion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("posicion");

            entity.HasOne(d => d.IdClasificacionNavigation).WithMany(p => p.Temporada)
                .HasForeignKey(d => d.IdClasificacion)
                .HasConstraintName("FK_Temporada_ClasificacionTemporada");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.Temporada)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("FK_Temporada_Equipo");
        });

        modelBuilder.Entity<TipoPartido>(entity =>
        {
            entity.ToTable("TipoPartido");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.TipoPartido1)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("tipo_partido");
        });

        modelBuilder.Entity<TipoTarjetum>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Tarjeta)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tarjeta");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clave)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
