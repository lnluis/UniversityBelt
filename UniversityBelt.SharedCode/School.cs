using System.Threading;

namespace UniversityBelt.Model
{
    public class School
    {
        private int _currentLife = 50;
        private readonly string _name;

        public School(string name)
        {
            _name = name;
        }

        public int CurrentLife { get { return _currentLife; } }

        public string Name { get { return _name; } }

        public void ReduceLife()
        {
            _currentLife = Interlocked.Decrement(ref _currentLife);
        }

        public void IncreaseLife()
        {
            _currentLife = Interlocked.Increment(ref _currentLife);
        }
    }
}