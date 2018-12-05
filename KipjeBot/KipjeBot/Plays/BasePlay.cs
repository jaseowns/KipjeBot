using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KipjeBot.Plays
{
    public interface IPlay
    {
        string Type { get; set; }
        bool Available(GameInfo game);
        GameInfo Execute(GameInfo game);
        bool Expired { get; set; }
        long? Ticks { get; set; }
        TimeSpan Duration { get; }
        Vector3 Target { get; set; }
    }

    public abstract class BasePlay : IPlay
    {
        public string Type { get; set; }

        public BasePlay()
        {
            
        }

        private bool _expired = false;
        public bool Expired { get { return (_expired) || (Ticks.HasValue && Duration.Milliseconds >= 250); } set { Ticks = null; _expired = value; } }
        public long? Ticks { get; set; }
        public TimeSpan Duration => Ticks.HasValue ? new TimeSpan(Ticks.Value) : new TimeSpan();
        public Vector3 Target { get; set; }

        public virtual bool Available(GameInfo game)
        {
            return false;
        }

        public virtual GameInfo Execute(GameInfo game)
        {
            if (!Ticks.HasValue)
            {
                Ticks = DateTime.UtcNow.Ticks;
            }
            game.Update(this);
            return game;
        }
    }
}
