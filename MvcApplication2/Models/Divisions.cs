using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication2.Models
{
    public class LeagueRow
    {
        public String Pos { get; set; }
        public string Name { get; set; }
        public String Played { get; set; }
        public String Won { get; set; }
        public String Lost { get; set; }
        public String Drawn { get; set; }
        public String For { get; set; }
        public String Aga { get; set; }
        public String Points { get; set; }
    }

    public class Table
    {
        public String League { get; set; }
        public List<LeagueRow> Division { get; set; }
    }
}
