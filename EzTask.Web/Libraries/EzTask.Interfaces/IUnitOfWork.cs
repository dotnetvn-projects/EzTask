using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Interfaces
{
    public interface IUnitOfWork<T> : IDisposable
    {
        T Context { get; }
        int Commit();
        Task<int> CommitAsync();
    }
}
