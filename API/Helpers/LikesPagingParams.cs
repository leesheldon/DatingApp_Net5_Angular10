namespace API.Helpers
{
    public class LikesPagingParams : PaginationParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }
    }
}
