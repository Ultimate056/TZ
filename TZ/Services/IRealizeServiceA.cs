using TZ.Models;

namespace TZ.Services
{
    public interface IRealizeServiceA
    { 
        string GetStatus(Unit unit);

        void UpdateAllStatus(object? state);
    }
}
