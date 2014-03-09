using Newtonsoft.Json.Linq;

namespace UniversityBelt.Model
{
    public class Battle
    {
        private readonly School _mySchool;
        private readonly School _anotherSchool;

        public Battle(School mySchool, School anotherSchool)
        {
            _mySchool = mySchool;
            _anotherSchool = anotherSchool;
        }

        public JObject Attack(string schoolName)
        {
            if (schoolName != _mySchool.Name)
            {
                _anotherSchool.ReduceLife();
            }
            else
            {
                _mySchool.ReduceLife();
            }
            return CreateBattleResult();
        }

        public JObject CreateBattleResult()
        {
            var jsonObject = new JObject();
            jsonObject[_mySchool.Name] = _mySchool.CurrentLife;
            jsonObject[_anotherSchool.Name] = _anotherSchool.CurrentLife;
            return jsonObject;
        }

        public JObject Repair(string mySchool)
        {
            if (mySchool == _mySchool.Name)
            {
                _mySchool.IncreaseLife();
            }
            else
            {
                _anotherSchool.IncreaseLife();
            }
            return CreateBattleResult();
        }
    }
}