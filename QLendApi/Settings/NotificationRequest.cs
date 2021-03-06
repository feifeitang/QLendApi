using System;

namespace QLendApi.Settings
{
    public class NotificationRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Action { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
        public bool Silent { get; set; }
    }
}