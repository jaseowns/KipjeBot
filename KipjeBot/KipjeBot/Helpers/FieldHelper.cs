using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KipjeBot.Helpers
{
    public class FieldHelper
    {
        public int GOAL_WIDTH = 1900;
        public int FIELD_LENGTH = 10280;
        public int FIELD_WIDTH = 8240;
        public Vector3[] BOOSTS = {
            new Vector3(3584, 0, 0),
            new Vector3(-3584, 0, 0),
            new Vector3(3072, 4096, 0),
            new Vector3(3072, 4096, 0)
        };
    }
}
