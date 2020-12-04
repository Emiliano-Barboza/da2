using System;
using System.Text.Json.Serialization;

namespace NaturalUruguayGateway.Domain
{
    [Serializable]
    public class Session
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public int UserId { get; set; } 
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}