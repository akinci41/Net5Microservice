namespace Directory.Entity
{
    public class QueueMessage
    {
        public string QueueName { get; set; } //dynamic queue name

        public string Object { get; set; } //json object
    }
}
