namespace UniversityBelt.Model
{
    public class BattleKey
    {
        private readonly string _key;
        public string Value { get { return _key; } }

        public BattleKey(string key)
        {
            _key = key;
        }
    }
}