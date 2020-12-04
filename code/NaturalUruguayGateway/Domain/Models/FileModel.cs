using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace NaturalUruguayGateway.Domain.Models
{
    public class FileModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}