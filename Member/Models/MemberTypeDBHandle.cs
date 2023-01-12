using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Member.Models
{
    public class MemberTypeDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["DBCS"].ToString();
            con = new SqlConnection(constring);
        }

        //################## ADD NEW MEMBER TYPE #####################//
        public bool AddMemberType(MemberType memberType)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewMemberType", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Type", memberType.type);
           

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        //####################### LIST MemberTypes ###################//
        public List<MemberType> GetMemberTypes()
        {
            connection();
            List<MemberType> memberTypeList = new List<MemberType>();

            SqlCommand cmd = new SqlCommand("GetMemberTypeDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                memberTypeList.Add(
                    new MemberType
                    {
                        memberTypeId = Convert.ToInt32(dr["Id"]),
                        type = Convert.ToString(dr["Type"])
                    });
            }
            return memberTypeList;
        }

        //##################### Update MemberType #################//
        public bool UpdateDetails(MemberType memberType)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateMemberTypeDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MTypeId", memberType.memberTypeId);
            cmd.Parameters.AddWithValue("@Type", memberType.type);
          

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        //################## DELETE MEMBER TYPE DETAILS ###################
        public bool DeleteMemberType(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteMemberType", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MTypeId", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
    }
}