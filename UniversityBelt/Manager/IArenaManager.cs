using UniversityBelt.SharedCode.Model;

namespace UniversityBelt.Server.Manager
{
    public interface IArenaManager
    {
        void JoinBattle(string battleKey, string mySchool, string anotherSchool);
        void Attack(string battleKey, string anotherSchool);
        void Repair(string battleKey, string anotherSchool);
        string GetBattleKey(string mySchool);
        BattleResult GetCurrentBattleResult(string battleKey);
    }
}