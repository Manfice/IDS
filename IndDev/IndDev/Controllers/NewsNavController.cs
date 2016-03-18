using System.Linq;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Models;

namespace IndDev.Controllers
{
    [AllowAnonymous]
    public class NewsNavController:Controller
    {
        private readonly INewsRepository _repository;

        public NewsNavController(INewsRepository repository)
        {
            _repository = repository;
        }

        public PartialViewResult Navigation(string category = null)
        {
           var categorys = new CatViewModel
           {
               Categories = _repository.Newses.Select(cat => cat.Category).Distinct().OrderBy(x => x),
               SelectedCategory = category
           };
            return PartialView(categorys);
        }

        public PartialViewResult LastNews()
        {
            var news = _repository.Newses.Where(p=>p.Published).OrderByDescending(n => n.NewsTime).Take(4);
            return PartialView(news);
        }

        public PartialViewResult NewsBlock()
        {
            var news = _repository.Newses.Where(p => p.Published).OrderByDescending(n => n.NewsTime).Take(6);
            return PartialView(news);
        }

        public FileContentResult GetImage(int id)
        {
            var news = _repository.News(id);
            return news != null ? File(news.Glyf, news.ImageMimeType) : null;
        }
    }
}