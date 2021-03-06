﻿using Microsoft.Graph;
using Microsoft.Identity.Client;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common;

namespace CoreServices
{
    public class SyncService : ISyncService
    {
        private readonly IRepository _repository;
        private GraphServiceClient _client;
        private readonly IClientService _clientService;

        public Action AfterLogin { set; private get; }

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
                new QueryOption("EndDateTime",endDate.ToUniversalTime().ToString("o")),
                new QueryOption("$top", "10000")
            };
            AfterLogin?.Invoke();
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
                        Id = c.Id,
                        BodyContent = c.Body.Content,
                        Subject = c.Subject,
                        Start = DateTime.Parse(c.Start.DateTime),
                        End = DateTime.Parse(c.End.DateTime),
                        LocationDisplayName = c.Location.DisplayName,
                        BodyContentWithoutHtml = c.Body.Content?.RemoveHtmlTags()
                    });
                });

            }
            return events.OrderBy(e => e.Start).ToList();
        }

        public async Task Sync(string loginHint, DateTime startDate, DateTime endDate)
        {
            try
            {
                var addCount = 0;
                var updateCount = 0;
                var calendarEvents = await GetEventsAsync(loginHint, startDate, endDate);
                if (calendarEvents.Any())
                {
                    calendarEvents.ForEach(_ =>
                    {
                        var eventModel = _repository.Find<EventModel>((e) => e.Id.Equals(_.Id));
                        if (eventModel == null) 
                        {
                            _repository.Save(_);
                            addCount++;
                        }
                        else if (!_.BodyContent.Equals(eventModel.BodyContent))
                        {
                            eventModel.Update(_);
                            _repository.Update(eventModel);
                            updateCount++;
                        }
                    });
                }
                _repository.Save(new SyncLog() { AmountOfAddedItems = addCount, AmountOfUpdatedItems = updateCount, StartDate = startDate, EndDate = endDate, SyncDate = DateTime.Today, Id = Guid.NewGuid()});
               
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is MsalClientException)
            {
                throw;
            }

        }
    }
}
