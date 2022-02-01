using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DenisovArt_Kurs
{

    public partial class DocTimeTable
    {
        public XElement DocTimeTablePassportXml()
        {
            List<Ticket> cTick = new List<Ticket>();
            foreach (Ticket t in (new ProjectEntitiesContext()).Tickets)
            {
                cTick.Add(t);
            }
            
            XElement xClientPassport =
                        new XElement("DocTimeTable",
                            new XElement("DocTimeTableID", DocTimeTableID.ToString()),
                            new XElement("WorkingShiftFirst", WorkingShiftFirst.ToString()),
                            new XElement("Room", Room.ToString()),
                            new XElement("DocID", DocID.ToString()));
            return xClientPassport;
        }
    }

}
