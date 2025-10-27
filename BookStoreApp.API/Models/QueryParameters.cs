using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.API.Models
{
    public class QueryParameters
    {
        [FromQuery(Name = "pageSize")] public int PageSize { get; set; } = 15;
        [FromQuery(Name = "startIndex")] public int StartIndex { get; set; }
        [FromQuery(Name = "allRows")] public bool AllRows { get; set; }
        
    }
}
