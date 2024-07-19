using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_.FileSystem
{
    public class UserAccount
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime BoD { get; set; }
        public int Age { get; set; }
        public bool VIP { get; set; }
        public decimal Points { get; set; }
    }
}
