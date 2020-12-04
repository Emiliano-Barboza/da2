using System.Collections.Generic;

namespace NaturalUruguayGateway.Domain.Models
{
    public class ReportModel<T>
    {
        public IEnumerable<T> Data { get; set; }
    }
}