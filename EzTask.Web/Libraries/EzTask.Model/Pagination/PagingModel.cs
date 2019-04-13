using System;
using System.Collections.Generic;

namespace EzTask.Model
{
    public class PagingModel<T>
    {
        public IEnumerable<T> Data { get; private set; }
        public int TotalRecord { get; private set; }
        public int ItemPerpage { get; private set; }
        public PageListModel PageList { get; private set; }

        public static PagingModel<T> CreatePager(IEnumerable<T> data, int totalRecord,
                int itemPerPage = 10, int currentPage = 1)
        {
            var pageModel = new PagingModel<T>
            {
                PageList = new PageListModel
                {
                    CurrentPage = currentPage
                },
                Data = data,
                ItemPerpage = itemPerPage,
                TotalRecord = totalRecord
            };

            pageModel.GetPages();
            return pageModel;
        }

        private void GetPages()
        {
            // calculate total pages
            int totalPages = (TotalRecord % ItemPerpage) == 0 ? TotalRecord / ItemPerpage : TotalRecord / ItemPerpage + 1;

            // ensure current page isn't out of range
            if (PageList.CurrentPage < 1)
            {
                PageList.CurrentPage = 1;
            }
            else if (PageList.CurrentPage > totalPages)
            {
                PageList.CurrentPage = totalPages;
            }

            int startPage;
            int endPage;

            if (totalPages <= 10)
            {
                // less than 10 total pages so show all
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                // more than 10 total pages so calculate start and end pages
                if (PageList.CurrentPage <= 6)
                {
                    startPage = 1;
                    endPage = 10;
                }
                else if (PageList.CurrentPage + 4 >= totalPages)
                {
                    startPage = totalPages - 9;
                    endPage = totalPages;
                }
                else
                {
                    startPage = PageList.CurrentPage - 5;
                    endPage = PageList.CurrentPage + 4;
                }
            }

            // calculate start and end item indexes
            int startIndex = (PageList.CurrentPage - 1) * ItemPerpage;
            int endIndex = Math.Min(startIndex + ItemPerpage - 1, TotalRecord - 1);

            // create an array of pages to ng-repeat in the pager control
            for (int i = startPage; i <= endPage; i++)
            {
                PageList.Pages.Add(i);
            }
            PageList.TotalPage = totalPages;
        }
    }
}
