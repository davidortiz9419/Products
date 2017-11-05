namespace Products.Droid
{
    public class Constants
    {
        public const string SenderID = "963742252260"; // Google API Project Number
        public const string ListenConnectionString = "Endpoint=" +
            "sb://productshub.servicebus.windows.net/;" +
            "SharedAccessKeyName=DefaultFullSharedAccessSignature;" +
            "SharedAccessKey=m2W9SkIxJlUNtcAL5Awym/txJFKRMZhifmMka1IY7qA=";
        public const string NotificationHubName = "ProductsHub";
    }
}