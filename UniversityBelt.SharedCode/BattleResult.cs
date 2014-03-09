namespace UniversityBelt.Model
{
    public class BattleResult
    {
        private readonly School _mySchool;
        private readonly School _opponentSchool;

        public BattleResult(School mySchool, School opponentSchool)
        {
            _mySchool = mySchool;
            _opponentSchool = opponentSchool;
        }

        public School MySchool { get { return _mySchool; } }
        public School OpponentOpponentSchool { get { return _opponentSchool; } }
    }
}