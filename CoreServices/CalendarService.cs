using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace CoreServices
{
    public class CalendarService : ICalendarService
    {
        private readonly ISyncService _syncService;
        private readonly IRepository _repository;

        public CalendarService(ISyncService syncService, IRepository repository)
        {
            _syncService = syncService;
            _repository = repository;
        }

        public async Task<IList<EventModel>> GetEventModels(Expression<Func<EventModel, bool>> expression)
        {
            return await _repository.FindAll<EventModel>(expression);
        }

        public void Sync(string loginHint, DateTime startDate, DateTime endDate)
        {
            _syncService.Sync(loginHint, startDate, endDate);
        }

    }
}
