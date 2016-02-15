using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{

    public class Score
    {
        public int ScoreHGoals { get; set; }
        public int ScoreHPoints { get; set; }
        public int ScoreAGoals { get; set; }
        public int ScoreAPoints { get; set; }
        public string HomeTeamScore { get; set; }
        public string AwayTeamScore { get; set; }
        public long TotalGoals1 { get; set; }
        public int TotalGoals2 { get; set; }
    }

    public class Referee
    {
        public string Surname { get; set; }
        public string Forename { get; set; }
        public string County { get; set; }
    }

    public class ExtraMatchConditions
    {
        public bool IsPostponed { get; set; }
        public bool IsExtraTimePlayable { get; set; }
        public bool IsReplay { get; set; }
        public bool IsAbadoned { get; set; }
        public bool IsRefixture { get; set; }
        public bool WasNeverPlayed { get; set; }
    }

    public class MatchData
    {
        public int Id { get; set; }
        public int CompetitionSeasonId { get; set; }
        public string CompetitionSeasonName { get; set; }
        public string HomeTeamOfficialName { get; set; }
        public string AwayTeamOfficialName { get; set; }
        public object MatchStartOptions { get; set; }
        public bool IsTbc { get; set; }
        public string MatchDateTime { get; set; }
        public string MatchSortDate { get; set; }
        public string MatchDisplayDateString { get; set; }
        public string MatchStartTime { get; set; }
        public Score Score { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int MatchType { get; set; }
        public string TVProviderName { get; set; }
        public int Group { get; set; }
        public Referee Referee { get; set; }
        public ExtraMatchConditions ExtraMatchConditions { get; set; }
        public string StadiumName { get; set; }
        public string RoundName { get; set; }
        public object GroupName { get; set; }
        public string HomeTeamScore { get; set; }
        public string AwayTeamScore { get; set; }
        public bool BlogExists { get; set; }
        public bool MatchPreviewExists { get; set; }
        public bool MatchReportExists { get; set; }
        public string BlogUrl { get; set; }
        public object SeoArticleTitle { get; set; }
        public string CompSeoName { get; set; }
        public int EventId { get; set; }
        public bool IsAwayTeamConceded { get; set; }
        public bool IsHomeTeamConceded { get; set; }
        public bool IsWalkOver { get; set; }
        public string GaaSport { get; set; }
        public string KickOffTime { get; set; }
        public bool IsResult { get; set; }
        public int RankOrder { get; set; }
        public int GroupRank { get; set; }
        public int AwayTeamType { get; set; }
        public int HomeTeamType { get; set; }
    }

}