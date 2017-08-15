using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMate.Common
{
    [Serializable]
    public class Command
    {
        public CommandType CommandType { get; set; }
        public String Content { get; set; }
    }
}
