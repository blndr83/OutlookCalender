using OutlookCalender.ViewModels;
using System;
using System.Threading.Tasks;

namespace OutlookCalender
{
    public interface IUiService
    {
        Action<SearchResult> ShowSearchResult { get; }
        Func<string, Task<bool>> DisplayAlert { get; }
        Func<string, Task> ShowActivityPopup { get; }
        Func<Task> RemoveActivityPopup { get; }

    }
}
