using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;
using Facebook.Client;
using UniversityBelt.SharedCode;
using UniversityBelt.SharedCode.Hubs;
using UniversityBelt.SharedCode.Model;
using UniversityBelt.WindowsPhone.Annotations;

namespace UniversityBelt.WindowsPhone
{
    public partial class MainPage : INotifyPropertyChanged
    {
        private bool _isConnected;
        private readonly HubManager _hubManager;
        private string _userId;

        public MainPage()
        {
            InitializeComponent();
            DataContext = this;
            _hubManager = new HubManager(MySchool,
                                         SchoolAgainstWith,
                                         AttackResult,
                                         RepairResult,
                                         UpdateBattleProgress,
                                         WinnerResult);
        }

        private void WinnerResult(BattleResult battleResult)
        {
            Dispatcher.BeginInvoke(() =>
                                   {
                                       MessageBox.Show(battleResult.Action);
                                       IsConnected = false;
                                   });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CheckUri(e.Uri);
        }

        private void Authenticate()
        {
            var fb = new FacebookSessionClient("1423782437860203");
            fb.LoginWithApp("basic_info");
        }
        
        private void CheckUri(Uri uri)
        {
            if (AppAuthenticationHelper.IsFacebookLoginResponse(uri))
            {
                var session = new FacebookSession();
                try
                {
                    session.ParseQueryString(HttpUtility.UrlDecode(uri.ToString()));
                    _userId = UniversityBeltServices.LoginWindowsPhoneAsync(session.AccessToken).Result;
                    MessageBox.Show(string.Format("You are now logged in - {0}", _userId));
                }
                catch (Facebook.FacebookOAuthException exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
            else
            {
                Authenticate();
            }
        }

        public string MySchool
        {
            get { return "FEU"; }
        }

        public string SchoolAgainstWith
        {
            get { return "UST"; }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; OnPropertyChanged();}
        }

        private void UpdateBattleProgress(BattleResult battleResult)
        {
            Dispatcher.BeginInvoke(() =>
                                   {
                                       MyLife.Value = battleResult.MySchoolLife;
                                       OtherLife.Value = battleResult.OpponentSchoolLife;
                                   });
        }

        private void RepairResult(BattleResult battleResult)
        {
            Dispatcher.BeginInvoke(() =>
                                   {
                                       UpdateBattleProgress(battleResult);
                                       Events.Items.Insert(0, battleResult.Action);
                                   });
        }

        private void AttackResult(BattleResult battleResult)
        {
            Dispatcher.BeginInvoke(() =>
                                   {
                                       UpdateBattleProgress(battleResult);
                                       Events.Items.Insert(0, battleResult.Action);
                                   });
        }
        
        private void Login(object sender, RoutedEventArgs e)
        {
            _hubManager.Connect(EnableActionButtons);
        }

        private void EnableActionButtons()
        {
            Dispatcher.BeginInvoke(() => IsConnected = true);
        }

        private void Repair(object sender, RoutedEventArgs e)
        {
            _hubManager.Repair();
        }

        private void Attack(object sender, RoutedEventArgs e)
        {
            _hubManager.Attack();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
