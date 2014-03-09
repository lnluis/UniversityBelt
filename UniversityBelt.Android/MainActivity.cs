using System;
using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using UniversityBelt.SharedCode.Hubs;
using UniversityBelt.SharedCode.Model;
using String = System.String;

namespace UniversityBelt.Android
{
    [Activity(Label = "University Belt", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        internal static MobileServiceClient MobileService = new MobileServiceClient(
            "https://universitybelt.azure-mobile.net/", "CGPrYVTTnIyiijYAlBQrIHAFAqrDhd40");
        private HubManager _hubManager;
        private bool _isConnected;
        private ProgressBar _mySchoolProgressBar;
        private ProgressBar _opponentSchoolProgressBar;
        private Button _repairkButton;
        private Button _attackButton;
        private Button _startBattleButton;
        private MobileServiceUser _currentUser;
        private ListView _events;
        private readonly List<String> _listItems = new List<String>();
        private ArrayAdapter<String> _adapter;

        protected override void OnCreate(Bundle bundle)
        {
            LoginWithFacebookAsync();

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            _hubManager = new HubManager(MySchool, SchoolAgainstWith, AttackResult, RepairResult, UpdateBattleProgress, WinnerResult);
            _startBattleButton = FindViewById<Button>(Resource.Id.startBattle);
            _attackButton = FindViewById<Button>(Resource.Id.attackButton);
            _repairkButton = FindViewById<Button>(Resource.Id.repairButton);
            _mySchoolProgressBar = FindViewById<ProgressBar>(Resource.Id.mySchoolHealth);
            _opponentSchoolProgressBar = FindViewById<ProgressBar>(Resource.Id.opponentSchoolHealth);
            _events = FindViewById<ListView>(Resource.Id.events);

            _adapter = new ArrayAdapter<String>(this, Resource.Layout.simple_list_item_1, _listItems);
            _events.Adapter = _adapter;
            _adapter.NotifyDataSetChanged();

            _startBattleButton.Click += StartBattleButtonOnClick;
            _attackButton.Click += AttackButtonOnClick;
            _repairkButton.Click += RepairkButtonOnClick;
        }

        private void WinnerResult(BattleResult battleResult)
        {
            Alert("Winner", battleResult.Action, false, result => { });
            RunOnUiThread(() => IsConnected = false);
        }

        private async void LoginWithFacebookAsync()
        {
            while (_currentUser == null)
            {
                try
                {
                    await MobileService
                        .LoginAsync(this, MobileServiceAuthenticationProvider.Facebook)
                        .ContinueWith(task =>
                                      {
                                          var userTable = MobileService.GetTable<User>();
                                          userTable.InsertAsync(new User { UserId = task.Result.UserId });
                                          _currentUser = task.Result;
                                      });
                }
                catch (InvalidOperationException)
                {

                }
            }
        }

        private void RepairkButtonOnClick(object sender, EventArgs eventArgs)
        {
            _hubManager.Repair();
        }

        private void AttackButtonOnClick(object sender, EventArgs eventArgs)
        {
            _hubManager.Attack();
        }

        private void StartBattleButtonOnClick(object sender, EventArgs eventArgs)
        {
            _hubManager.Connect(() => RunOnUiThread(() => IsConnected = true));
        }


        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                _attackButton.Enabled = value;
                _repairkButton.Enabled = value;
            }
        }

        private void UpdateBattleProgress(BattleResult battleResult)
        {
            RunOnUiThread(() =>
                                   {
                                       _mySchoolProgressBar.Progress = battleResult.MySchoolLife;
                                       _opponentSchoolProgressBar.Progress = battleResult.OpponentSchoolLife;
                                   });
        }

        private void RepairResult(BattleResult battleResult)
        {
            RunOnUiThread(() =>
                                   {
                                       UpdateBattleProgress(battleResult);
                                       _listItems.Insert(0,battleResult.Action);
                                       _adapter.NotifyDataSetChanged();
                                   });
        }

        private void AttackResult(BattleResult battleResult)
        {
            RunOnUiThread(() =>
                                   {
                                       UpdateBattleProgress(battleResult);
                                       _listItems.Insert(0, battleResult.Action);
                                       _adapter.NotifyDataSetChanged();
                                   });
        }

        public string MySchool
        {
            get { return "UST"; }
        }

        public string SchoolAgainstWith
        {
            get { return "FEU"; }
        }
        public void Alert(string title, string message, bool cancelButton, Action<Result> callback)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle(title);
            builder.SetIcon(Resource.Drawable.Icon);
            builder.SetMessage(message);

            builder.SetPositiveButton("Ok", (sender, e) => callback(Result.Ok));

            if (cancelButton)
            {
                builder.SetNegativeButton("Cancel", (sender, e) => callback(Result.Canceled));
            }

            builder.Show();
        }
    }

}

