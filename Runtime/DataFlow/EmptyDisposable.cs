namespace UniGame.Core.Runtime.Common
{
    using System;

    public class EmptyDisposable : IDisposable
    {
        private static EmptyDisposable _disposable = new();
        public static IDisposable Empty => _disposable;

        public void Dispose()
        {
        }
    }
}