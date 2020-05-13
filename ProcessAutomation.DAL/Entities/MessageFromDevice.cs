using MongoDB.Bson;

public class MessageFromDevice
{
    public MessageFromDevice()
    {
        Id = new ObjectId();
        Message = string.Empty;
        IsProcessed = false;
    }

    public ObjectId Id { get; set; }
    public string Message { get; set; }
    public bool IsProcessed { get; set; }
}