using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleWebApplication.Services;
using SampleWebApplication.Models.Article;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWebApplication.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService articleServices;

        public ArticleController(IArticleService articleServices)
        {
            this.articleServices = articleServices;
        }

        public IActionResult Index()
        {
            List<ArticleInfo> infos = this.articleServices.GetArticlesInfo();
            IndexViewModel model = new IndexViewModel(infos);
            return View(model);
        }

        public IActionResult Content(int id)
        {
            try
            {
                Article artcle = this.articleServices.ReadArticle(id);
                ContentViewModel model = new ContentViewModel(artcle);
                return this.View(model);
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}
