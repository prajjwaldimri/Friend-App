using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLitePCL;

namespace Friend_s.Model
{
    class DatabaseService : IDatabaseService
    {
        SQLiteConnection conn = new SQLiteConnection("testdatabase.sqlite");
        Random rand = new Random();
        public async Task<List<TestItem>> GetTestItems()
        {
            List<TestItem> testItems = new List<TestItem>();
            using (var statement = conn.Prepare("SELECT * FROM TestItem"))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    testItems.Add(new TestItem()
                    {
                        Id = (int)statement[0],
                        Title = (string)statement[1],
                        Subtitle = (string)statement[2]
                        
                    });
                }
            }
            return testItems;
        }
    }
}
