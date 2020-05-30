using System;
using System.Threading.Tasks;

namespace CoreServices
{
    public interface ISyncService
    {
        Task Sync(string loginHint, DateTime startDate, DateTime endDate);
        Action AfterLogin { set; }
    }
}
