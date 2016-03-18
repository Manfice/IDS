using System.Collections.Generic;
using IndDev.Domain.Entity;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Abstract
{
    public interface INewsRepository
    {
        IEnumerable<News> Newses { get; }
        News News(int id);
        void Save(NewsViewModel news);
        News Remove(int id);
    }
}