namespace Zeta.NontonFilm.Application.Services.QrCode;

public interface IQrCodeService
{
    byte[] GetGraphic(string qrCode);

}
