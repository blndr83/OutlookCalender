﻿using Autofac;
using OutlookCalender.ViewModels;

namespace OutlookCalender.Locator
{
    public class ViewModelLocator
    {
        private readonly IContainer _container;

        public static ViewModelLocator Instance { get; private set; }

        public static void CreateInstance(IContainer container)
        {
            Instance = new ViewModelLocator(container);
        }

        public  SearchResult SearchResult { get; set; }

        private ViewModelLocator(IContainer container)
        {
            _container = container;
        }

        public  T GetViewModel<T>() where T : ViewModelBase
        {
           return _container.Resolve<T>();
        }
    }
}
