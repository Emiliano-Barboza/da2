using System;

namespace NaturalUruguayGateway.Domain.Models
{
    [Serializable]
    public class PagingModel
    {
        private const int DefaultLimit = 30;
        private const int DefaultOffset = 0;
        
        public string Direction { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string Order { get; set; }
        public string FilterBy { get; set; }

        public PagingModel()
        {
            Direction = string.Empty;
            Limit = DefaultLimit;
            Offset = DefaultOffset;
            Order = string.Empty;
            FilterBy = string.Empty;
        }
    }
}