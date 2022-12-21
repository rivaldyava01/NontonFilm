namespace Zeta.NontonFilm.Infrastructure.UserProfile;

public class UserProfileOptions
{
    public const string SectionKey = nameof(UserProfile);

    public string Provider { get; set; } = UserProfileProvider.None;
}

public static class UserProfileProvider
{
    public const string None = nameof(None);
    public const string ZetaGarde = nameof(ZetaGarde);
    public const string IS4IM = nameof(IS4IM);
}
