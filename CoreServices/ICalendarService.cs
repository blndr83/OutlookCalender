using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CoreServices
{
    public interface ICalendarService
    {
        void Sync(string loginHint, DateTime startDate, DateTime endDate);

        List<EventModel> GetEventModels(Expression<Func<EventModel, bool>> expression);
    }
}
