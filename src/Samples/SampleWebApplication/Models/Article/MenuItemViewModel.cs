using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.Models.Article
{
    public class MenuItemViewModel
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

        public MenuItemViewModel(Services.Contract.ArticleInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            this.Id = info.Id;
            this.Title = info.Title;
        }
    }
}
