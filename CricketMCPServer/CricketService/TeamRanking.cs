
using Newtonsoft.Json;

namespace CricketMCP.CricketService
{
    public class TeamRanking
    {
        [JsonProperty("data")]
        public Data[] Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("position")]
        public object Position { get; set; }

        [JsonProperty("points")]
        public object Points { get; set; }

        [JsonProperty("rating")]
        public object Rating { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("team")]
        public Team[] Team { get; set; }
    }

    public class Team
    {
        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("image_path")]
        public string ImagePath { get; set; }

        [JsonProperty("country_id")]
        public int CountryId { get; set; }

        [JsonProperty("national_team")]
        public bool NationalTeam { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("ranking")]
        public Ranking Ranking { get; set; }
    }

    public class Ranking
    {
        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("matches")]
        public int Matches { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }
    }
}
