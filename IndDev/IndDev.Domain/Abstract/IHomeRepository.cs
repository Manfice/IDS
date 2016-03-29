using System;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Abstract
{
    public interface IHomeRepository
    {
        CursViewModel GetCurses(DateTime date);
    }
}