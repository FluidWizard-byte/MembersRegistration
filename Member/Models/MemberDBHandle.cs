using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Helpers;

namespace Member.Models
{
    public class MemberDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["DBCS"].ToString();
            con = new SqlConnection(constring);
        }

        //################## ADD NEW MEMBER #####################//
        public bool AddMember(Members member)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewMember", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FullName", member.fullName);
            cmd.Parameters.AddWithValue("@Address", member.address);
            cmd.Parameters.AddWithValue("@MobileNumber", member.mobileNumber);
            cmd.Parameters.AddWithValue("@Email", member.email);
            cmd.Parameters.AddWithValue("@Gender", member.gender);
            cmd.Parameters.AddWithValue("@MemberType", member.memberTypeId);
            cmd.Parameters.AddWithValue("@EntryDate", member.entryDate);
            cmd.Parameters.AddWithValue("@ExpiryDate", member.expiryDate);
            cmd.Parameters.AddWithValue("@MemberFee", member.memberFee);
            cmd.Parameters.AddWithValue("@Remarks", member.remarks);
            cmd.Parameters.AddWithValue("@Attachment", member.attachment);
            cmd.Parameters.AddWithValue("@IsActive", member.isActive);
            cmd.Parameters.AddWithValue("@Image", member.image);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        //####################### LIST Members ###################//
        public List<Members> GetMembers()
        {
            connection();
            List<Members> memberList = new List<Members>();

            SqlCommand cmd = new SqlCommand("GetMemberDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                 int VmemberId = Convert.ToInt32(dr["Id"]);
                 string VfullName = Convert.ToString(dr["FullName"]);
                 string Vaddress = Convert.ToString(dr["Address"]);
                 string VmobileNumber;
                 try {  VmobileNumber = Convert.ToString(dr["MobileNumber"]); } catch {  VmobileNumber = ""; }
                 string Vemail;
                 try { Vemail = Convert.ToString(dr["Email"]); } catch { Vemail = ""; }
                 
                 string Vgender = Convert.ToString(dr["Gender"]);
                 int VmemberTypeId = Convert.ToInt32(dr["MemberType"]);

                
                 DateTime VentryDate = Convert.ToDateTime(dr["EntryDate"]);
                 DateTime VexpiryDate = Convert.ToDateTime(dr["ExpiryDate"]);

                int VmemberFee;
                try { VmemberFee = Convert.ToInt32(dr["MemberFee"]); } catch { VmemberFee = 0; }

                string Vremarks;
                try { Vremarks = Convert.ToString(dr["Remarks"]); } catch { Vremarks = ""; }

                string Vattachment;
                try { Vattachment = Convert.ToString(dr["Attachment"]); } catch { Vattachment = ""; }
                  
                Boolean VisActive = Convert.ToBoolean(dr["IsActive"]);

                string Vimage;
                try { Vimage = Convert.ToString(dr["Image"]); } catch { Vimage = ""; }
                
                memberList.Add(
                    new Members
                    {
                        memberId = VmemberId,
                        fullName = VfullName,
                        address = Vaddress,
                        mobileNumber = VmobileNumber,
                        email = Vemail,
                        gender = Vgender,
                        memberTypeId = VmemberTypeId,
                        entryDate = VentryDate,
                        expiryDate = VexpiryDate,
                        memberFee = VmemberFee,
                        remarks = Vremarks,
                        attachment = Vattachment,
                        isActive = VisActive,
                        image= Vimage
                    });
            }
            return memberList;
        }

        //##################### Update Member #################//
        public bool UpdateDetails(Members member)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateMemberDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MId", member.memberId);
            cmd.Parameters.AddWithValue("@FullName", member.fullName);
            cmd.Parameters.AddWithValue("@Address", member.address);
            cmd.Parameters.AddWithValue("@MobileNumber", member.mobileNumber);
            cmd.Parameters.AddWithValue("@Email", member.email);
            cmd.Parameters.AddWithValue("@Gender", member.gender);
            cmd.Parameters.AddWithValue("@MemberType", member.memberTypeId);
            cmd.Parameters.AddWithValue("@EntryDate", member.entryDate);
            cmd.Parameters.AddWithValue("@ExpiryDate", member.expiryDate);
            cmd.Parameters.AddWithValue("@MemberFee", member.memberFee);
            cmd.Parameters.AddWithValue("@Remarks", member.remarks);
            cmd.Parameters.AddWithValue("@Attachment", member.attachment);
            cmd.Parameters.AddWithValue("@IsActive", member.isActive);
            cmd.Parameters.AddWithValue("@Image", member.image);


            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
        //################## DELETE MEMBER DETAILS ###################
        public bool DeleteMember(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteMember", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MId", id);

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