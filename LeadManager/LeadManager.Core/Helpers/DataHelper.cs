using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadManager.Core.Entities;
using LeadManager.Core.Interfaces;

namespace LeadManager.Core.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }
        public bool HasPrevious => (CurrentPage > 1);

        public int PreviousPage { get; set; }

        public bool HasNext => (CurrentPage < TotalPages);

        public int NextPage { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            //If a pagenumber that does not exist is submitted
            //Send last page as default
            if (pageNumber > TotalPages)
            {
                pageNumber = TotalPages;
            }

            CurrentPage = pageNumber;

            if (HasNext)
            {
                NextPage = CurrentPage + 1;
            }

            if (HasPrevious)
            {
                PreviousPage = CurrentPage - 1;
            }

            AddRange(items);
        }

        public async static Task<PagedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = await Task.FromResult(source.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable().ToList());
            return new PagedList<T>(items, count, pageNumber, pageSize);

        }

        public static object GetPaginationMetadata(PagedList<T> pagedList)
        {
            if (pagedList.HasNext && pagedList.HasPrevious)
            {
                var paginationMetaDataWithNextAndPreviousPage = new
                {
                    CurrentPage = pagedList.CurrentPage,
                    TotalPages = pagedList.TotalPages,
                    PageSize = pagedList.PageSize,
                    TotalCount = pagedList.TotalCount,
                    HasPreviousPage = pagedList.HasPrevious,
                    PreviousPage = pagedList.PreviousPage,
                    HasNextPage = pagedList.HasNext,
                    NextPage = pagedList.NextPage
                };

                return paginationMetaDataWithNextAndPreviousPage;
            }
            else if (pagedList.HasNext)
            {
                var paginationMetaDataWithNextPage = new
                {
                    CurrentPage = pagedList.CurrentPage,
                    TotalPages = pagedList.TotalPages,
                    PageSize = pagedList.PageSize,
                    TotalCount = pagedList.TotalCount,
                    HasPreviousPage = pagedList.HasPrevious,
                    HasNextPage = pagedList.HasNext,
                    NextPage = pagedList.NextPage
                };

                return paginationMetaDataWithNextPage;
            }
            else if (pagedList.HasPrevious)
            {
                var paginationMetaDataWithPreviousPage = new
                {
                    CurrentPage = pagedList.CurrentPage,
                    TotalPages = pagedList.TotalPages,
                    PageSize = pagedList.PageSize,
                    TotalCount = pagedList.TotalCount,
                    HasPreviousPage = pagedList.HasPrevious,
                    HasNextPage = pagedList.HasNext,
                    PreviousPage = pagedList.PreviousPage
                };

                return paginationMetaDataWithPreviousPage;
            }
            else
            {
                var paginationMetaData = new
                {
                    CurrentPage = pagedList.CurrentPage,
                    TotalPages = pagedList.TotalPages,
                    PageSize = pagedList.PageSize,
                    TotalCount = pagedList.TotalCount,
                    HasPreviousPage = pagedList.HasPrevious,
                    HasNextPage = pagedList.HasNext
                };

                return paginationMetaData;
            }


        }
                
    }

    public class LeadFilter : IFilter
    {
        public List<int> leadIds { get; set; } = new List<int>();

        public List<int> supplierIds { get; set; } = new List<int>();

        const int maxPageSize = 20;

        public int PageNumber { get; set; } = 1;
        public DateTime FromCreatedDate { get; set; }
        public DateTime ToCreatedDate { get; set; }

        public bool IncludeSource { get; set; }

        public bool IncludeSupplier { get; set; }

        public bool IncludePeople { get; set; }
        public bool IncludeAttributes { get; set; }

        public bool IncludeTrackingSummary { get; set; }

        private int _pageSize = 5;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public string OrderBy { get; set; } = "DateCreated";
    }
}
