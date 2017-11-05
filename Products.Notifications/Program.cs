namespace Products.NotificationHubs
{
    using Microsoft.Azure.NotificationHubs;
    using System;

    public class Program
    {
        private static NotificationHubClient hub;

        public static void Main(string[] args)
        {
            hub = NotificationHubClient.CreateClientFromConnectionString(
                "Endpoint=" +
                "sb://productshub.servicebus.windows.net/;" +
                "SharedAccessKeyName=DefaultFullSharedAccessSignature;" +
                "SharedAccessKey=m2W9SkIxJlUNtcAL5Awym/txJFKRMZhifmMka1IY7qA=", 
                "ProductsHub");

            do
            {
                Console.WriteLine("Type a newe message:");
                var message = Console.ReadLine();
                SendNotificationAsync(message);
                Console.WriteLine("The message was sent...");
            } while (true);
        }

        private static async void SendNotificationAsync(string message)
        {
            await hub.SendGcmNativeNotificationAsync("{ \"data\" : {\"message\":\"" + message + "\"}}");
        }
    }
}