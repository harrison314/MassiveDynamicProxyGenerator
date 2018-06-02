using Microsoft.AspNetCore.Mvc;
using SampleWebApplication.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleWebApplication.Models.Article;

namespace SampleWebApplication.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService articleServices;
        private readonly INotificationService notificationsService;

        public ArticleController(IArticleService articleServices, INotificationService notificationService)
        {
            this.articleServices = articleServices;
            this.notificationsService = notificationService;
        }

        public IActionResult Index()
        {
            List<ArticleInfo> infos = this.articleServices.GetArticlesInfo();
            IndexViewModel model = new IndexViewModel(infos);
            this.notificationsService.NotifyRead("ArticleMenu", null);

            return View(model);
        }

        public IActionResult Content(int id)
        {
            try
            {
                Article artcle = this.articleServices.ReadArticle(id);
                ContentViewModel model = new ContentViewModel(artcle);
                this.notificationsService.NotifyRead("Article", id);

                return this.View(model);
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}
