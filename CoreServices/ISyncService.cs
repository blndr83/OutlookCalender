using System;

namespace CoreServices
{
    public interface ISyncService
    {
        void Sync(string loginHint, DateTime startDate, DateTime endDate);

    }
}
