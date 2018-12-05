using FlatBuffers;
using rlbot.flat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KipjeBot.GameTickPacket
{
    public class GameTickPacketTest
    {
        public int TileInformationLength { get; }
        public BallInfo? Ball { get; }
        public int BoostPadStatesLength { get; }
        public int PlayersLength { get; }
        public ByteBuffer ByteBuffer { get; }
        public rlbot.flat.GameInfo? GameInfo { get; }
    }
}
