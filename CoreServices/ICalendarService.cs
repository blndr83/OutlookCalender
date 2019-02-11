using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreServices
{
    public interface ICalendarService
    {
        void Sync(string loginHint, DateTime startDate, DateTime endDate);

        Task<IList<EventModel>> GetEventModels(Expression<Func<EventModel, bool>> expression);
    }
}
