namespace Models
{
    public class PhoneMessage
    {
        public string Sender;
        public string Message;
        
        public PhoneMessage()
        {
        }
        
        public PhoneMessage(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }
    }
}