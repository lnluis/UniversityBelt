using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using UniversityBelt.SharedCode.Hubs;
using UniversityBelt.SharedCode.Model;

namespace UniversityBelt.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private bool _isConnected;
        private readonly HubManager _hubManager;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _hubManager = new HubManager(MySchool, SchoolAgainstWith, AttackResult, RepairResult, UpdateBattleProgress, WinnerResult);
        }

        private void WinnerResult(BattleResult battleResult)
        {
            Dispatcher.BeginInvoke(new Action(() =>
                                   {
                                       MessageBox.Show(battleResult.Action);
                                       IsConnected = false;
                                   }));
        }

        public string MySchool
        {
            get { return "UST"; }
        }

        public string SchoolAgainstWith
        {
            get { return "FEU"; }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        private void UpdateBattleProgress(BattleResult battleResult)
        {

            Dispatcher.BeginInvoke(new Action(() =>
                                              {
                                                  MyLife.Value = battleResult.MySchoolLife;
                                                  OtherLife.Value = battleResult.OpponentSchoolLife;
                                              }));
        }

        private void RepairResult(BattleResult battleResult)
        {
            Dispatcher.BeginInvoke(new Action(() =>
                                              {
                                                  UpdateBattleProgress(battleResult);
                                                  Events.Items.Insert(0, battleResult.Action);
                                              }));
        }


        private void AttackResult(BattleResult battleResult)
        {
            Dispatcher.BeginInvoke(new Action(() =>
                                              {
                                                  UpdateBattleProgress(battleResult);
                                                  Events.Items.Insert(0, battleResult.Action);
                                              }));
        }
       
        
        private void Login(object sender, RoutedEventArgs e)
        {
            _hubManager.Connect(EnableActionButtons);
        }

        private void EnableActionButtons()
        {
            Dispatcher.BeginInvoke(new Action(() =>IsConnected = true));
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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
