using MudBlazor;

namespace Zeta.NontonFilm.Bsui.Layouts.Constants;

public class ThemeFor
{
    public static readonly MudTheme MainLayout = new()
    {
        Palette = new Palette()
        {
            Primary = Colors.Teal.Darken2,
            Secondary = Colors.Pink.Lighten2,
            AppbarBackground = Colors.Grey.Darken4
        }
    };
}
