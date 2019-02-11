using Autofac;
using OutlookCalender.ViewModels;

namespace OutlookCalender.Locator
{
    public class ViewModelLocator
    {
        private IContainer _container;

        public ViewModelLocator(IContainer container)
        {
            _container = container;
        }

        public T GetViewModel<T>() where T : ViewModelBase
        {
           return _container.Resolve<T>();
        }
    }
}
