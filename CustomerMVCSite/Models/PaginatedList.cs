using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int totalElement, int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalElement / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreatePaging(IQueryable<T> source, int currentPage, int pageSize)
        {
            var totalElement = await source.CountAsync();

            var items = await source.Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(items, totalElement, currentPage, pageSize);
        }
    }
}
