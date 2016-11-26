using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.Services
{
    public class ArticleDecoratorService : IArticleService
    {
        private readonly IArticleService parent;

        public ArticleDecoratorService(IArticleService parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            this.parent = parent;
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
