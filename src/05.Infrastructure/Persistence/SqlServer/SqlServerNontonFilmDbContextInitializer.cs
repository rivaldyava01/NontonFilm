using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zeta.NontonFilm.Base.Enums;
using Zeta.NontonFilm.Domain.Entities;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer;

public class SqlServerNontonFilmDbContextInitializer
{
    private readonly ILogger<SqlServerNontonFilmDbContextInitializer> _logger;
    private readonly SqlServerNontonFilmDbContext _context;

    public SqlServerNontonFilmDbContextInitializer(ILogger<SqlServerNontonFilmDbContextInitializer> logger, SqlServerNontonFilmDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            var isMigrationNeeded = (await _context.Database.GetPendingMigrationsAsync()).Any();

            if (isMigrationNeeded)
            {
                _logger.LogInformation("Applying database migration...");

                await _context.Database.MigrateAsync();
            }
            else
            {
                _logger.LogInformation("Database is up to date. No database migration required.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");

            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");

            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        #region Seeding data Genres
        var anyGenreExists = await _context.Genres.AnyAsync();

        if (!anyGenreExists)
        {
            _logger.LogInformation("Seeding data Genres...");
            await _context.Genres.AddAsync(new Genre { Id = Guid.NewGuid(), Name = "Drama" });
            await _context.Genres.AddAsync(new Genre { Id = Guid.NewGuid(), Name = "Action" });
            await _context.Genres.AddAsync(new Genre { Id = Guid.NewGuid(), Name = "Comedy" });
            await _context.Genres.AddAsync(new Genre { Id = Guid.NewGuid(), Name = "Horror" });
            await _context.Genres.AddAsync(new Genre { Id = Guid.NewGuid(), Name = "Romance" });
            await _context.Genres.AddAsync(new Genre { Id = Guid.NewGuid(), Name = "Fantasy" });
            await _context.Genres.AddAsync(new Genre { Id = Guid.NewGuid(), Name = "Thiller" });
            await _context.Genres.AddAsync(new Genre { Id = Guid.NewGuid(), Name = "Musikal" });
            await _context.Genres.AddAsync(new Genre { Id = Guid.NewGuid(), Name = "Sci-Fi" });
            await _context.Genres.AddAsync(new Genre { Id = Guid.NewGuid(), Name = "Biografi" });
            await _context.Genres.AddAsync(new Genre { Id = Guid.NewGuid(), Name = "Animasi" });
        }

        #endregion Seeding data Genres

        #region Seeding data Cities
        var anyCityExists = await _context.Cities.AnyAsync();

        if (!anyCityExists)
        {
            _logger.LogInformation("Seeding data Cities...");
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Jakarta" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Bekasi" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Bandung" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Cirebon" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Depok" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Bogor" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Tangerang" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Denpasar" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Aceh" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Jayapura" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Medan" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Pontianak" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Manado" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Ambon" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Kupang" });
            await _context.Cities.AddAsync(new City { Id = Guid.NewGuid(), Name = "Lombok" });
        }

        #endregion Seeding data Cities

        #region Seeding data CinemaChains
        var anyCinemaChainExists = await _context.CinemaChains.AnyAsync();

        if (!anyCinemaChainExists)
        {
            _logger.LogInformation("Seeding data CinemaChains...");
            await _context.CinemaChains.AddAsync(new CinemaChain
            {
                Id = Guid.NewGuid(),
                Name = "XXI",
                OfficeAddress = "Jl. KH Wahid Hasyim No.96A",
                EmailAddress = "xxi@hq.com",
                PhoneNumber = "021-8929199229929"
            });

            await _context.CinemaChains.AddAsync(new CinemaChain
            {
                Id = Guid.NewGuid(),
                Name = "CGV",
                OfficeAddress = "AIA Central 26th floor Jl. Jend. Sudirman Kav. 48A",
                EmailAddress = "callcenter@cgv.id",
                PhoneNumber = "021-29200100"
            });

            await _context.CinemaChains.AddAsync(new CinemaChain
            {
                Id = Guid.NewGuid(),
                Name = "FLIX",
                OfficeAddress = "Jl. Jend. Sudirman Kav. 52-53",
                EmailAddress = "callcenter@flix.id",
                PhoneNumber = "021-29200100"
            });

            await _context.CinemaChains.AddAsync(new CinemaChain
            {
                Id = Guid.NewGuid(),
                Name = "Cinepolis",
                OfficeAddress = "Jl. Jend. Sudirman Kav. 52-53",
                EmailAddress = "bd@cinepolis.co.id",
                PhoneNumber = "021-8392989299"
            });
        }

        await _context.SaveChangesAsync(this);

        #endregion Seeding data CinemaChains

        #region Seeding data Movies
        var anyMovieExists = await _context.Movies.AnyAsync();

        if (!anyMovieExists)
        {
            _logger.LogInformation("Seeding data Movies...");

            var blackPanther = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Black Panther",
                Rating = RatingTypes.SU,
                Duration = 100,
                ReleaseDate = DateTime.Now,
                Synopsis = "The people of Wakanda fight to protect their home from intervening world powers as they mourn the death of King T'Challa.",
                PosterImage = "https://m.media-amazon.com/images/M/MV5BNTM4NjIxNmEtYWE5NS00NDczLTkyNWQtYThhNmQyZGQzMjM0XkEyXkFqcGdeQXVyODk4OTc3MTY@._V1_FMjpg_UX1000_.jpg"
            };

            var blackPantherGenres = _context.Genres.Where(x => x.Name.Contains("Action")).SingleOrDefault();

            if (blackPantherGenres is not null)
            {
                blackPanther.MovieGenres.Add(new MovieGenre
                {
                    Id = Guid.NewGuid(),
                    MovieId = blackPanther.Id,
                    GenreId = blackPantherGenres.Id
                });
            }

            await _context.Movies.AddAsync(blackPanther);

            var qorin = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Qorin",
                Rating = RatingTypes.D,
                Duration = 109,
                ReleaseDate = DateTime.Now,
                Synopsis = "Zahra, a third year student at Rodiatul Jannah boarding school, has always been a model student who has many achievements in school.",
                PosterImage = "https://media.21cineplex.com/webcontent/gallery/pictures/166849056869806_405x594.jpg"
            };

            var qorinGenres = _context.Genres.Where(x => x.Name.Contains("Horror") || x.Name.Contains("Drama")).ToList();

            if (qorinGenres is not null)
            {
                foreach (var qoringenre in qorinGenres)
                {
                    qorin.MovieGenres.Add(new MovieGenre
                    {
                        Id = Guid.NewGuid(),
                        MovieId = qorin.Id,
                        GenreId = qoringenre.Id
                    });
                }
            }

            await _context.Movies.AddAsync(qorin);

            var decibel = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Decibel",
                Rating = RatingTypes.R,
                Duration = 110,
                ReleaseDate = DateTime.Now,
                Synopsis = "The story unfolds when a bomb that responds to sound is discovered at the center of the city.",
                PosterImage = "https://asset.tix.id/wp-content/uploads/2022/11/aed58393fd2645df88c9ac6e882ffc31.jpg"
            };

            var decibelGenres = _context.Genres.Where(x => x.Name.Contains("Thiller") || x.Name.Contains("Drama")).ToList();

            if (decibelGenres is not null)
            {
                foreach (var decibelgenre in decibelGenres)
                {
                    decibel.MovieGenres.Add(new MovieGenre
                    {
                        Id = Guid.NewGuid(),
                        MovieId = decibel.Id,
                        GenreId = decibelgenre.Id
                    });
                }
            }

            await _context.Movies.AddAsync(decibel);
        }

        await _context.SaveChangesAsync(this);
        #endregion Seeding data Movies

        #region Seeding data Cinemas
        var anyCinemaExists = await _context.Cinemas.AnyAsync();
        var jakarta = _context.Cities.Where(x => x.Name.Contains("Jakarta")).SingleOrDefault();
        var bekasi = _context.Cities.Where(x => x.Name.Contains("Bekasi")).SingleOrDefault();

        var xxi = _context.CinemaChains.Where(x => x.Name.Contains("XXI")).SingleOrDefault();
        var cgv = _context.CinemaChains.Where(x => x.Name.Contains("CGV")).SingleOrDefault();
        var flix = _context.CinemaChains.Where(x => x.Name.Contains("FLIX")).SingleOrDefault();
        var cinepolis = _context.CinemaChains.Where(x => x.Name.Contains("Cinepolis")).SingleOrDefault();

        if (!anyCinemaExists)
        {
            _logger.LogInformation("Seeding data Cinemas...");

            if (jakarta is not null && bekasi is not null)
            {
                if (xxi is not null && cgv is not null && flix is not null && cinepolis is not null)
                {
                    await _context.Cinemas.AddAsync(new Cinema
                    {
                        Id = Guid.NewGuid(),
                        CinemaChainId = xxi.Id,
                        CityId = jakarta.Id,
                        Name = "XXI Kramat Jati",
                        Address = "Lantai 3 Lippo Plaza Jalan Raya Bogor",
                        EmailAddress = "xxi@hq.com",
                        PhoneNumber = "021-80877457"
                    });

                    await _context.Cinemas.AddAsync(new Cinema
                    {
                        Id = Guid.NewGuid(),
                        CinemaChainId = cgv.Id,
                        CityId = jakarta.Id,
                        Name = "CGV Slipi Jaya",
                        Address = "Plaza Slipi Jaya Lantai 4 Jl. Letjend S Parman no. Kav 17-18 ",
                        EmailAddress = "callcenter@cgv.id",
                        PhoneNumber = "021-5346474"
                    });

                    await _context.Cinemas.AddAsync(new Cinema
                    {
                        Id = Guid.NewGuid(),
                        CinemaChainId = flix.Id,
                        CityId = bekasi.Id,
                        Name = "FLIX Grand Galaxy Park",
                        Address = "Grand Galaxy Park Mall Lantai 2, Jl. Boulevard Raya Timur,",
                        EmailAddress = "callcenter@flix.id",
                        PhoneNumber = "021-29200100"
                    });

                    await _context.Cinemas.AddAsync(new Cinema
                    {
                        Id = Guid.NewGuid(),
                        CinemaChainId = cinepolis.Id,
                        CityId = bekasi.Id,
                        Name = "Cinepolis Blu Plaza ",
                        Address = "Mall Blu Plaza Bekasi, Jl. Chairil Anwar",
                        EmailAddress = "bd@cinepolis.co.id",
                        PhoneNumber = "081394937646"
                    });
                }
            }
        }

        await _context.SaveChangesAsync(this);

        #endregion Seeding data Cinemas
    }
}

