using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friend_s.Portable.Model;

namespace Friend_s.Portable.Design
{
    public class DesignDatabaseService : IDatabaseService
    {
        public async Task<List<TestItem>> GetTestItems()
        {
            List<TestItem> testItems = new List<TestItem>
            {
                new TestItem()
                {
                    Id=1,
                    Title = "Prajjwal Dimri",
                    Subtitle="7830207022"
                },
                new TestItem()
                {
                    Id=2,
                    Title = "Rajat Dimri",
                    Subtitle="7830207022"
                },
            };
            return testItems;
        }

    }
}
