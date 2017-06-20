using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FirmaTransportowa.Areas.BusRoutes
{
    public partial class RoutesList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RepeaterDataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            rptPages.ItemCommand +=
               new RepeaterCommandEventHandler(rptPages_ItemCommand);
        }

        protected void RepeaterDataBind()
        {
            using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TransportDB"].ConnectionString))
            {

                myConnection.Open();

                SqlDataAdapter myCommand;
                    myCommand = new SqlDataAdapter("SELECT Id, Name, Price, DepartDate FROM BusRoutes ORDER BY Id ASC", myConnection);

                DataTable dt = new DataTable();
                myCommand.Fill(dt);

                myConnection.Close();

                PagedDataSource pgItems = new PagedDataSource();
                DataView dv = new DataView(dt);

                pgItems.DataSource = dv;
                pgItems.AllowPaging = true;
                pgItems.PageSize = 10;
                pgItems.CurrentPageIndex = PageNumber;
                if (pgItems.PageCount > 1)
                {
                    rptPages.Visible = true;
                    ArrayList pages = new ArrayList();
                    for (int i = 0; i < pgItems.PageCount; i++)
                        pages.Add((i + 1).ToString());
                    rptPages.DataSource = pages;
                    rptPages.DataBind();
                }
                else
                    rptPages.Visible = false;

                rptRoutesList.DataSource = pgItems;
                rptRoutesList.DataBind();

                myConnection.Close();
            }
        }

        public int PageNumber
        {
            get
            {
                if (ViewState["PageNumber"] != null)
                    return Convert.ToInt32(ViewState["PageNumber"]);
                else
                    return 0;
            }
            set
            {
                ViewState["PageNumber"] = value;
            }
        }

        void rptPages_ItemCommand(object source,
                             RepeaterCommandEventArgs e)
        {
            PageNumber = Convert.ToInt32(e.CommandArgument) - 1;
            RepeaterDataBind();
        }

        protected void BtnCreate1_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreatePackage.aspx");
        }
    }
}