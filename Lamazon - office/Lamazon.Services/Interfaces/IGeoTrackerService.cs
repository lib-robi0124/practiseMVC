using Lamazon.Services.Models;

namespace Lamazon.Services.Interfaces
{
    public interface IGeoTrackerService
    {
        string GetCountryFlagUrl(string countryCode);
        Task<IpGeoInfo> GetIpGeoInfoAsync(string ipAddress);
    }
}
