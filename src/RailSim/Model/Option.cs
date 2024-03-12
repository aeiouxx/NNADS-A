namespace RailSim.Model
{
    public class Option<T>
    {
        public static readonly Option<T> None = new Option<T>();

        public T Value
        { get; private set; }
        public bool HasValue
        { get; private set; }


        public Option()
        {
            HasValue = false;
        }

        public Option(T value)
        {
            Value = value;
            HasValue = true;
        }

        public void Unset()
        {
            Value = default!;
            HasValue = false;
        }

        public void Set(T value)
        {
            Value = value;
            HasValue = true;
        }

        public static implicit operator Option<T>(T value) => value == null ? None : new Option<T>(value);
    }
}
