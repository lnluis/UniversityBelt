using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using UniversityBelt.Server.Manager;
using UniversityBelt.SharedCode.Model;

namespace UniversityBelt.Server.Hubs
{
    [HubName("arenaHub")]
    public class ArenaHub : Hub
    {
        private readonly IArenaManager _arenaManager;

        public ArenaHub() : this(ArenaManager.Instance) { }

        public ArenaHub(IArenaManager arenaManager)
        {
            _arenaManager = arenaManager;
        }

        public string GetBattleKey(string mySchool, string anotherSchool)
        {
            var battleKey = _arenaManager.GetBattleKey(mySchool);
            Groups.Add(Context.ConnectionId, battleKey);
            _arenaManager.JoinBattle(battleKey, mySchool, anotherSchool);
            return battleKey;
        }

        public BattleResult GetCurrentBattleResult(string battleKey)
        {
            return _arenaManager.GetCurrentBattleResult(battleKey);
        }

        public void LeaveBattleBetween(string mySchool, string anotherSchool)
        {
            var battleKey = _arenaManager.GetBattleKey(mySchool);
            Groups.Remove(Context.ConnectionId, battleKey);
        }

        public void Repair(string battleKey, string mySchool)
        {
            _arenaManager.Repair(battleKey, mySchool);
        }

        public void Attack(string battleKey, string schoolName)
        {
            _arenaManager.Attack(battleKey, schoolName);
        }
    }
}