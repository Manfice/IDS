using System;
using System.Collections.Generic;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Abstract
{
    public interface IHomeRepository
    {
        CursViewModel GetCurses(DateTime date);
        IEnumerable<ProductView> GetTopProducts { get; }
    }
}