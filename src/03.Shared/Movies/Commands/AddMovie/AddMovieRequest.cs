using FluentValidation;
using Zeta.NontonFilm.Base.Enums;
using Zeta.NontonFilm.Shared.Common.Attributes;
using Zeta.NontonFilm.Shared.Movies.Constants;

namespace Zeta.NontonFilm.Shared.Movies.Commands.AddMovie;

public class AddMovieRequest
{
    public string Title { get; set; } = default!;
    public RatingTypes Rating { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; } = default!;
    public string PosterImage { get; set; } = default!;

    [SpecialValueAttribute(SpecialValueType.Json)]
    public List<Guid> MovieGenreIds { get; set; } = new();

}

public class AddMovieRequestValidator : AbstractValidator<AddMovieRequest>
{
    public AddMovieRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.Title)
            .MaximumLength(MaximumLengthFor.Title);

        RuleFor(x => x.Rating)
           .NotEmpty();

        RuleFor(x => x.Duration)
           .NotEmpty();

        RuleFor(x => x.Duration)
            .NotEmpty();

        RuleFor(x => x.Synopsis)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.Synopsis);

        RuleFor(x => x.PosterImage)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.PosterImage);

        RuleFor(x => x.MovieGenreIds.Count)
         .GreaterThanOrEqualTo(MinimumValueFor.MovieGenresCount);
    }
}
