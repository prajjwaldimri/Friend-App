using System.Collections.Generic;
using System.Threading.Tasks;
using Friend_s.Model;

namespace Friend_s.Design
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
