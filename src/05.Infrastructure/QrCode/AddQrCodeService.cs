using QRCoder;
using Zeta.NontonFilm.Application.Services.QrCode;

namespace Zeta.NontonFilm.Infrastructure.QrCode;

public class AddQrCodeService : IQrCodeService
{
    private const string Issuer = "NontonFilm";

    public byte[] GetGraphic(string otpUrl)
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(otpUrl, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);

        return qrCode.GetGraphic(5);
    }
}
