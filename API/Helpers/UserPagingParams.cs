namespace API.Helpers
{
    public class UserPagingParams : PaginationParams
    {        
        public string CurrentUsername { get; set; }
        public string GenderToFilter { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 110;
        public string OrderBy { get; set; } = "lastActive";

    }
}
