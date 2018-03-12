using SampleWebApplication.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.Services.Implementation
{
    public class ArticleDecoratorService : IArticleService
    {
        private readonly IArticleService parent;

        public ArticleDecoratorService(IArticleService parent)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public List<ArticleInfo> GetArticlesInfo()
        {
            return this.parent.GetArticlesInfo();
        }

        public Article ReadArticle(int id)
        {
            Article article = this.parent.ReadArticle(id);

            article.Autor = "harrison314";
            article.Content = string.Concat(article.Content, Environment.NewLine, "Decorated");

            return article;
        }
    }
}
