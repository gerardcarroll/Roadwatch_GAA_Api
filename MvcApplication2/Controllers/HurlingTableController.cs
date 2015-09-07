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
    public class HurlingTableController : ApiController
    {
        public List<Table> GetHurlingTables()
        {
            List<Table> tables = new List<Table>();

            var web = new HtmlWeb();
            HtmlDocument doc;
            try
            {
                //get div 1
                Table div1a = new Table { League = "Roinn 1A", Division = new List<LeagueRow>() };
                var link = "http://www.gaa.ie/fixtures-and-results/league-tables/hurling-league-tables/roinn-1a/2015/";
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
                    div1a.Division.Add(row);
                }
                tables.Add(div1a);

                //get div 1b
                Table div1b = new Table { League = "Roinn 1B", Division = new List<LeagueRow>() };
                var link1b = "http://www.gaa.ie/fixtures-and-results/league-tables/hurling-league-tables/roinn-1b/2015/";
                doc = web.Load(link1b);
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
                    div1b.Division.Add(row);
                }
                tables.Add(div1b);

                //get div 2a
                Table div2a = new Table { League = "Roinn 2A", Division = new List<LeagueRow>() };
                var link3 = "http://www.gaa.ie/fixtures-and-results/league-tables/hurling-league-tables/roinn-2a/2015/";
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
                    div2a.Division.Add(row);
                }
                tables.Add(div2a);

                //get div 2b
                Table div2b = new Table { League = "Roinn 2B", Division = new List<LeagueRow>() };
                var link4 = "http://www.gaa.ie/fixtures-and-results/league-tables/hurling-league-tables/roinn-2b/2015/";
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
                    div2b.Division.Add(row);
                }
                tables.Add(div2b);

                //get div 3a
                Table div3a = new Table { League = "Roinn 3A", Division = new List<LeagueRow>() };
                var link5 = "http://www.gaa.ie/fixtures-and-results/league-tables/hurling-league-tables/roinn-3a/2015/";
                doc = web.Load(link5);
                var nodes5 = doc.DocumentNode.SelectNodes("//table [@id='league_table2']//tr");
                for (int i = 1; i < nodes5.Count; i++)
                {
                    LeagueRow row = new LeagueRow
                    {
                        Pos = nodes5[i].ChildNodes[0].InnerText,
                        Name = nodes5[i].ChildNodes[1].InnerText,
                        Played = nodes5[i].ChildNodes[2].InnerText,
                        Won = nodes5[i].ChildNodes[3].InnerText,
                        Lost = nodes5[i].ChildNodes[4].InnerText,
                        Drawn = nodes5[i].ChildNodes[5].InnerText,
                        For = nodes5[i].ChildNodes[6].InnerText,
                        Aga = nodes5[i].ChildNodes[7].InnerText,
                        Points = nodes5[i].ChildNodes[8].InnerText
                    };
                    div3a.Division.Add(row);
                }
                tables.Add(div3a);

                //get div 3b
                Table div3b = new Table { League = "Roinn 3B", Division = new List<LeagueRow>() };
                var link6 = "http://www.gaa.ie/fixtures-and-results/league-tables/hurling-league-tables/roinn-3b/2015/";
                doc = web.Load(link6);
                var nodes6 = doc.DocumentNode.SelectNodes("//table [@id='league_table2']//tr");
                for (int i = 1; i < nodes6.Count; i++)
                {
                    LeagueRow row = new LeagueRow
                    {
                        Pos = nodes6[i].ChildNodes[0].InnerText,
                        Name = nodes6[i].ChildNodes[1].InnerText,
                        Played = nodes6[i].ChildNodes[2].InnerText,
                        Won = nodes6[i].ChildNodes[3].InnerText,
                        Lost = nodes6[i].ChildNodes[4].InnerText,
                        Drawn = nodes6[i].ChildNodes[5].InnerText,
                        For = nodes6[i].ChildNodes[6].InnerText,
                        Aga = nodes6[i].ChildNodes[7].InnerText,
                        Points = nodes6[i].ChildNodes[8].InnerText
                    };
                    div3b.Division.Add(row);
                }
                tables.Add(div3b);


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
