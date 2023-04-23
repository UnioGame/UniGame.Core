namespace UniGame.Runtime.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using Unity.IL2CPP.CompilerServices;

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(TValue newValue)
        {
            value = newValue;
            hasValue = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue Take()
        {
            if (!hasValue) return defaultValue;
            hasValue = false;
            var result = value;
            value = defaultValue;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Take(out TValue result)
        {
            if (!hasValue)
            {
                result = default;
                return false;
            }

            result = Take();
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue Look() => value;

    }
}