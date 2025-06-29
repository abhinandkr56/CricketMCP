namespace CricketMCP.CricketService;

public class Player
{
    public PlayerData[] data { get; set; }
}

public class PlayerData
{
    public string resource { get; set; }
    public int id { get; set; }
    public int country_id { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string fullname { get; set; }
    public string image_path { get; set; }
    public string dateofbirth { get; set; }
    public string gender { get; set; }
    public string battingstyle { get; set; }
    public string bowlingstyle { get; set; }
    public Position position { get; set; }
    public string updated_at { get; set; }
}

public class Position
{
    public string resource { get; set; }
    public int id { get; set; }
    public string name { get; set; }
}


