using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TicketReservation_WS
{
    public class TestController
    {
        TicketReservation_WS ws = new TicketReservation_WS();

        public DataSet searchFlight(string from,string to, DateTime when)
        {
            DataSet resutl = ws.searchOneWayFlight(when, from, to);
            return resutl;
        }

        public bool validateLogin(string userName,string password)
        {
            return (0<ws.validateLogin(userName, GetSHA1(password)));
        }

        public string reserveFlight(int departureFlightID,int ClientID)
        {
            return ws.makeAOneWayReservation(departureFlightID, ClientID, "");
        }
        public bool makeCheckIn(int ticket)
        {
            return ws.makeCheckIn(ticket);
        }

        private string GetSHA1(string str)
        {
            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
    
    
}