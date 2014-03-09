using System;
using System.Collections.Concurrent;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using UniversityBelt.Server.Hubs;
using UniversityBelt.SharedCode.Model;

namespace UniversityBelt.Server.Manager
{
    public class ArenaManager : IArenaManager
    {
        private readonly ConcurrentDictionary<string, BattleKey> _battleKeys = new ConcurrentDictionary<string, BattleKey>();
        // Singleton instance
        private readonly static Lazy<ArenaManager> LazyInstance = new Lazy<ArenaManager>(
            () => new ArenaManager(GlobalHost.ConnectionManager.GetHubContext<ArenaHub>().Clients));

        private readonly IHubConnectionContext _clients;
        private ConcurrentDictionary<string, Battle> _battles;
        private const string WinnerActionFormat = "{0} won the game!";

        private ArenaManager(IHubConnectionContext clients)
        {
            _clients = clients;
            InitializeBattleKeys();
        }

        private void InitializeBattleKeys()
        {
            // Each school can only have one battle at a time.
            // We have to implement 1 - n mapping if we'll implement more than one battle
            _battleKeys.TryAdd("FEU", new BattleKey("FEUUST"));
            _battleKeys.TryAdd("UST", new BattleKey("FEUUST"));
            _battleKeys.TryAdd("DLSU", new BattleKey("DLSUADMU"));
            _battleKeys.TryAdd("ADMU", new BattleKey("DLSUADMU"));
        }

        public ConcurrentDictionary<string, Battle> Battles
        {
            get { return _battles ?? (_battles = new ConcurrentDictionary<string, Battle>()); }
            set { _battles = value; }
        }

        public static ArenaManager Instance
        {
            get
            {
                return LazyInstance.Value;
            }
        }

        public void JoinBattle(string battleKey, string mySchoolName, string anotherSchoolName)
        {
            var battle = new Battle(mySchoolName, anotherSchoolName);
            Battles.TryAdd(battleKey, battle);
        }

        public void Attack(string battleKey, string anotherSchool)
        {
            Battle currentBattle;

            if (!Battles.TryGetValue(battleKey, out currentBattle)) return;

            var battleResult = currentBattle.Attack(anotherSchool);
            _clients.Group(battleKey).AttackResult(battleResult);
            CheckWinner(battleKey, anotherSchool, battleResult);
        }

        private void CheckWinner(string battleKey, string anotherSchool, BattleResult battleResult)
        {
            var isSomeoneDeafeted = battleResult.MySchoolLife != 0 && battleResult.OpponentSchoolLife != 0;
            if (isSomeoneDeafeted) return;

            battleResult.Action = battleResult.MySchoolLife == 0
             ? string.Format(WinnerActionFormat, anotherSchool)
             : string.Format(WinnerActionFormat, battleResult.MySchoolName);

            _clients.Group(battleKey).Winner(battleResult);
            Battle currentBattle;
            Battles.TryRemove(battleKey,out currentBattle);
        }

        public void Repair(string battleKey, string mySchool)
        {
            Battle currentBattle;
            if (!Battles.TryGetValue(battleKey, out currentBattle)) return;

            var battleResult = currentBattle.Repair(mySchool);
            _clients.Group(battleKey).RepairResult(battleResult);
        }

        public string GetBattleKey(string mySchool)
        {
            BattleKey battleKey;
            _battleKeys.TryGetValue(mySchool, out battleKey);
            return battleKey != null ? battleKey.Value : string.Empty;
        }

        public BattleResult GetCurrentBattleResult(string battleKey)
        {
            return Battles[battleKey].CreateNewBattleResult();
        }
    }
}