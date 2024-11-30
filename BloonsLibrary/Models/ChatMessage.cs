namespace BloonLibrary
{
    public class ChatMessage
    {
        public string Username { get; set; }  // The user who sent the message
        public string Content { get; set; }   // The message content
        public string Timestamp { get; set; } // The timestamp when the message was sent
        public string MessageId { get; set; } // A unique ID for the message
    }
}