using MongoDB.Bson;

public class Message
{
    public ObjectId Id { get; set; }
    public string RecievedDate { get; set; }
    public string Money { get; set; }
    public string Account { get; set; }
    public string Web { get; set; }
    public string MessageContent { get; set; }
    public bool IsSatisfied { get; set; }
    public bool IsProcessed{ get; set; }
}