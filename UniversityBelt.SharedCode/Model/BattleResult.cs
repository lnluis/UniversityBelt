namespace UniversityBelt.SharedCode.Model
{
    public class BattleResult
    {
        private readonly string _mySchoolName;
        private readonly string _opponentSchoolName;
        private readonly int _mySchoolLife;
        private readonly int _opponentSchoolLife;

        public BattleResult(string mySchoolName, string opponentSchoolName,
                            int mySchoolLife, int opponentSchoolLife,
                            string action = null)
        {
            _mySchoolName = mySchoolName;
            _opponentSchoolName = opponentSchoolName;
            _mySchoolLife = mySchoolLife;
            _opponentSchoolLife = opponentSchoolLife;
            Action = action;
        }

        public string MySchoolName { get { return _mySchoolName; } }
        public string OpponentSchoolName { get { return _opponentSchoolName; } }
        public string Action { get; set; }
        public int MySchoolLife { get { return _mySchoolLife; } }
        public int OpponentSchoolLife { get { return _opponentSchoolLife; } }
    }
}