namespace UniversityBelt.SharedCode.Model
{
    public class Battle 
    {
        private readonly School _mySchool;
        private readonly School _opponentSchool;
        private const string AttackEventFormat = "{0} is attacking {1} !";
        private const string RepairEventFormat = "{0} is repairing their fortress !";

        public Battle(string mySchoolName, string anotherSchoolName)
        {
            _mySchool = new School(mySchoolName);
            _opponentSchool = new School(anotherSchoolName);
        }

        public School MySchool { get { return _mySchool; } }
        public School OtherSchool { get { return _opponentSchool; } }

        public BattleResult Attack(string schoolName)
        {
            string action;
            if (schoolName != _mySchool.Name)
            {
                _opponentSchool.ReduceLife();
                action = string.Format(AttackEventFormat, _mySchool.Name, _opponentSchool.Name);
            }
            else
            {
                _mySchool.ReduceLife();
                action = string.Format(AttackEventFormat, _opponentSchool.Name, _mySchool.Name);
            }
            return new BattleResult(_mySchool.Name, _opponentSchool.Name, _mySchool.CurrentLife, _opponentSchool.CurrentLife, action);
        }
        
        public BattleResult Repair(string mySchool)
        {

            string action;
            if (mySchool == _mySchool.Name)
            {
                _mySchool.IncreaseLife();
                action = string.Format(RepairEventFormat, _mySchool.Name);
            }
            else
            {
                _opponentSchool.IncreaseLife();
                action = string.Format(RepairEventFormat, _opponentSchool.Name);
            }
            return new BattleResult(_mySchool.Name, _opponentSchool.Name, _mySchool.CurrentLife, _opponentSchool.CurrentLife, action);
        }

        public BattleResult CreateNewBattleResult()
        {
            return new BattleResult(_mySchool.Name, _opponentSchool.Name, _mySchool.CurrentLife, _opponentSchool.CurrentLife);
        }
    }
}