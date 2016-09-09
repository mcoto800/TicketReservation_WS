using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;

namespace TicketReservation_WS
{
    /// <summary>
    /// Summary description for TicketReservation_WS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TicketReservation_WS : System.Web.Services.WebService
    {
        DataBase db;

        [WebMethod]
        public DataSet getAllAirports()
        {
            db = new DataBase();
            DataSet response = null;

            string sqlCommand = "SELECT * FROM AIRPORT";

            response = db.readCommand(sqlCommand, "AIRPORT");
            return response;
        }

        #region Client
        [WebMethod]
        public int validateLogin(string username, string password)
        {
            db = new DataBase();
            DataSet response = null;


            string sqlCommand = "SELECT CLIENT_ID FROM CLIENT WHERE USERNAME = @USERNAME AND PSWD = @PASSWORD";
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@USERNAME", SqlDbType.VarChar,20);
            param[0].Value = username;

            param[1] = new SqlParameter("@PASSWORD", SqlDbType.VarChar, 40);
            param[1].Value = password;
            response = db.readCommand(sqlCommand, "CLIENT",param);

            if (response.Tables[0].Rows.Count > 0)
            {
                return int.Parse(response.Tables[0].Rows[0][0].ToString());
            }else{
                return -1;
            }
            
        }

        [WebMethod]
        public int newClient(string userName, string password, string email, string firstName, string lastName)
        {


            try
            {
                db = new DataBase();
                string sqlCommand = null;


                sqlCommand = "INSERT INTO CLIENT(USERNAME,PSWD,EMAIL,FIRST_NAME,LAST_NAME) VALUES(@USERNAME,@PSWD, @EMAIL, @FIRSTNAME, @LASTNAME) ";
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@USERNAME", SqlDbType.VarChar,20);
                param[0].Value = userName;

                param[1] = new SqlParameter("@PSWD", SqlDbType.VarChar, 40);
                param[1].Value = password;

                param[2] = new SqlParameter("@EMAIL", SqlDbType.VarChar,100);
                param[2].Value = email;

                param[3] = new SqlParameter("@FIRSTNAME", SqlDbType.VarChar,40);
                param[3].Value = firstName;

                param[4] = new SqlParameter("@LASTNAME", SqlDbType.VarChar, 40);
                param[4].Value = lastName;

                

                if(db.executeCommand(sqlCommand, param))
                {
                    db = new DataBase();
                    DataSet response = null;

                    sqlCommand = "SELECT [CLIENT_ID] FROM CLIENT WHERE USERNAME=@USERNAME";
                     param = new SqlParameter[1];
                    param[0] = new SqlParameter("@USERNAME", SqlDbType.VarChar, 20);
                    param[0].Value = userName;

                    response = db.readCommand(sqlCommand, "CLIENT", param);
                    if (response.Tables[0].Rows.Count > 0)
                    {
                        return int.Parse(response.Tables[0].Rows[0][0].ToString());
                    }
                    else
                    {
                        return -1;
                    }

                }else
                {
                    return -1;
                }

            }
            catch (Exception generatedExceptionName)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "newClient: " + generatedExceptionName.Message);
                return -1;
            }
        }

        [WebMethod]
        public DataSet getClient(int userID)
        {
            db = new DataBase();
            DataSet response = null;


            string sqlCommand = "SELECT [CLIENT_ID],[USERNAME] ,[EMAIL],[FIRST_NAME],[LAST_NAME] FROM CLIENT WHERE CLIENT_ID=@CLIENTID";
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@CLIENTID", SqlDbType.Int);
            param[0].Value = userID;

            response = db.readCommand(sqlCommand, "CLIENT", param);


            return response;
            

        }
        [WebMethod]
        public DataSet getClientByMail(string email)
        {
            db = new DataBase();
            DataSet response = null;


            string sqlCommand = "SELECT [CLIENT_ID],[USERNAME] ,[EMAIL],[FIRST_NAME],[LAST_NAME] FROM CLIENT WHERE EMAIL=@EMAIL";
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@EMAIL", SqlDbType.VarChar,100);
            param[0].Value = email;

            response = db.readCommand(sqlCommand, "CLIENT", param);


            return response;


        }

        [WebMethod]
        public bool validNewUser(string email,string username)
        {
            db = new DataBase();
            DataSet response = null;


            string sqlCommand = "DECLARE @clientes int,@staff int; SET @clientes = (SELECT COunt(*)  FROM CLIENT where CLIENT.EMAIL=@EMAIL OR CLIENT.USERNAME = @USERNAME ) ;SET @staff = (SELECT COunt(*)  FROM STAFF where STAFF.EMAIL=@EMAIL OR STAFF.USERNAME = @USERNAME ) ; Select @clientes+@staff";
                
                             
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@EMAIL", SqlDbType.VarChar, 100);
            param[0].Value = email;

            param[1] = new SqlParameter("@USERNAME", SqlDbType.VarChar, 30);
            param[1].Value = email;


            response = db.readCommand(sqlCommand, "CLIENT", param);

            if (int.Parse(response.Tables[0].Rows[0][0].ToString()) == 0)
            {
                return true;
            }else
            {
                return false;
            }

        }
        #endregion

        #region Flight
        [WebMethod]
        public bool InsertFlight(int airlineID, string flightNumber, DateTime departureDate, DateTime arrivingDate, string departureAirportCode, string destinationAirportCode, int airplaneID, float price )
        {

            
            try
            {
                db = new DataBase();
                string sqlCommand = null;


                sqlCommand = "INSERT INTO FLIGHT([AIRLINE_ID],[FLIGHT_NUMBER],[DEPARTURE_DATE],[ARRIVING_DATE],[DEPARTURE_AIRPORT_CODE],[DESTINATION_AIRPORT_CODE],[AIRPLANE_ID],[PRICE]) VALUES(@AIRLINE_ID,@FLIGHT_NUMBER, @DEPARTURE_DATE, @ARRIVING_DATE, @DEPARTURE_AIRPORT_CODE,@DESTINATION_AIRPORT_CODE, @AIRPLANE_ID, @PRICE) ";
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@AIRLINE_ID", SqlDbType.Int);
                param[0].Value = airlineID;

                param[1] = new SqlParameter("@FLIGHT_NUMBER", SqlDbType.VarChar,10);
                param[1].Value = flightNumber;

                param[2] = new SqlParameter("@DEPARTURE_DATE", SqlDbType.DateTime);
                param[2].Value = departureDate;

                param[3] = new SqlParameter("@ARRIVING_DATE", SqlDbType.DateTime);
                param[3].Value = arrivingDate;

                param[4] = new SqlParameter("@DEPARTURE_AIRPORT_CODE", SqlDbType.VarChar,3);
                param[4].Value = departureAirportCode;

                param[5] = new SqlParameter("@DESTINATION_AIRPORT_CODE", SqlDbType.VarChar,3);
                param[5].Value = destinationAirportCode;

                param[6] = new SqlParameter("@AIRPLANE_ID", SqlDbType.Int);
                param[6].Value = airplaneID;

                param[7] = new SqlParameter("@PRICE", SqlDbType.Int);
                param[7].Value = price;
                

                return db.executeCommand(sqlCommand, param);
               
            }
            catch (Exception generatedExceptionName)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "InsertFlight: " + generatedExceptionName.Message);
                return false;
            }
        }

        [WebMethod]
        public DataSet getAirlineFlights(int airlineID)
        {
            db = new DataBase();
            DataSet response = null;

            string sqlCommand = "SELECT * FROM [TicketReservationDB].[dbo].[Flights] where AIRLINE_ID = @AIRLINEID ORDER BY Departure ASC";

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@AIRLINEID", SqlDbType.Int);
            param[0].Value = airlineID;

            response = db.readCommand(sqlCommand, "FLIGHT", param);
            return response;
        }


        [WebMethod]
        public DataSet searchRoundTripFlight(DateTime departureDate, DateTime returnDate, string fromAirportCode, string toAirportCode)
        {
            db = new DataBase();
            DataSet response = null;
            
            
            //First select all departure flights
            string sqlCommand = "SELECT * FROM [TicketReservationDB].[dbo].[Flights] where DEPARTURE_AIRPORT_CODE = @DEPARTURE AND DESTINATION_AIRPORT_CODE = @DESTINATION AND DEPARTURE_DATE >= @DEPARTURE_DATE AND DEPARTURE_DATE < DATEADD(HOUR, 24, @DEPARTURE_DATE) ORDER BY PRICE ASC";

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@DEPARTURE", SqlDbType.VarChar, 3);
            param[0].Value = fromAirportCode;

            param[1] = new SqlParameter("@DESTINATION", SqlDbType.VarChar, 3);
            param[1].Value = toAirportCode;

            param[2] = new SqlParameter("@DEPARTURE_DATE", SqlDbType.DateTime);
            param[2].Value = departureDate;

            response = db.readCommand(sqlCommand, "DEPARTURE", param);


            // Select all return flights
            sqlCommand = "SELECT * FROM [TicketReservationDB].[dbo].[Flights] where DEPARTURE_AIRPORT_CODE = @DESTINATION AND DESTINATION_AIRPORT_CODE = @DEPARTURE AND DEPARTURE_DATE >= @RETURN_DATE AND DEPARTURE_DATE < DATEADD(HOUR, 24, @RETURN_DATE) ORDER BY PRICE ASC";

            param = new SqlParameter[3];
            param[0] = new SqlParameter("@DEPARTURE", SqlDbType.VarChar, 3);
            param[0].Value = fromAirportCode;

            param[1] = new SqlParameter("@DESTINATION", SqlDbType.VarChar, 3);
            param[1].Value = toAirportCode;

            param[2] = new SqlParameter("@RETURN_DATE", SqlDbType.DateTime);
            param[2].Value = returnDate;

            response.Merge(db.readCommand(sqlCommand, "RETURN", param));
            return response;
        }

        [WebMethod]
        public DataSet searchOneWayFlight(DateTime departureDate, string fromAirportCode, string toAirportCode)
        {
            db = new DataBase();
            DataSet response = null;

            string sqlCommand = "SELECT * FROM [TicketReservationDB].[dbo].[Flights] where DEPARTURE_AIRPORT_CODE = @DEPARTURE AND DESTINATION_AIRPORT_CODE = @DESTINATION AND DEPARTURE_DATE >= @DEPARTURE_DATE AND DEPARTURE_DATE < DATEADD(HOUR, 24, @DEPARTURE_DATE) ORDER BY PRICE ASC";

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@DEPARTURE", SqlDbType.VarChar,3);
            param[0].Value = fromAirportCode;

            param[1] = new SqlParameter("@DESTINATION", SqlDbType.VarChar, 3);
            param[1].Value = toAirportCode;

            param[2] = new SqlParameter("@DEPARTURE_DATE", SqlDbType.DateTime);
            param[2].Value = departureDate;

            response = db.readCommand(sqlCommand, "FLIGHT",param);
            return response;
        }

        [WebMethod]
        public DataSet getTripDetails( string reservationNumber)
        {
            db = new DataBase();
            DataSet response = null;

            string sqlCommand = "SELECT * FROM [TicketReservationDB].[dbo].[TicketFlights] where RESERVATION_NUMER = @RESERVATION ORDER BY Departure ASC";

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RESERVATION", SqlDbType.VarChar, 6);
            param[0].Value = reservationNumber;

            response = db.readCommand(sqlCommand, "FLIGHT", param);
            return response;
        }

        [WebMethod]
        public bool makeCheckIn(int ticketID)
        {
            try
            {
                string sqlCommand = null;

                sqlCommand = "UPDATE TICKET SET CHECKED_IN=1 where TICKET_ID = @TICKET ";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@TICKET", SqlDbType.Int);
                param[0].Value = ticketID;
                
                db = new DataBase();
                return db.executeCommand(sqlCommand, param);
                

            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "makeCheckIn: " + ex.Message);
                return false;
            }
        }

        [WebMethod]
        public bool changeSeat(int ticketID, string seat)
        {
            try
            {
                string sqlCommand = null;

                sqlCommand = "UPDATE TICKET SET SEAT_NUMER=@SEAT where TICKET_ID = @TICKET ";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@TICKET", SqlDbType.Int);
                param[0].Value = ticketID;

                param[1] = new SqlParameter("@SEAT", SqlDbType.VarChar,4);
                param[1].Value = seat;

                db = new DataBase();
                return db.executeCommand(sqlCommand, param);


            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "makeCheckIn: " + ex.Message);
                return false;
            }
        }

        [WebMethod]
        public bool cancelReservation(string reservation)
        {
            try
            {
                string sqlCommand = null;

                sqlCommand = "DELETE FROM TICKET where RESERVATION_NUMER = @RESERV ";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@RESERV", SqlDbType.VarChar,6);
                param[0].Value = reservation;
                

                db = new DataBase();
                return db.executeCommand(sqlCommand, param);


            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "makeCheckIn: " + ex.Message);
                return false;
            }
        }

        [WebMethod]
        public bool cancelFlight(int flightID)
        {
            try
            {
                string sqlCommand = null;

                sqlCommand = "DELETE FROM TICKET where FLIGHT_ID = @FLIGHTID ";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@FLIGHTID", SqlDbType.Int);
                param[0].Value = flightID;


                db = new DataBase();
                if( db.executeCommand(sqlCommand, param))
                {
                    sqlCommand = "DELETE FROM FLIGHT where FLIGHT_ID = @FLIGHTID ";
                    param = new SqlParameter[1];
                    param[0] = new SqlParameter("@FLIGHTID", SqlDbType.Int);
                    param[0].Value = flightID;


                    db = new DataBase();
                    return db.executeCommand(sqlCommand, param);
                }
                else
                {
                    return false;
                }



            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "CancelFlight: " + ex.Message);
                return false;
            }
        }
        [WebMethod]
        public string makeAOneWayReservation (int departureFlightID, int clientID, string seatNumber)
        {
            try
            {
                string sqlCommand = null;
                string reservationNumber;

                sqlCommand = "INSERT INTO TICKET(FLIGHT_ID, SEAT_NUMER, RESERVATION_NUMER, CHECKED_IN,CLIENT_ID) VALUES(@FLIGHT_ID, @SEAT_NUMBER, @RESERVATION_NUMBER, @CHECKED_IN,@CLIENTID) ";
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@FLIGHT_ID", SqlDbType.Int);
                param[0].Value = departureFlightID;

                param[1] = new SqlParameter("@SEAT_NUMBER", SqlDbType.VarChar, 4);
                param[1].Value = seatNumber;

                param[2] = new SqlParameter("@RESERVATION_NUMBER", SqlDbType.VarChar, 6);
                reservationNumber = generateReservationNumber();
                param[2].Value = reservationNumber;

                param[3] = new SqlParameter("@CHECKED_IN", SqlDbType.Bit);
                param[3].Value = false;

                param[4] = new SqlParameter("@CLIENTID", SqlDbType.Int);
                param[4].Value = clientID;

                db = new DataBase();
                if(db.executeCommand(sqlCommand, param))
                {
                    return reservationNumber;
                }else
                {
                    return "";
                }
                
            }catch(Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "makeAOneWayReservation: " + ex.Message);
                return "";
            }
        }

        [WebMethod]
        public string makeARoundTripReservation(int departureFlightID, int returnFlightID, int clientID, string departureSeatNumber, string returnSeatNumber)
        {
            try
            {
                string sqlCommand = null;

                string reservationNumber;

                sqlCommand = "INSERT INTO TICKET(FLIGHT_ID, SEAT_NUMER, RESERVATION_NUMER, CHECKED_IN, CLIENT_ID) VALUES(@DEP_FLIGHT_ID, @DEP_SEAT_NUMBER, @RESERVATION_NUMBER, @CHECKED_IN, @CLIENTID) ";
                sqlCommand += "INSERT INTO TICKET(FLIGHT_ID, SEAT_NUMER, RESERVATION_NUMER, CHECKED_IN, CLIENT_ID) VALUES(@RET_FLIGHT_ID, @RET_SEAT_NUMBER, @RESERVATION_NUMBER, @CHECKED_IN,@CLIENTID) ";

                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@DEP_FLIGHT_ID", SqlDbType.Int);
                param[0].Value = departureFlightID;

                param[1] = new SqlParameter("@DEP_SEAT_NUMBER", SqlDbType.VarChar, 4);
                param[1].Value = departureSeatNumber;

                param[2] = new SqlParameter("@RET_FLIGHT_ID", SqlDbType.Int);
                param[2].Value = returnFlightID;

                param[3] = new SqlParameter("@RET_SEAT_NUMBER", SqlDbType.VarChar, 4);
                param[3].Value = returnSeatNumber;

                param[4] = new SqlParameter("@RESERVATION_NUMBER", SqlDbType.VarChar, 6);
                reservationNumber = generateReservationNumber();
                param[4].Value = reservationNumber;

                param[5] = new SqlParameter("@CHECKED_IN", SqlDbType.Bit);
                param[5].Value = false;

                param[6] = new SqlParameter("@CLIENTID", SqlDbType.Int);
                param[6].Value = clientID;

                db = new DataBase();
                if (db.executeCommand(sqlCommand, param))
                {
                    return reservationNumber;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "makeARoundTripReservation: " + ex.Message);
                return "";
            }
        }

        private string generateReservationNumber()
        {
            try
            {
                string input = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                StringBuilder reservationNumber = new StringBuilder();
                char ch;
                
                for (int i = 0; i < 6; i++)
                {
                    
                    ch = input[GetRandomNumber(0, input.Length)];
                    reservationNumber.Append(ch);
                }
                db = new DataBase();
                DataSet response = null;

                string sqlCommand = "SELECT RESERVATION_NUMER FROM TICKET WHERE RESERVATION_NUMER=@RESERVATIONNUMBER";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@RESERVATIONNUMBER", SqlDbType.VarChar, 6);
                param[0].Value = reservationNumber.ToString();

                response = db.readCommand(sqlCommand, "TICKET", param);

                if (response.Tables[0].Rows.Count == 0)
                {
                    return reservationNumber.ToString();
                }
                else
                {
                    return generateReservationNumber();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "generateReservationNumber: " + ex.Message);
                return "";
            }
            
        }
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
            }
        }

        [WebMethod]
        public DataSet getFlightById(int flightID)
        {
            db = new DataBase();
            DataSet response = null;

            string sqlCommand = "SELECT * FROM [TicketReservationDB].[dbo].[Flights] where FLIGHT_ID=@FLIGHTID";

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@FLIGHTID", SqlDbType.Int);
            param[0].Value = flightID;

            response = db.readCommand(sqlCommand, "FLIGHT", param);
            return response;
        }

        #endregion

        #region Tickets

        [WebMethod]
        public DataSet getFlightsDepartingHours(int hours)
        {
            db = new DataBase();
            DataSet response = null;

            string sqlCommand = "SELECT * FROM [TicketReservationDB].[dbo].[TicketFlights] where DATEDIFF(mi, GETDATE(), Departure_DATE) = ("+hours+"*60) ORDER BY AIRLINE_ID ASC ";

            
            response = db.readCommand(sqlCommand, "FLIGHT");
            return response;
        }

        [WebMethod]
        public DataSet getTicketsOfFlight(int flightID)
        {
            db = new DataBase();
            DataSet response = null;

            string sqlCommand = "SELECT * FROM [TicketReservationDB].[dbo].[TicketFlights] where FLIGHT_ID = @FLIGHTID ORDER BY Client ASC ";

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@FLIGHTID", SqlDbType.Int);
            param[0].Value = flightID;


            response = db.readCommand(sqlCommand, "TICKET",param);
            return response;
        }


        #endregion

        #region Staff

        [WebMethod]
        public int newStaff(string userName, string password, string email, string firstName, string lastName,int airlineID)
        {


            try
            {
                db = new DataBase();
                string sqlCommand = null;


                sqlCommand = "INSERT INTO STAFF(USERNAME,PSWD,EMAIL,FIRST_NAME,LAST_NAME,AIRLINE_ID) VALUES(@USERNAME,@PSWD, @EMAIL, @FIRSTNAME, @LASTNAME,@AIRLINE) ";
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@USERNAME", SqlDbType.VarChar, 20);
                param[0].Value = userName;

                param[1] = new SqlParameter("@PSWD", SqlDbType.VarChar, 40);
                param[1].Value = password;

                param[2] = new SqlParameter("@EMAIL", SqlDbType.VarChar, 100);
                param[2].Value = email;

                param[3] = new SqlParameter("@FIRSTNAME", SqlDbType.VarChar, 40);
                param[3].Value = firstName;

                param[4] = new SqlParameter("@LASTNAME", SqlDbType.VarChar, 40);
                param[4].Value = lastName;

                param[5] = new SqlParameter("@AIRLINE", SqlDbType.Int);
                param[5].Value = airlineID;


                if (db.executeCommand(sqlCommand, param))
                {
                    db = new DataBase();
                    DataSet response = null;

                    sqlCommand = "SELECT [STAFF_ID] FROM STAFF WHERE USERNAME=@USERNAME";
                    param = new SqlParameter[1];
                    param[0] = new SqlParameter("@USERNAME", SqlDbType.VarChar, 20);
                    param[0].Value = userName;

                    response = db.readCommand(sqlCommand, "STAFF", param);
                    if (response.Tables[0].Rows.Count > 0)
                    {
                        return int.Parse(response.Tables[0].Rows[0][0].ToString());
                    }
                    else
                    {
                        return -1;
                    }

                }
                else
                {
                    return -1;
                }

            }
            catch (Exception generatedExceptionName)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "newClient: " + generatedExceptionName.Message);
                return -1;
            }
        }

        [WebMethod]
        public DataSet getStaff(int userID)
        {
            db = new DataBase();
            DataSet response = null;


            string sqlCommand = "SELECT [CLIENT_ID],[USERNAME] ,[EMAIL],[FIRST_NAME],[LAST_NAME],[AIRLINE_ID] FROM STAFF WHERE STAFF_ID=@STAFFID";
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@STAFFID", SqlDbType.Int);
            param[0].Value = userID;

            response = db.readCommand(sqlCommand, "STAFF", param);


            return response;


        }

        [WebMethod]
        public DataSet getStaffofAirline(int airlineID)
        {
            db = new DataBase();
            DataSet response = null;

            string sqlCommand = "SELECT * FROM [TicketReservationDB].[dbo].[Staff] where AIRLINE_ID=@AIRLINEID";

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@AIRLINEID", SqlDbType.Int);
            param[0].Value = airlineID;

            response = db.readCommand(sqlCommand, "STAFF", param);
            return response;
        }

        [WebMethod]
        public DataSet getAllAirlines()
        {
            db = new DataBase();
            DataSet response = null;

            string sqlCommand = "SELECT * FROM AIRLINE";

            response = db.readCommand(sqlCommand, "AIRLINE");
            return response;
        }

        [WebMethod]
        public int validateStaffLogin(string username, string password)
        {
            db = new DataBase();
            DataSet response = null;


            string sqlCommand = "SELECT STAFF_ID FROM STAFF WHERE USERNAME = @USERNAME AND PSWD = @PASSWORD";
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@USERNAME", SqlDbType.VarChar, 20);
            param[0].Value = username;

            param[1] = new SqlParameter("@PASSWORD", SqlDbType.VarChar, 40);
            param[1].Value = password;
            response = db.readCommand(sqlCommand, "STAFF", param);

            if (response.Tables[0].Rows.Count > 0)
            {
                return int.Parse(response.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }

        }

        #endregion
    }
}
