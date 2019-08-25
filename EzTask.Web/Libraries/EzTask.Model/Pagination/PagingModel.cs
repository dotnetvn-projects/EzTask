using System;
using System.Collections.Generic;

namespace EzTask.Model
{
    public class PagingModel<T>
    {
        public IList<T> Data { get; private set; }
        public int TotalRecord { get; private set; }
        public int ItemPerpage { get; private set; }
        public PageListModel PageList { get; private set; }

        public static PagingModel<T> CreatePager(IList<T> data, int totalRecord,
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

            if (totalPages <= 5)
            {
                // less than 10 total pages so show all
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                // more than 10 total pages so calculate start and end pages
                if (PageList.CurrentPage <= 2)
                {
                    startPage = 1;
                    endPage = 5;
                }
                else if (PageList.CurrentPage + 2 >= totalPages)
                {
                    startPage = totalPages - 4;
                    endPage = totalPages;
                }
                else
                {
                    startPage = PageList.CurrentPage - 2;
                    endPage = PageList.CurrentPage + 2;
                }
            }

            // create an array of pages to ng-repeat in the pager control
            for (int i = startPage; i <= endPage; i++)
            {
                PageList.Pages.Add(i);
            }
            PageList.TotalPage = totalPages;
        }
    }
}
