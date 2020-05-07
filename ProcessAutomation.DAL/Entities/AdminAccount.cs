using MongoDB.Bson;

public class AdminAccount
{
    public AdminAccount()
    {
        Id = new ObjectId();
        Web = string.Empty;
        AccountName = string.Empty;
        Password = string.Empty;
    }

    public ObjectId Id { get; set; }
    public string Web { get; set; }
    public string AccountName { get; set; }
    public string Password { get; set; }
}