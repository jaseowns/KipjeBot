using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KipjeBot.Plays
{
    public class Loading : BasePlay
    {
        public Loading()
        {
            Expired = true;
            Type = "Loading";
        }
    }

    public class StartPlaybook : BasePlay
    {
        public StartPlaybook()
        {
            Expired = true;
            Type = "StartPlaybook";
        }
    }
}
