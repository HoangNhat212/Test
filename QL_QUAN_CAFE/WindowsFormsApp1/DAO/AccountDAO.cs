﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.DTO;

namespace WindowsFormsApp1.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance 
        { 
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { AccountDAO.instance = value; }
        }

        private AccountDAO() { }

        public bool Login(string userName, string passWord)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }

            string query = "USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query,new object[]{userName, hasPass});

            return result.Rows.Count > 0;
        }

        public bool UpdateAccount(string userName, string displayName, string passWord, string newPass)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }

            byte[] temp1 = ASCIIEncoding.ASCII.GetBytes(newPass);
            byte[] hasData1 = new MD5CryptoServiceProvider().ComputeHash(temp1);

            string hasNewPass = "";

            foreach (byte item in hasData1)
            {
                hasNewPass += item;
            }

            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @userName , @displayName , @password , @newPassword", new object[] { userName, displayName, hasPass, hasNewPass });

            return result > 0;
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from account where userName = '" + userName + "'");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }
        public DataTable getListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT USERNAME,DISPLAYNAME,TYPE FROM ACCOUNT");
        }
        public bool InsertAccount(string userName, string displayName, int Type)
        {
            string query = string.Format("INSERT dbo.Account ( UserName, DisplayName, Type, Password )VALUES  ( N'{0}', N'{1}', {2}, N'{3}')", userName, displayName, Type, "1962026656160185351301320480154111117132155");
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateAccount(string userName, string displayName, int Type)
        {
            string query = string.Format("update ACCOUNT set DISPLAYNAME=N'{1}',TYPE={2} where USERNAME=N'{0}'", userName, displayName, Type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            string query = string.Format("Delete Account where USERNAME = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool ResetPassword(string Name)
        {
            string query = string.Format("update ACCOUNT set PASSWORD=N'1962026656160185351301320480154111117132155' where USERNAME=N'{0}'", Name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
