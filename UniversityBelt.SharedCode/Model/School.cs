using System.Threading;

namespace UniversityBelt.SharedCode.Model
{
    public class School
    {
        private const int Maxlife = 50;
        private int _currentLife = Maxlife;
        private readonly string _name;

        public School(string name)
        {
            _name = name;
        }

        public int CurrentLife { get { return _currentLife; } }

        public string Name { get { return _name; } }

        public void ReduceLife()
        {
            // Interlocked.Decrement won't go below 0
            _currentLife = Interlocked.Decrement(ref _currentLife);
        }

        public void IncreaseLife()
        {
            if (_currentLife > Maxlife) return;
            _currentLife = Interlocked.Increment(ref _currentLife);
        }
    }
}