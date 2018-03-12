using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.Models.Article
{
    public class ContentViewModel
    {
        public int Id
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }

        public string Content
        {
            get;
            private set;
        }

        public string Autor
        {
            get;
            private set;
        }
        public ContentViewModel(SampleWebApplication.Services.Contract.Article article)
        {
            if (article == null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            this.Autor = article.Autor;
            this.Content = article.Content;
            this.Id = article.Id;
            this.Title = article.Title;
        }
    }
}
