using Enums;

namespace ViewModels.Notification
{
    public class MailRequestViewModel
    {
        public MailRequestViewModel(string sendTo, string subject, string body, string userName, NotificationType type)
        {
            SendTo = sendTo;
            Subject = subject;
            Body = body;
            UserName = userName;
            Type = type;
        }
        public string SendTo { get; set; }
        public string UserName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public NotificationType Type { get; set; }
    }
}
