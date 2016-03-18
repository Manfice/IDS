using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;
using IndDev.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IndDev.Controllers
{
    [Authorize(Roles = "M,A")]
    public class NewsController : Controller
    {
        private readonly INewsRepository _repository;
        public int NewsOnPage = 4;


        public NewsController(INewsRepository repository)
        {
            _repository = repository;
        }

        // GET: News
        [AllowAnonymous]
        public ViewResult Index(string category, int page=1)
        {
            var model = new NewsViewModel
            {
                News = _repository.Newses.Where(cat=>category==null||cat.Category==category).Where(p=>p.Published).OrderByDescending(n=>n.NewsTime).Skip((page-1)*NewsOnPage).Take(NewsOnPage),
                PageInfo = new PageInfo
                {
                    CurentPage = page,
                    ItemsPerPage = NewsOnPage,
                    TotalItems = category==null ? _repository.Newses.Count() : _repository.Newses.Count(cat => cat.Category==category)
                },
                 Category = category
            };

            return View(model);
        }

        // GET: News/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id, string returnUrl=null)
        {
            var news = new NewsViewModel
            {
                CurrentNews = _repository.Newses.FirstOrDefault(x=>x.Id==id),
                ReturnUrl = returnUrl
            };
            return View(news);
        }

        
        //Get canEdit news list
        public ActionResult EditList()
        {
            return View(_repository.Newses.OrderByDescending(x=>x.NewsTime));
        }

        // GET: News/Create
        
        public ViewResult Create()
        {
            
            return View("Edit", new Domain.ViewModels.NewsViewModel());
        }

        // GET: News/Edit/5
        
        public ActionResult Edit(int id)
        {
            var news = _repository.News(id);
            var model = new Domain.ViewModels.NewsViewModel();
            
            if (news != null)
            {
                model = new Domain.ViewModels.NewsViewModel
                {
                    Id = news.Id,
                    Published = news.Published,
                    FullNewsBody = news.FullNewsBody,
                    Title = news.Title,
                    Category = news.Category,
                    ShortDescr = news.ShortDescr,
                    Source = news.Source
                };
            }
            return View(model);
        }

        // POST: News/Edit/5
        
        [HttpPost]
        public ActionResult Edit(Domain.ViewModels.NewsViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            _repository.Save(model);
            TempData["message"] = $"Изменения в новости \"{model.Title}\" были произведены успешно";

            return RedirectToAction("EditList","News");
        }

        // POST: News/Delete/5
        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var news = _repository.Remove(id);
                if (news!=null)
                {
                    TempData["message"] = $"Новость: \"{news.Title}\" была успешно удалена.";
                }
                    return RedirectToAction("EditList","News");
            }
            catch
            {
                return RedirectToAction("EditList","News");
            }
        }
    }
}
