using System.Collections.Generic;

namespace MvcApplication2.Models
{
    public class Team1Name
    {
    }

    public class Team2Name
    {
    }

    public class Tv
    {
        //public String TvStation { get; set; }
        public override string ToString()
        {
            return "";
        }
    }

    public class ReportUrl
    {
    }

    public class Fixture1
    {
        public string unique_id { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string competition_name { get; set; }
        public string competition_short_name { get; set; }
        public string competition_id { get; set; }
        public string comp_style { get; set; }
        public string comp_level { get; set; }
        public string comp_type { get; set; }
        public string round_name { get; set; }
        public string club_1_name { get; set; }
        public string club_1_id { get; set; }
        public Team1Name team_1_name { get; set; }
        public string club_2_name { get; set; }
        public string club_2_id { get; set; }
        public Team2Name team_2_name { get; set; }
        public string team_1_goals { get; set; }
        public string team_1_points { get; set; }
        public string team_2_goals { get; set; }
        public string team_2_points { get; set; }
        public string venue_name { get; set; }
        public string venue_id { get; set; }
        public object referee_surname { get; set; }
        public object referee_forename { get; set; }
        public object referee_county { get; set; }
        public string result { get; set; }
        public object notes { get; set; }
        public object tv { get; set; }
        public string postponed { get; set; }
        public string extratime_playable { get; set; }
        public string replay { get; set; }
        public string refixture { get; set; }
        public string parent_competition_name { get; set; }
        public string parent_competition_id { get; set; }
        public string team_1_conceded { get; set; }
        public string team_2_conceded { get; set; }
        public ReportUrl report_url { get; set; }
        public object sponsor { get; set; }
        public string sponsor_position { get; set; }
        public string abandoned { get; set; }
        public string never_played { get; set; }
        public string owner { get; set; }
    }

    public class Fixtures
    {
        public List<Fixture> fixture { get; set; }
    }

    public partial class RootObject
    {
        public Fixtures fixtures { get; set; }
    }
}