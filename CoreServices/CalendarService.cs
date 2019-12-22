using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreServices
{
    public class CalendarService : ICalendarService
    {
        private readonly ISyncService _syncService;
        private readonly IRepository _repository;

        public Action SyncDone { get; set; }

        public CalendarService(ISyncService syncService, IRepository repository)
        {
            _syncService = syncService;
            _repository = repository;
            _syncService.SyncDone = OnSyncDone;
        }

        private void OnSyncDone()
        {
            SyncDone?.Invoke();
        }

        public Task<List<EventModel>> GetEventModels(Expression<Func<EventModel, bool>> expression)
        {
            return  _repository.FindAll<EventModel>(expression);
        }

        public void Sync(string loginHint, DateTime startDate, DateTime endDate)
        {
            _syncService.Sync(loginHint, startDate, endDate);
        }

    }
}
