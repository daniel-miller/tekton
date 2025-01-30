using System;

namespace Tek.Contract
{
    [Serializable]
    public class Pagination
    {
        public const string HeaderKey = "X-Tek-Pagination";

        public Pagination()
            : this(null, null)
        { 
        
        }

        public Pagination(int? page, int? take, int items, int total)
            : this(page, take)
        {
            Items = items;
            Total = total;
        }

        public Pagination(int? page, int? take)
        {
            // If the page number is not specified, then the first page is assumed.
            if (page == null || page < 1)
                page = 1;

            // Take at least 10 rows, but no more than 100 rows.
            if (take == null || take < 1)
                take = 10;
            else if (take > 100)
                take = 100;

            Page = page;
            Take = take;
        }

        public int? Page { get; set; }
        public int? Take { get; set; }

        public int? Items { get; set; }
        public int? Total { get; set; }

        public int? Pages => Total.HasValue && Take.HasValue ? (int?)Math.Ceiling((double)Total.Value / Take.Value) : null;
        public bool More => (Page.HasValue && Pages.HasValue) ? Page < Pages : false;
    }
}