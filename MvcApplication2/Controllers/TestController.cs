using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using System.Xml.Linq;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class TestController : ApiController
    {
        public List<Place> GetTests()
        {
            try
            {
                var xelement = XElement.Load("https://www.theaa.ie/api/IncidentService.svc/GetIncidents");

                var cleanExElement = StripNs(xelement);

                var els = cleanExElement.Elements();

                return els.Select(item => new Place
                {
                    Area = (string) item.Element("Area"),
                    IncidentTypeID = (int) item.Element("IncidentTypeID"),
                    ID = (int) item.Element("ID"),
                    Latitude = (double) item.Element("Latitude"),
                    Location = (string) item.Element("Location"),
                    Longitude = (double) item.Element("Longitude"),
                    Report = (string) item.Element("Report"),
                    Title = (string) item.Element("Title"),
                    UpdatedAt = (DateTime) item.Element("UpdatedAt"),
                    ZoomLevel = (int) item.Element("ZoomLevel")
                }).ToList();
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertRoadwatchErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
            return null;
        }

        private static XElement StripNs(XElement root)
        {
            var res = new XElement(
                root.Name.LocalName,
                root.HasElements
                    ? root.Elements().Select(StripNs)
                    : (object) root.Value
                );

            res.ReplaceAttributes(
                root.Attributes().Where(attr => (!attr.IsNamespaceDeclaration)));

            return res;
        }
    }
}