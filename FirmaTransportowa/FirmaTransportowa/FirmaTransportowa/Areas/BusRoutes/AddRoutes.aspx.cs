using DomainModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FirmaTransportowa.Views.Aspx
{
    public partial class AddRoutes : System.Web.UI.Page
    {
        private List<BusStop> _stops { get; set; } = new List<BusStop>();
        private List<int> _stopsToDelID { get; set; } = new List<int>();
        private List<int> _stopsToEditID { get; set; } = new List<int>();

        private int _tempID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _tempID = -1;
                hfId.Value = Request.QueryString["routeID"];

                if (!hfId.Value.Equals(""))
                    GetPackageInfo();

                RepeaterDataBind();
            }
            else if (ViewState["_stopsViewState"] != null)
            {
                _stops = (List<BusStop>)ViewState["_stopsViewState"];
                _stopsToDelID = (List<int>)ViewState["_stopsToDel"];
                _stopsToEditID = (List<int>)ViewState["_stopsToEdit"];
                _tempID = (int)ViewState["_tempID"];
            }
        }

        void GetPackageInfo()
        {
            using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TransportDB"].ConnectionString))
            {
               myConnection.Open();

                using (SqlCommand myCommand = new SqlCommand("SELECT Name, Price, DepartHoursSimple FROM BusRoutes WHERE ID=" + hfId.Value, myConnection))
                {
                    SqlDataReader reader = myCommand.ExecuteReader();


                    while (reader.Read())
                    {
                        txtRouteName.Text = reader.GetString(0);
                        txtRoutePrice.Text = Convert.ToString(reader.GetDecimal(1));
                        //var date = DateTime.ParseExact(Convert.ToString(reader.GetDateTime(2)),"dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        textRouteDate.Text = (reader.GetString(2));//.ToString("dd/MM/yyyy H:mm");
                    }
                }

                myConnection.Close();
            }
        }

        void Page_PreRender(object sender, EventArgs e)
        {
            ViewState.Add("_stopsViewState", _stops);
            ViewState.Add("_stopsToDel", _stopsToDelID);
            ViewState.Add("_stopsToEdit", _stopsToEditID);
            ViewState.Add("_tempID", _tempID);
        }

        protected void RepeaterDataBind()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            dt.Columns.Add("Id", Type.GetType("System.Int32"));
            dt.Columns.Add("FirstStop", Type.GetType("System.String"));
            dt.Columns.Add("LastStop", Type.GetType("System.String"));
            dt.Columns.Add("Price", Type.GetType("System.Decimal"));

            foreach (BusStop stops in _stops)
            {
                dt.Rows.Add(stops.Id, stops.FirstStop, stops.LastStop, stops.Price);
            }

            if (!IsPostBack)
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TransportDB"].ConnectionString))
                {
                    myConnection.Open();

                    if (!hfId.Value.Equals(""))
                    {
                        using (SqlDataAdapter myCommand = new SqlDataAdapter("SELECT Id, FirstStop, LastStop, Price FROM BusStops WHERE BusRoute_Id=" + hfId.Value, myConnection))
                        {
                            myCommand.Fill(dt);

                            foreach (DataRow _stop in dt.Rows)
                            {
                                BusStop _tempStop = new BusStop();
                                _tempStop.Id = Convert.ToInt32(_stop.ItemArray[0]);
                                _tempStop.FirstStop = Convert.ToString(_stop.ItemArray[1]);
                                _tempStop.LastStop = Convert.ToString(_stop.ItemArray[2]);
                                _tempStop.Price = Convert.ToDecimal(_stop.ItemArray[3]);

                                _stops.Add(_tempStop);
                            }
                        }
                 
                    }
                    myConnection.Close();
                }
            }

            ds.Tables.Add(dt);

            ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns["Id"] };

            foreach (var stopDel in _stopsToDelID)
            {
                ds.Tables[0].Rows.Find(stopDel).Delete();
            }

            rptStopsList.DataSource = ds;

            rptStopsList.DataBind();
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TransportDB"].ConnectionString))
            {
                connection.Open();

                string sqlCommandText;

                SqlCommand sqlCommand = connection.CreateCommand();
                SqlTransaction transaction;

                transaction = connection.BeginTransaction("AddRouteTransaction");

                sqlCommand.Connection = connection;
                sqlCommand.Transaction = transaction;

                try
                {
                    if (hfId.Value.Equals(""))
                        sqlCommandText = "INSERT INTO BusRoutes(Name, Price, DepartHoursSimple) VALUES (@RouteName, @RoutePrice, @RouteDepartHours);" + "Select Scope_Identity();";
                    else
                        sqlCommandText = "UPDATE BusRoutes SET Name=@RouteName, Price=@RoutePrice, DepartHoursSimple=@RouteDepartHours WHERE Id=@Id";

                    sqlCommand.CommandText = sqlCommandText;

                    sqlCommand.Parameters.AddWithValue("@RouteName", txtRouteName.Text);
                    sqlCommand.Parameters.AddWithValue("@RoutePrice", Convert.ToDecimal(txtRoutePrice.Text));
                    sqlCommand.Parameters.AddWithValue("@RouteDepartHours", textRouteDate.Text);
                    //DateTime.ParseExact(, "dd-MM-yyyy H:mm", CultureInfo.InvariantCulture)

                    if (!hfId.Value.Equals(""))
                    {
                        sqlCommand.Parameters.AddWithValue("@Id", Convert.ToInt32(hfId.Value));
                        sqlCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        hfId.Value = Convert.ToString(sqlCommand.ExecuteScalar());                        
                    }

                    sqlCommand.Parameters.Clear();

                    AddHours(connection, transaction);
                    SaveNewStops(connection, transaction);
                    EditStops(connection, transaction);
                    DeleteStops(connection, transaction);

                    transaction.Commit();
                    sqlCommand.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }

                }
                connection.Close();
            }
        }

        protected void AddHours(SqlConnection _connection, SqlTransaction _transaction)
        {
            string sqlCommandText =
                    "INSERT INTO DepartDates (DDate, BusRoute_ID) VALUES (@DDateVal, @RouteID)";

            string[] _hours = (textRouteDate.Text).Split(',');

            //zgnije za to w piekle ale nie mam juz czasu
            DelHours(_connection, _transaction);

            using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, _connection, _transaction))
            {
                foreach (string _hour in _hours)
                {
                    var _date = Convert.ToString(DateTime.Today);
                    var _today = _date.Split(' ');
                    string _finalDate = _today[0] + " " + _hour;
                    sqlCommand.Parameters.AddWithValue("@DDateVal", DateTime.ParseExact(_finalDate, "yyyy-MM-dd H:mm", CultureInfo.InvariantCulture));
                    sqlCommand.Parameters.AddWithValue("@RouteID", hfId.Value);

                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Parameters.Clear();
                }
            }
        }

        protected void DelHours(SqlConnection _connection, SqlTransaction _transaction)
        {
            string sqlCommandText =
                   "DELETE FROM DepartDates WHERE BusRoute_Id=" + hfId.Value;

            using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, _connection, _transaction))
            {
                sqlCommand.ExecuteNonQuery();
            }
        }

        protected void SaveNewStops(SqlConnection _connection, SqlTransaction _transaction)
        {
            string sqlCommandText =
                    "INSERT INTO BusStops (FirstStop, LastStop, Price, BusRoute_ID) VALUES (@BusFirstStop, @BusLastStop, @BusPrice, @RouteID)";

            using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, _connection, _transaction))
            {
                foreach (BusStop _stop in _stops)
                {
                    if (_stop.Id < 0)
                    {
                        sqlCommand.Parameters.AddWithValue("@BusFirstStop", _stop.FirstStop);
                        sqlCommand.Parameters.AddWithValue("@BusLastStop", _stop.LastStop);
                        sqlCommand.Parameters.AddWithValue("@BusPrice", _stop.Price);
                        sqlCommand.Parameters.AddWithValue("@RouteID", hfId.Value);

                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Parameters.Clear();
                    }
                }

            }
        }

        protected void EditStops(SqlConnection _connection, SqlTransaction _transaction)
        {
            string sqlCommandText =
                    "UPDATE BusStops SET FirstStop=@BusFirstStop, LastStop=@BusLastStop, Price=@BusPrice WHERE Id=@Id";

            using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, _connection, _transaction))
            {
                foreach (var editID in _stopsToEditID)
                {
                    var _temp = _stops.Find(x => x.Id == Convert.ToInt32(editID));
                    sqlCommand.Parameters.AddWithValue("@Id", editID);
                    sqlCommand.Parameters.AddWithValue("@BusFirstStop", _temp.FirstStop);
                    sqlCommand.Parameters.AddWithValue("@BusLastStop", _temp.LastStop);
                    sqlCommand.Parameters.AddWithValue("@BusPrice", _temp.Price);

                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Parameters.Clear();
                }
            }              
        }

        protected void DeleteStops(SqlConnection _connection, SqlTransaction _transaction)
        {
            string sqlCommandText = "DELETE FROM BusStops WHERE Id=@Id";

            using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, _connection, _transaction))
            {
                foreach (var delID in _stopsToDelID)
                {
                    sqlCommand.Parameters.AddWithValue("@Id", delID);
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Parameters.Clear();
                }
            }            
        }

        protected void btnSaveLeave_OnClick(object sender, EventArgs e)
        {
            btnSave_OnClick(sender, e);
            Response.Redirect("RoutesList.aspx");
        }

        protected void btnBack_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("RoutesList.aspx");
        }

        protected void btnCreateConsign_OnClick(object sender, EventArgs e)
        {
            BusStop _stop = new BusStop();

            _stop.FirstStop = txtStartStop.Text;
            _stop.LastStop = txtEndStop.Text;

            if (txtStopPrice.Text != "")
                _stop.Price = Convert.ToDecimal(txtStopPrice.Text);
            else
                _stop.Price = 0;

            _stop.Id = _tempID;
            _tempID--;

            _stops.Add(_stop);

            RepeaterDataBind();
        }

        protected void btnDeleteConsign_OnClick(object sender, EventArgs e)
        {
            var _stopID = Convert.ToInt32(((Button)sender).CommandArgument);

            if (_stopID > 0)
                _stopsToDelID.Add(_stopID);

            else if (_stopID < 0)
            {
                var toRemove = _stops.Find(x => x.Id == _stopID);
                _stops.Remove(toRemove);
            }

            RepeaterDataBind();
        }

        protected void rptConsign_textChng(object sender, EventArgs e)
        {
            var _hfControl = ((TextBox)sender).Parent.FindControl("hfRptStopId");
            var _stopID = Convert.ToInt32(((HiddenField)_hfControl).Value);

            string _senderControl = ((TextBox)sender).ID.ToString();
            var _value = ((TextBox)sender).Text;

            var _temp = _stops.Find(x => x.Id == _stopID);

            switch (_senderControl)
            {
                case "rptStartStop": { _temp.FirstStop = _value; break; }
                case "rptEndStop": { _temp.LastStop = _value; break; }
                case "rptStopPrice": { _temp.Price = Convert.ToDecimal(_value); break; }
                default: break;
            }

            if (!_stopsToEditID.Contains(_stopID))
                _stopsToEditID.Add(_stopID);
        }
    }
}