using System;
using System.Collections.Generic;
using System.Text;

namespace HTTP.sys.Services
{
    public interface IDisposeManager : IAsyncDisposable, IDisposable
    {
        IDisposeManager Register(IDisposable newDisposable);
        IDisposeManager Unregister(IDisposable existingDisposable);
        IDisposeManager UnregisterAll();
    }
}
