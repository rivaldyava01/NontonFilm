using FluentValidation;

namespace Zeta.NontonFilm.Shared.MovieGenres.Commands.UpdateMovieGenre;

public class UpdateMovieRequest_MovieGenre
{
    public Guid GenreId { get; set; }
}

public class UpdateMovieGenreRequest_MovieGenreValidator : AbstractValidator<UpdateMovieRequest_MovieGenre>
{
    public UpdateMovieGenreRequest_MovieGenreValidator()
    {
        RuleFor(x => x.GenreId)
            .NotEmpty();
    }
}
