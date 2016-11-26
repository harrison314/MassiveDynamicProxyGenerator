using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWebApplication.Services
{
    public class MockArticleService : IArticleService
    {
        private readonly ConcurrentDictionary<int, Article> articles;

        public MockArticleService()
        {
            this.articles = new ConcurrentDictionary<int, Article>();
            this.AddArticle(1);
            this.AddArticle(2);
            this.AddArticle(3);
            this.AddArticle(7);
            this.AddArticle(13);
            this.AddArticle(28);
        }

        public List<ArticleInfo> GetArticlesInfo()
        {
            List<ArticleInfo> infos = new List<ArticleInfo>();
            foreach (Article article in this.articles.Values)
            {
                ArticleInfo info = new ArticleInfo();
                info.Id = article.Id;
                info.Title = article.Title;

                infos.Add(info);
            }

            return infos;
        }

        public Article ReadArticle(int id)
        {
            Article article;

            if (this.articles.TryGetValue(id, out article))
            {
                return article;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        private void AddArticle(int id)
        {
            string title = $"Article {id}";

            StringBuilder article = new StringBuilder();
            for (int i = 0; i < 50; i++)
            {
                article.Append(title);
                article.Append(" ");
                if (i % 13 == 1)
                {
                    article.AppendLine();
                }
            }

            this.articles[id] = new Article()
            {
                Autor = $"Autor {id}",
                Content = article.ToString(),
                Id = id,
                Title = title
            };
        }
    }
}
