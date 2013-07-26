using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using WindowsPhone.Contracts.Navigation;

namespace WindowsPhone.Common.Navigation
{
    public class NavigationService : INavigationService
    {
        public void SetCurrentService(System.Windows.Navigation.NavigationService service)
        {
            Current = service;
        }

        private System.Windows.Navigation.NavigationService Current;

        public Uri Source
        {
            get { return Current.Source; }
            set { Current.Source = value; }
        }

        public Uri CurrentSource
        {
            get { return Current.CurrentSource; }
        }

        public bool CanGoForward
        {
            get { return Current.CanGoForward; }
        }

        public bool CanGoBack
        {
            get { return Current.CanGoBack; }
        }

        public IEnumerable<JournalEntry> BackStack
        {
            get { return Current.BackStack; }
        }

        public event NavigationFailedEventHandler NavigationFailed;
        public event NavigatingCancelEventHandler Navigating;
        public event NavigatedEventHandler Navigated;
        public event NavigationStoppedEventHandler NavigationStopped;
        public event FragmentNavigationEventHandler FragmentNavigation;
        public event EventHandler<JournalEntryRemovedEventArgs> JournalEntryRemoved;

        public bool Navigate(Uri source)
        {
            return Current.Navigate(source);
        }

        public void GoForward()
        {
            Current.GoForward();
        }

        public void GoBack()
        {
            Current.GoBack();
        }

        public void StopLoading()
        {
            Current.StopLoading();
        }

        public JournalEntry RemoveBackEntry()
        {
            return Current.RemoveBackEntry();
        }
    }
}