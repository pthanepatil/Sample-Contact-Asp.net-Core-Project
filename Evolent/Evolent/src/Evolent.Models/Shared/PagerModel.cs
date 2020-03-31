
namespace Evolent.Models.Shared
{
    public class PagerModel
    {
        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public string ActionLink { get; set; }
    }
}
