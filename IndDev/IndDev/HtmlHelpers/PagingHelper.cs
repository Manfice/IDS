using System;
using System.Text;
using System.Web.Mvc;
using IndDev.Models;

namespace IndDev.HtmlHelpers
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PageInfo pageInfo, Func<int, string> pageUrl)
        {
            var result = new StringBuilder();
            for (var i = 0; i < pageInfo.TotalPages; i++)
            {
                var page = i + 1;
                var tag = new TagBuilder("a");
                tag.MergeAttribute("href",pageUrl(page));
                tag.InnerHtml = page.ToString();
                if (page== pageInfo.CurentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag);
            }
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString SmalPageNav(this HtmlHelper html, PageInfo pageInfo, Func<int, string> pageUrl)
        {
            var isFirst = pageInfo.CurentPage - 1;
            var isLast = pageInfo.CurentPage + 1;
            if (pageInfo.CurentPage==1)
            {
                isFirst = pageInfo.CurentPage;
                isLast = pageInfo.CurentPage + 2;
            }
            if (pageInfo.CurentPage==pageInfo.TotalPages)
            {
                isFirst = pageInfo.CurentPage-2;
                isLast = pageInfo.CurentPage;
            }

            var result = new StringBuilder();
            var pagesLinks = new StringBuilder();
            var pages = new TagBuilder("ul");
            pages.AddCssClass("pagination");
            pages.AddCssClass("reset_link");

            //Start making prev link
            var prevPage = new TagBuilder("li");
            var linkPrevPage = new TagBuilder("a");
            if (pageInfo.CurentPage == 1)
            {
                prevPage.AddCssClass("disabled");
                linkPrevPage.MergeAttribute("href", pageUrl(pageInfo.CurentPage));
            }
            else
            {
                linkPrevPage.MergeAttribute("href",pageUrl(pageInfo.CurentPage-1));
            }
            linkPrevPage.InnerHtml = "&laquo;";
            prevPage.InnerHtml = linkPrevPage.ToString();
            result.Append(prevPage);
            //End making prev page


            for (int i = isFirst; i <= isLast; i++)
            {
                var list = new TagBuilder("li");
                if (i==pageInfo.CurentPage)
                {
                    list.AddCssClass("active");
                }
                var link = new TagBuilder("a");
                link.MergeAttribute("href",pageUrl(i));
                link.InnerHtml = i.ToString();
                list.InnerHtml = link.ToString();
                result.Append(list);
            }
            //Start making next link
            var nextPage = new TagBuilder("li");
            var linkNextPage = new TagBuilder("a");
            if (pageInfo.CurentPage == pageInfo.TotalPages)
            {
                nextPage.AddCssClass("disabled");
                linkNextPage.MergeAttribute("href", pageUrl(pageInfo.CurentPage));
            }
            else
            {
                linkNextPage.MergeAttribute("href", pageUrl(pageInfo.CurentPage + 1));
            }
            linkNextPage.InnerHtml = "&raquo;";
            nextPage.InnerHtml = linkNextPage.ToString();
            result.Append(nextPage);
            //End making Next page

            pages.InnerHtml = result.ToString();
            pagesLinks.Append(pages);

            return MvcHtmlString.Create(pagesLinks.ToString());
        }
    }
}