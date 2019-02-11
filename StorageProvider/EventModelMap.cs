using Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace StorageProvider
{
    internal class EventModelMap : ClassMapping<EventModel>
    {
        public EventModelMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.GuidComb));
            Property(x => x.Start);
            Property(x => x.End);
            Property(x => x.Subject);
            Property(x => x.BodyContent);
        }
    }
}
