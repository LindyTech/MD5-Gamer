using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5_Pro_Gramer
{
    class DB
    {
        public void fnc_ConnectoToDB(ref SqlConnection cn)
        {
            string strConnectionString = null;

            strConnectionString = @"Data Source=AHSANMUGHAL;Initial Catalog=SurveyBuilder;User ID=sa;Password=ahsan";

            cn = new SqlConnection();
            cn.ConnectionString = strConnectionString;
            cn.Open();
        }

        public void fnc_CloseCn(ref SqlConnection cn)
        {
            if (cn.State == ConnectionState.Open == true)
            {
                cn.Close();
            }
        }
    }
}
