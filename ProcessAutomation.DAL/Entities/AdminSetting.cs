using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AdminSetting
{
    public AdminSetting()
    {
        Id = new ObjectId();
        Name = string.Empty;
        Key = string.Empty;
        Value = string.Empty;
    }

    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}