using SampleWebApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.Models.Article
{
    public class IndexViewModel
    {
        public IReadOnlyCollection<MenuItemViewModel> MenuItems
        {
            get;
            private set;
        }

        public IndexViewModel(IEnumerable<ArticleInfo> infos)
        {
            if (infos == null)
            {
                throw new ArgumentNullException(nameof(infos));
            }

            this.MenuItems = infos.Select(t => new MenuItemViewModel(t)).ToList();
        }
    }
}
