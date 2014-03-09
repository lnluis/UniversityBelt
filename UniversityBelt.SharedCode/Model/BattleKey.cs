using System.Runtime.Serialization;

namespace UniversityBelt.SharedCode.Model
{
    public class BattleKey
    {
        public int Id { get; set; }

        public string SchoolName { get; set; }

        private readonly string _key;
        [DataMember(Name="Value")]
        public string Value { get { return _key; } }

        public BattleKey(string key)
        {
            _key = key;
        }
    }
}