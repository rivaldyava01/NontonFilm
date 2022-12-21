using System.Globalization;

namespace Zeta.NontonFilm.Shared.Common.Extensions;

public static class DecimalExtensions
{
    public static string ToCurrency0DisplayText(this decimal money)
    {
        return string.Format(new CultureInfo("id-ID", false), "{0:C0}", money);
    }
}
