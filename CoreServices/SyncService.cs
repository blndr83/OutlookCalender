using Microsoft.Graph;
using Microsoft.Identity.Client;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreServices
{
    public class SyncService : ISyncService
    {
        private readonly IRepository _repository;
        private GraphServiceClient _client;
        private readonly IClientService _clientService;

        public Action SyncDone { get; set; }

        public SyncService(IRepository repository, IClientService clientService)
        {
            _repository = repository;
            _clientService = clientService;
        }

        private async Task<IUserCalendarViewCollectionPage> GetEvents(string loginHint, DateTime startDate, DateTime endDate)
        {
            if (_client == null)
                _client = await _clientService.GraphServiceClient(loginHint);
            var queryOptions = new List<QueryOption>
            {
                new QueryOption("StartDateTime",startDate.ToUniversalTime().ToString("o")),
                new QueryOption("EndDateTime",endDate.ToUniversalTime().ToString("o"))
            };
            return await _client.Me.CalendarView.Request(queryOptions).GetAsync();
        }

        private async Task<List<EventModel>> GetEventsAsync(string loginHint, DateTime startDate, DateTime endDate)
        {
            var calendarView = await GetEvents(loginHint, startDate, endDate);
            var events = new List<EventModel>();
            if (calendarView != null)
            {

                calendarView.ToList().ForEach(c =>
                {

                    events.Add(new EventModel
                    {
                        Id = Guid.NewGuid(),
                        BodyContent = c.Body.Content,
                        Subject = c.Subject,
                        Start = DateTime.Parse(c.Start.DateTime),
                        End = DateTime.Parse(c.End.DateTime)
                    });
                });
                var nextStartDate = startDate.AddDays(7);
                if (nextStartDate < endDate)
                {
                    var nextEvents = await GetEventsAsync(loginHint, nextStartDate, endDate);
                    if (nextEvents.Any())
                    {
                        events.AddRange(nextEvents);
                    }
                }
            }
            return events.OrderBy(e => e.Start).ToList();
        }

        public async void Sync(string loginHint, DateTime startDate, DateTime endDate)
        {
            try
            {
                var calendarEvents = await GetEventsAsync(loginHint, startDate, endDate);
                if (calendarEvents.Any())
                {
                    calendarEvents.ForEach(_ =>
                    {
                        var eventModel = _repository.Find<EventModel>((e) => e.Start.Equals(_.Start)
                        && e.End.Equals(_.End) && e.Subject.Equals(_.Subject));
                        if (eventModel == null) _repository.Save(_);
                        else if (!_.BodyContent.Equals(eventModel.BodyContent))
                        {
                            eventModel.Update(_);
                            _repository.Update(eventModel);
                        }
                    });
                }
                SyncDone?.Invoke();
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is MsalClientException)
            {
                SyncDone?.Invoke();
            }

        }
    }
}
