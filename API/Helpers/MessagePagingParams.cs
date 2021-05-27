namespace API.Helpers
{
    public class MessagePagingParams : PaginationParams
    {
        public string Username { get; set; }
        public string Container { get; set; } = "Unread";
        
    }
}