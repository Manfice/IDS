using System;
using System.Collections.Generic;
using System.Linq;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Context
{
    public class DbNewsRepository : INewsRepository
    {
        readonly DataContext _context = new DataContext();
        public IEnumerable<News> Newses => _context.News.ToList();

        public News News (int id)
        {
            return _context.News.Find(id);
        }

        public void Save(NewsViewModel model)
        {
            if (model.Id == 0)
            {
                var news = new News
                {
                    NewsTime = DateTime.Now,
                    ShortDescr = model.ShortDescr,
                    Source = model.Source,
                    Category = model.Category,
                    FullNewsBody = model.FullNewsBody,
                    Title = model.Title,
                    Published = model.Published
                };
                using (var reader = new System.IO.BinaryReader(model.UploadImage.InputStream))
                {
                    var imgData = reader.ReadBytes(model.UploadImage.ContentLength);
                    news.Glyf = imgData;
                    news.ImageMimeType = model.UploadImage.ContentType;
                }
                _context.News.Add(news);
            }
            else
            {
                var dbNews = _context.News.Find(model.Id);
                if (dbNews!=null)
                {
                    dbNews.Title = model.Title;
                    dbNews.Category = model.Category;
                    dbNews.ShortDescr = model.ShortDescr;
                    dbNews.FullNewsBody = model.FullNewsBody;
                    dbNews.Published = model.Published;
                    dbNews.Source = model.Source;
                    using (var reader = new System.IO.BinaryReader(model.UploadImage.InputStream))
                    {
                        var imgData = reader.ReadBytes(model.UploadImage.ContentLength);
                        dbNews.Glyf = imgData;
                        dbNews.ImageMimeType = model.UploadImage.ContentType;
                    }
                }
            }
                _context.SaveChanges();
        }

        public News Remove(int id)
        {
            var dbNews = _context.News.FirstOrDefault(x => x.Id == id);
            if (dbNews == null) return new News();
            _context.News.Remove(dbNews);
            _context.SaveChanges();
            return dbNews;
        }
    }
}