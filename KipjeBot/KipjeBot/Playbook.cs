using KipjeBot.Plays;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KipjeBot
{
    public class Playbook
    {
        public IPlay PreviousPlay { get; private set; }
        public IPlay CurrentPlay { get; private set; }
        public Controller Controller { get; private set; }
        public float CurrentPlayTimer { get; private set; }

    public Playbook()
        {
            CurrentPlayTimer = 0;
            CurrentPlay = new Loading();
            PreviousPlay = new Loading();
            Setup(new StartPlaybook());
        }

        public void Setup(IPlay play)
        {
            if (play.Type != CurrentPlay.Type)
            {
                PreviousPlay = CurrentPlay;
                CurrentPlay = play;
            }
        }

        public void Setup(Controller controller)
        {
            CurrentPlayTimer = 0;
            Controller = controller;
        }

        public void Step(float dt)
        {
            CurrentPlayTimer += dt;
        }

        public bool IsPlayStuck(float stuckCheck)
        {
            if (CurrentPlayTimer >= stuckCheck)
            {
                Setup(new ChaseBallPlay());
                return true;
            }
            return false;            
        }
    }
}
