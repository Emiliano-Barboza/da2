using System.Collections.Generic;

namespace NaturalUruguayGateway.Domain.Models
{
    public class PaginatedModel<T>
    {
        public IEnumerable<T> Data { get; set; }
        public PaginatedCountModel Counts { get; set; }

        public PaginatedModel(PagingModel pagingModel, int total)
        {
            Counts = new PaginatedCountModel(pagingModel, total);
        }
    }
}