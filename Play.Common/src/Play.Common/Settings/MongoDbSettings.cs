namespace Play.Common.Settings
{
  public class MongoDbSettings
  {
    public string DatabaseName { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string ConnectionString => $"mongodb://{Host}:{Port}";
  }
}
