using System;

namespace NaturalUruguayGateway.Domain.Models
{
    public class PaginatedCountModel
    {
        public int Total { get; set; }
        public int Pages
        {
            get
            {
                return Paging.Limit > 0 ? (int)Math.Ceiling((decimal)Total / Paging.Limit) : 0;
            }
        }

        public int CurrentPage
        {
            get
            {
                int page = 0;
                if (Paging.Offset == 0)
                {
                    page = 1;
                }
                else if (Paging.Offset + Paging.Limit >= Total)
                {
                    page = Pages;
                }
                else
                {
                    page = Paging.Limit > 0 ? (Paging.Offset / Paging.Limit) + 1 : 0;
                }

                return page;
            }
        }

        public string NextPage { 
            get
            {
                string nextPage = "";
                var offset = Paging.Offset + Paging.Limit;
                if (offset < Total)
                {
                    nextPage = string.Format("?limit={0}&offset={1}&order={2}&direction={3}", Paging.Limit, offset, Paging.Order, Paging.Direction);
                }

                return nextPage;
            }
        }

        public string PreviousPage
        {
            get
            {
                string previousPage = "";
                var offset = Paging.Offset - Paging.Limit;
                if (offset >= 0)
                {
                    
                    previousPage = string.Format("?limit={0}&offset={1}&order={2}&direction={3}", Paging.Limit, offset, Paging.Order, Paging.Direction);
                }

                return previousPage;
            }
        }

        public PagingModel Paging { get; set; }

        public PaginatedCountModel(PagingModel paging, int total)
        {
            this.Paging = paging;
            this.Total = total;
        }
    }
}