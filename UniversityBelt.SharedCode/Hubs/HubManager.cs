using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniversityBelt.SharedCode.Model;

namespace UniversityBelt.SharedCode.Hubs
{
    public class HubManager
    {
        private readonly string _mySchool;
        private readonly string _schoolAgainstWith;
        private readonly Action<BattleResult> _startBattleCallback;
        private readonly IHubProxy _arenaHubProxyHubProxy;
        private readonly HubConnection _connection;
        private string _battleKey;

        public HubManager(string mySchool, string schoolAgainstWith, Action<BattleResult> attackResultCallBack,
            Action<BattleResult> repairResultCallBack, Action<BattleResult> startBattleCallback, Action<BattleResult> winnerCallback)
        {
            _mySchool = mySchool;
            _schoolAgainstWith = schoolAgainstWith;
            _startBattleCallback = startBattleCallback;
            _connection = new HubConnection("http://universitybelt.azurewebsites.net/");
            _arenaHubProxyHubProxy = _connection.CreateHubProxy("arenaHub");
            _arenaHubProxyHubProxy.On("AttackResult", attackResultCallBack);
            _arenaHubProxyHubProxy.On("RepairResult", repairResultCallBack);
            _arenaHubProxyHubProxy.On("Winner", winnerCallback);
        }

        public async void Connect(Action callBack)
        {
            await _connection.Start()
                .ContinueWith(taskTwo => GetBattleKey(callBack));
        }

        private async Task GetBattleKey(Action callBack)
        {
            await _arenaHubProxyHubProxy.Invoke<string>("GetBattleKey", _mySchool, _schoolAgainstWith)
                                        .ContinueWith(task => _battleKey = task.Result);

            await _arenaHubProxyHubProxy.Invoke<BattleResult>("GetCurrentBattleResult", _battleKey)
                                        .ContinueWith((task) => _startBattleCallback(task.Result))
                                        .ContinueWith((nextTask) => callBack());
        }

        public async void Repair()
        {
            await _arenaHubProxyHubProxy.Invoke("Repair", _battleKey, _mySchool);
        }

        public async void Attack()
        {
            await _arenaHubProxyHubProxy.Invoke("Attack", _battleKey, _schoolAgainstWith);
        }
    }
}