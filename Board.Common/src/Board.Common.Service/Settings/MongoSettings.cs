namespace Board.Common.Settings.Mongo;

public class MongoSettings
{
    public string Host{get; init;}
    public int Port { get; init; }
    public string ConnectionString => $"mongodb://{Host}:{Port}";
}