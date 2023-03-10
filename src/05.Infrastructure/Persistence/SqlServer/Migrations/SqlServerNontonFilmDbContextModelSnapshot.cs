// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Zeta.NontonFilm.Infrastructure.Persistence.SqlServer;

#nullable disable

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Migrations
{
    [DbContext(typeof(SqlServerNontonFilmDbContext))]
    partial class SqlServerNontonFilmDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Audit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ActionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ClientApplicationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FromIpAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("NewValues")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Audits", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Cinema", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CinemaChainId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("CinemaChainId");

                    b.HasIndex("CityId");

                    b.ToTable("Cinemas", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.CinemaChain", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OfficeAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("CinemaChains", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.City", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Cities", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Genres", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PosterImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Synopsis")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Movies", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.MovieGenre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("GenreId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieGenres", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Seat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Column")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Row")
                        .IsRequired()
                        .HasColumnType("nvarchar(5)");

                    b.Property<Guid>("StudioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StudioId");

                    b.ToTable("Seats", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Show", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ShowDateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("StudioId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("TicketPrice")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("StudioId");

                    b.ToTable("Shows", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Studio", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CinemaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CinemaId");

                    b.ToTable("Studios", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CinemaName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("MovieName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeatCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SeatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ShowDateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ShowId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StudioName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TicketPrice")
                        .HasColumnType("money");

                    b.Property<Guid?>("TicketSalesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SeatId");

                    b.HasIndex("ShowId");

                    b.HasIndex("TicketSalesId");

                    b.ToTable("Tickets", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.TicketSales", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("SalesDateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("TicketSales", "NontonFilm");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Audit", b =>
                {
                    b.OwnsOne("Zeta.NontonFilm.Base.ValueObjects.Geolocation", "FromGeolocation", b1 =>
                        {
                            b1.Property<Guid>("AuditId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Accuracy")
                                .HasColumnType("float")
                                .HasColumnName("Accuracy");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float")
                                .HasColumnName("Latitude");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float")
                                .HasColumnName("Longitude");

                            b1.HasKey("AuditId");

                            b1.ToTable("Audits", "NontonFilm");

                            b1.WithOwner()
                                .HasForeignKey("AuditId");
                        });

                    b.Navigation("FromGeolocation");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Cinema", b =>
                {
                    b.HasOne("Zeta.NontonFilm.Domain.Entities.CinemaChain", "CinemaChain")
                        .WithMany("Cinemas")
                        .HasForeignKey("CinemaChainId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Zeta.NontonFilm.Domain.Entities.City", "City")
                        .WithMany("Cinemas")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CinemaChain");

                    b.Navigation("City");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.MovieGenre", b =>
                {
                    b.HasOne("Zeta.NontonFilm.Domain.Entities.Genre", "Genre")
                        .WithMany("MovieGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Zeta.NontonFilm.Domain.Entities.Movie", "Movie")
                        .WithMany("MovieGenres")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Seat", b =>
                {
                    b.HasOne("Zeta.NontonFilm.Domain.Entities.Studio", "Studio")
                        .WithMany("Seats")
                        .HasForeignKey("StudioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Studio");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Show", b =>
                {
                    b.HasOne("Zeta.NontonFilm.Domain.Entities.Movie", "Movie")
                        .WithMany("Shows")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Zeta.NontonFilm.Domain.Entities.Studio", "Studio")
                        .WithMany("Shows")
                        .HasForeignKey("StudioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Studio");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Studio", b =>
                {
                    b.HasOne("Zeta.NontonFilm.Domain.Entities.Cinema", "Cinema")
                        .WithMany("Studios")
                        .HasForeignKey("CinemaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cinema");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Ticket", b =>
                {
                    b.HasOne("Zeta.NontonFilm.Domain.Entities.Seat", "Seat")
                        .WithMany("Tickets")
                        .HasForeignKey("SeatId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Zeta.NontonFilm.Domain.Entities.Show", "Show")
                        .WithMany("Tickets")
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Zeta.NontonFilm.Domain.Entities.TicketSales", "TicketSales")
                        .WithMany("Tickets")
                        .HasForeignKey("TicketSalesId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Seat");

                    b.Navigation("Show");

                    b.Navigation("TicketSales");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Cinema", b =>
                {
                    b.Navigation("Studios");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.CinemaChain", b =>
                {
                    b.Navigation("Cinemas");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.City", b =>
                {
                    b.Navigation("Cinemas");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Genre", b =>
                {
                    b.Navigation("MovieGenres");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Movie", b =>
                {
                    b.Navigation("MovieGenres");

                    b.Navigation("Shows");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Seat", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Show", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.Studio", b =>
                {
                    b.Navigation("Seats");

                    b.Navigation("Shows");
                });

            modelBuilder.Entity("Zeta.NontonFilm.Domain.Entities.TicketSales", b =>
                {
                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
