using System;
using System.Numerics;

using RLBotDotNet;
using KipjeBot;
using KipjeBot.Actions;
using KipjeBot.Plays;
using KipjeBot.Helpers;

using SystemVector2 = System.Numerics.Vector2;
using SystemVector3 = System.Numerics.Vector3;
using KipjeBot.Utility;

namespace DadBot
{
    public class DadBot : Bot
    {
        private GameInfo gameInfo;
        private bool clearRender = true;
        private int previousPlayCount = 0;

        public DadBot(string botName, int botTeam, int botIndex) : base(botName, botTeam, botIndex)
        {
            gameInfo = new GameInfo(botIndex, botTeam, botName);
            previousPlayCount = 0;
        }

        public override Controller GetOutput(rlbot.flat.GameTickPacket gameTickPacket)
        {
            if (clearRender)
            {
                clearRender = false;
                //Renderer.EraseFromScreen();
            }

            gameInfo.Renderer = Renderer;
            gameInfo.Update(gameTickPacket);
            var currentPlay = gameInfo.Playbook.CurrentPlay;
            if (currentPlay.Expired || !gameInfo.IsRoundActive)
            {
                IPlay[] book = { new Loading(), new KickOffPlay(), new ChaseBallPlay() };
                foreach (var play in book)
                {
                    if (play.Available(gameInfo))
                    {
                        gameInfo.Playbook.Setup(play);
                        break;
                    }
                }
            }

            if (name == "DadBot")
            {
                Renderer.DrawString2D("Play:" + gameInfo.Playbook.CurrentPlay.Type, ColorWheel.CyanAqua, new SystemVector2(0, 0), 2, 2);
                //Renderer.DrawString2D("Bot : " + gameInfo.MyCar.Position.Debug(), ColorWheel.White, new SystemVector2(0, 40), 2, 2);
                //Renderer.DrawString2D("DeltaTime : " + gameInfo.Time, ColorWheel.White, new SystemVector2(0, 0), 2, 2);
                //Renderer.DrawString2D("Controller : " + gameInfo.Playbook.Controller.Debug(), ColorWheel.White, new SystemVector2(0, 80), 1, 1);

            }

            gameInfo.Playbook.CurrentPlay.Execute(gameInfo);
            return gameInfo.Playbook.Controller;
        }
    }

    struct KickOffStruct
    {
        public Vector2 Position;
        public float Yaw;

        public KickOffStruct(Vector2 position, double yaw)
        {
            Position = position;
            Yaw = (float)yaw;
        }
    }
}
