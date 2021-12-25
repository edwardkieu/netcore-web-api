using System;

namespace Application.Wrappers
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage => (PageNumber > 1);
        public bool HasNextPage => (PageNumber < TotalPages);
    }

    public class PagedResponse<T> : Response<T>
    {
        public Pagination Pagination { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            Pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            Data = data;
            Message = null;
            Succeeded = true;
            Errors = null;
        }

        public PagedResponse(T data, int totalCount, int pageNumber, int pageSize)
        {
            Pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
            Data = data;
            Message = null;
            Succeeded = true;
            Errors = null;
        }
    }
}