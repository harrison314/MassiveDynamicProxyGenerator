using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.Services
{
    public interface IArticleService
    {
        List<ArticleInfo> GetArticlesInfo();

        Article ReadArticle(int id);
    }
}
