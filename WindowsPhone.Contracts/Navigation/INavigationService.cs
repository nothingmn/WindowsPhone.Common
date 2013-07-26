using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace WindowsPhone.Contracts.Navigation
{
    public interface INavigationService
    {
        void SetCurrentService(System.Windows.Navigation.NavigationService service);
        Uri Source { get; set; }
        Uri CurrentSource { get; }
        bool CanGoForward { get; }
        bool CanGoBack { get; }
        IEnumerable<JournalEntry> BackStack { get; }
        event NavigationFailedEventHandler NavigationFailed;
        event NavigatingCancelEventHandler Navigating;
        event NavigatedEventHandler Navigated;
        event NavigationStoppedEventHandler NavigationStopped;
        event FragmentNavigationEventHandler FragmentNavigation;
        event EventHandler<JournalEntryRemovedEventArgs> JournalEntryRemoved;
        bool Navigate(Uri source);
        void GoForward();
        void GoBack();
        void StopLoading();
        JournalEntry RemoveBackEntry();


    }
}
