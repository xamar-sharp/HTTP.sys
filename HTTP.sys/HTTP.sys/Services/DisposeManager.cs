using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace HTTP.sys.Services
{
    public sealed class DisposeManager : IDisposeManager
    {
        private readonly IList<IDisposable> _disposables;
        public DisposeManager()
        {
            _disposables = new List<IDisposable>(1);
        }
        public IDisposeManager Register(IDisposable @new)
        {
            if (!_disposables.Contains(@new))
            {
                _disposables.Add(@new);
            }
            return this;
        }
        public IDisposeManager Unregister(IDisposable @old)
        {
            if (_disposables.Contains(@old))
            {
                _disposables.Remove(@old);
            }
            return this;
        }
        public IDisposeManager UnregisterAll()
        {
            _disposables.Clear();
            return this;
        }
        private void DisposeElements()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
        public async ValueTask DisposeAsync()
        {
            await Task.Run(() =>
            {
                DisposeElements();
            });
        }
        public void Dispose()
        {
            DisposeElements();
        }
    }
}
