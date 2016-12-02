using UWPHelper.Utilities;

namespace Project42
{
    public sealed class Coordinate : NotifyPropertyChangedBase
    {
        public int Degrees
        {
            get { return (int)GetValue(nameof(Degrees)); }
            set { SetValue(nameof(Degrees), ref value); }
        }
        public int Minutes
        {
            get { return (int)GetValue(nameof(Minutes)); }
            set { SetValue(nameof(Minutes), ref value); }
        }
        public float Seconds
        {
            get { return (float)GetValue(nameof(Seconds)); }
            set { SetValue(nameof(Seconds), ref value); }
        }
        public bool IsPositive
        {
            get { return (bool)GetValue(nameof(IsPositive)); }
            set { SetValue(nameof(IsPositive), ref value); }
        }

        public Coordinate()
        {
            RegisterProperty(nameof(Degrees), typeof(int), 0);
            RegisterProperty(nameof(Minutes), typeof(int), 0);
            RegisterProperty(nameof(Seconds), typeof(float), 0f);
            RegisterProperty(nameof(IsPositive), typeof(bool), true);
        }
    }
}
