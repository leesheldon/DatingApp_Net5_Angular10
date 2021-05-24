namespace API.Helpers
{
    public class PagingParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string CurrentUsername { get; set; }
        public string GenderToFilter { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 110;
        public string OrderBy { get; set; } = "lastActive";

    }
}
