using Coswalt.Models;

namespace Coswalt.Services;

public interface IAirfoilService
{
    Task<IEnumerable<string>> GetAvailableAirfoilsAsync();
    Task<Airfoil> LoadAirfoilAsync(string name);
}