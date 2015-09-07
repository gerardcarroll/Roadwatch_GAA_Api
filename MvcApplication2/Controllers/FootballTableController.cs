using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HtmlAgilityPack;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class FootballTableController : ApiController
    {
        public List<Table> GetFootballTables()
        {
            List<Table> tables = new List<Table>();
            
            var web = new HtmlWeb();
            HtmlDocument doc;
            try
            {
                //get div 1
                Table div1 = new Table {League = "Roinn 1", Division = new List<LeagueRow>()};
                var link = "http://www.gaa.ie/fixtures-and-results/league-tables/football-league-tables/roinn-1/2015/";
                doc = web.Load(link);
                var nodes = doc.DocumentNode.SelectNodes("//table [@id='league_table2']//tr");
                for (int i = 1; i < nodes.Count; i++)
                {
                    LeagueRow row = new LeagueRow
                                    {
                                        Pos = nodes[i].ChildNodes[0].InnerText,
                                        Name = nodes[i].ChildNodes[1].InnerText,
                                        Played = nodes[i].ChildNodes[2].InnerText,
                                        Won = nodes[i].ChildNodes[3].InnerText,
                                        Lost = nodes[i].ChildNodes[4].InnerText,
                                        Drawn = nodes[i].ChildNodes[5].InnerText,
                                        For = nodes[i].ChildNodes[6].InnerText,
                                        Aga = nodes[i].ChildNodes[7].InnerText,
                                        Points = nodes[i].ChildNodes[8].InnerText
                                    };
                    div1.Division.Add(row);
                }
                tables.Add(div1);

                //get div 2
                Table div2 = new Table { League = "Roinn 2", Division = new List<LeagueRow>() };
                var link2 = "http://www.gaa.ie/fixtures-and-results/league-tables/football-league-tables/roinn-2/2015/";
                doc = web.Load(link2);
                var nodes2 = doc.DocumentNode.SelectNodes("//table [@id='league_table2']//tr");
                for (int i = 1; i < nodes2.Count; i++)
                {
                    LeagueRow row = new LeagueRow
                                    {
                                        Pos = nodes2[i].ChildNodes[0].InnerText,
                                        Name = nodes2[i].ChildNodes[1].InnerText,
                                        Played = nodes2[i].ChildNodes[2].InnerText,
                                        Won = nodes2[i].ChildNodes[3].InnerText,
                                        Lost = nodes2[i].ChildNodes[4].InnerText,
                                        Drawn = nodes2[i].ChildNodes[5].InnerText,
                                        For = nodes2[i].ChildNodes[6].InnerText,
                                        Aga = nodes2[i].ChildNodes[7].InnerText,
                                        Points = nodes2[i].ChildNodes[8].InnerText
                                    };
                    div2.Division.Add(row);
                }
                tables.Add(div2);

                //get div 3
                Table div3 = new Table { League = "Roinn 3", Division = new List<LeagueRow>() };
                var link3 = "http://www.gaa.ie/fixtures-and-results/league-tables/football-league-tables/roinn-3/2015/";
                doc = web.Load(link3);
                var nodes3 = doc.DocumentNode.SelectNodes("//table [@id='league_table2']//tr");
                for (int i = 1; i < nodes3.Count; i++)
                {
                    LeagueRow row = new LeagueRow
                    {
                        Pos = nodes3[i].ChildNodes[0].InnerText,
                        Name = nodes3[i].ChildNodes[1].InnerText,
                        Played = nodes3[i].ChildNodes[2].InnerText,
                        Won = nodes3[i].ChildNodes[3].InnerText,
                        Lost = nodes3[i].ChildNodes[4].InnerText,
                        Drawn = nodes3[i].ChildNodes[5].InnerText,
                        For = nodes3[i].ChildNodes[6].InnerText,
                        Aga = nodes3[i].ChildNodes[7].InnerText,
                        Points = nodes3[i].ChildNodes[8].InnerText
                    };
                    div3.Division.Add(row);
                }
                tables.Add(div3);

                //get div 4
                Table div4 = new Table { League = "Roinn 4", Division = new List<LeagueRow>() };
                var link4 = "http://www.gaa.ie/fixtures-and-results/league-tables/football-league-tables/roinn-4/2015/";
                doc = web.Load(link4);
                var nodes4 = doc.DocumentNode.SelectNodes("//table [@id='league_table2']//tr");
                for (int i = 1; i < nodes4.Count; i++)
                {
                    LeagueRow row = new LeagueRow
                    {
                        Pos = nodes4[i].ChildNodes[0].InnerText,
                        Name = nodes4[i].ChildNodes[1].InnerText,
                        Played = nodes4[i].ChildNodes[2].InnerText,
                        Won = nodes4[i].ChildNodes[3].InnerText,
                        Lost = nodes4[i].ChildNodes[4].InnerText,
                        Drawn = nodes4[i].ChildNodes[5].InnerText,
                        For = nodes4[i].ChildNodes[6].InnerText,
                        Aga = nodes4[i].ChildNodes[7].InnerText,
                        Points = nodes4[i].ChildNodes[8].InnerText
                    };
                    div4.Division.Add(row);
                }
                tables.Add(div4);
                

            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return tables;
        }
    }
}
