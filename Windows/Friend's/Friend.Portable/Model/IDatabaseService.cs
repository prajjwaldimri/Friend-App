using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friend_s.Portable.Model
{
    public interface IDatabaseService
    {
        Task<List<TestItem>> GetTestItems();
    }
}
