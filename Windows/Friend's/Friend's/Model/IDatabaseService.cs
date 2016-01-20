using System.Collections.Generic;
using System.Threading.Tasks;

namespace Friend_s.Model
{
    public interface IDatabaseService
    {
        Task<List<TestItem>> GetTestItems();
    }
}
