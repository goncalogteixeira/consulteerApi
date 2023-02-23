namespace WebApiTemplate.Api.Application.Common.CacheKeys
{
    public static class TodoCacheKeys
    {
        public static string Prefix => "Todo";

        public static string ListKey => $"{Prefix}List";

        public static string SelectListKey => $"{Prefix}SelectList";

        public static string GetAllKey() => $"{Prefix}-All";

        public static string GetDetailsKey(int resourceId) => $"{Prefix}Details-{resourceId}";
    }
}
