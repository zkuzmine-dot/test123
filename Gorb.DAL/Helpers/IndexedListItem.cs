using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorb.DAL.Helpers
{
    internal class IndexedListItem
    {
        public int Index { get; set; }

        public string? Name { get; set; }

        public bool IsEven => Index % 2 == 0;
    }
}
