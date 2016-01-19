using System;
using System.Collections.Generic;
using System.Diagnostics;
using SQLitePCL;

namespace Friend_s
{
    public static class CreateDatabase
    {
        public static void CreateTable()
        {
            using (var connection = new SQLiteConnection("Main.db"))
            {
                using (
                    var statement =
                        connection.Prepare(@"CREATE TABLE IF NOT EXISTS Details(ID NVARCHAR(1) Primary Key, Name NVARCHAR(20), Number NVARCHAR(15));"))
                {
                    statement.Step();
                }
            }
        }

        public static void InsertData(string id, string name, string number)
        {
            try
            {
                using (var connection = new SQLiteConnection("Main.db"))
                {
                    using (var statement = connection.Prepare(@"INSERT INTO Details (ID,Name,Number) VALUES(?,?,?);"))
                    {
                        statement.Bind(1,id);
                        statement.Bind(2,name);
                        statement.Bind(3,number);
                        statement.Step();
                        statement.Reset();
                        statement.ClearBindings();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception\n" + e);
            }
        }

        public static void UpdateData(string id, string name, string number)
        {
            try
            {
                using (var connection = new SQLiteConnection("Main.db"))
                {
                    using (var statement = connection.Prepare(@"UPDATE Details SET Name=?, Number=? WHERE ID=?;"))
                    {
                        statement.Bind(1, name);
                        statement.Bind(2, number);
                        statement.Bind(3, id);
                        statement.Step();
                        statement.Reset();
                        statement.ClearBindings();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception\n" + e);
            }
        }
        public static string[] GetValues(string sqlstatement)
        {
            List<String> list = new List<string>();
            using (var connection = new SQLiteConnection("Main.db"))
            {
                using (var statement = connection.Prepare(sqlstatement))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        list.Add(statement[0].ToString());

                    }
                    Debug.WriteLine(statement[0]);
                }
            }
            return list.ToArray();
        }

        public static void Delete(string id)
        {
            using (var connection = new SQLiteConnection("Main.db"))
            {
                using (var statement = connection.Prepare(@"DELETE FROM Details WHERE ID=?"))
                {
                    statement.Bind(1,id);
                    statement.Step();
                }
            }
        } 
    }
}