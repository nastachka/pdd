using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using PDD.Utility;
using PDD.Views;

namespace PDD
{
    public sealed partial class App
    {
        private TransitionCollection _transitions;

        private const string DbName = "pdd.sqlite";
        public static string DbPath = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName));
        

        public App()
        {
            InitializeComponent();

            if (!CheckFileExists(DbName).Result)
            {
                CopyDb();
            }
            else
            {
                ReadDataHelper.GetDataFromDb();
            }

            Suspending += OnSuspending;

#if WINDOWS_PHONE_APP
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif
        }

        private static async void CopyDb()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///db/" + DbName));
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                await file.CopyAsync(folder);
                DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
                ReadDataHelper.GetDataFromDb();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private static async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                StorageFile store = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

#if WINDOWS_PHONE_APP
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null || !rootFrame.CanGoBack) return;
            e.Handled = true;
            rootFrame.GoBack();
        }
#endif

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame {CacheSize = 1};
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (rootFrame.ContentTransitions != null)
                {
                    _transitions = new TransitionCollection();
                    foreach (Transition c in rootFrame.ContentTransitions)
                    {
                        _transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += RootFrame_FirstNavigated;

                if (!rootFrame.Navigate(typeof (MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            Window.Current.Activate();
        }

        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            if (rootFrame != null)
            {
                rootFrame.ContentTransitions = _transitions ?? new TransitionCollection {new NavigationThemeTransition()};
                rootFrame.Navigated -= RootFrame_FirstNavigated;
            }
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
        }
    }
}