namespace UniGame.Runtime.Common
{
    using System;

    [Serializable]
    public class SignaleValueProperty<TValue> : ISignaleValueProperty<TValue>
    {
        public TValue defaultValue;
        public bool hasValue;
        public TValue value;

        public SignaleValueProperty()
        {
            defaultValue = default;
            value = default;
        }

        public SignaleValueProperty(TValue defaultValue)
        {
            this.defaultValue = defaultValue;
            value = defaultValue;
        }

        public bool Has => hasValue;

        public TValue Value
        {
            get => Take();
            set
            {
                this.value = value;
                hasValue = true;
            }
        }

        public void SetValue(TValue newValue)
        {
            value = newValue;
            hasValue = true;
        }

        public TValue Take()
        {
            if (!hasValue) return defaultValue;
            hasValue = false;
            var result = value;
            value = defaultValue;
            return result;
        }

        public TValue Look() => value;

    }
}