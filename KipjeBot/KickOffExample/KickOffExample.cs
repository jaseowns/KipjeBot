using System;
using System.Numerics;

using RLBotDotNet;
using RLBotDotNet.GameState;

using KipjeBot;
using KipjeBot.Actions;

namespace KickOffExample
{
    public class KickOffExample : Bot
    {
        private GameInfo gameInfo;

        private float timeout = 0;
        private int runKickoff = 0;

        private KickOff kickoff;

        private KickOffStruct Team0KickOffFrontCorner1 = new KickOffStruct(new Vector2(Team0KickOffPosition.FrontCorner1.X, Team0KickOffPosition.FrontCorner1.Y), 0.75 * Math.PI);
        private KickOffStruct Team1KickOffFrontCorner1 = new KickOffStruct(new Vector2(Team1KickOffPosition.FrontCorner1.X, Team1KickOffPosition.FrontCorner1.Y), -0.75 * Math.PI);

        private KickOffStruct Team0KickOffFrontCorner2 = new KickOffStruct(new Vector2(Team0KickOffPosition.FrontCorner2.X, Team0KickOffPosition.FrontCorner2.Y), 0.75 * Math.PI);
        private KickOffStruct Team1KickOffFrontCorner2 = new KickOffStruct(new Vector2(Team1KickOffPosition.FrontCorner2.X, Team1KickOffPosition.FrontCorner2.Y), -0.75 * Math.PI);

        private KickOffStruct Team0KickOffFrontCorner3 = new KickOffStruct(new Vector2(Team0KickOffPosition.FrontCorner3.X, Team0KickOffPosition.FrontCorner3.Y), 0.75 * Math.PI);
        private KickOffStruct Team1KickOffFrontCorner3 = new KickOffStruct(new Vector2(Team1KickOffPosition.FrontCorner3.X, Team1KickOffPosition.FrontCorner3.Y), -0.75 * Math.PI);

        private KickOffStruct Team0KickOffFrontCorner4 = new KickOffStruct(new Vector2(Team0KickOffPosition.FrontCorner4.X, Team0KickOffPosition.FrontCorner4.Y), 0.75 * Math.PI);
        private KickOffStruct Team1KickOffFrontCorner4 = new KickOffStruct(new Vector2(Team1KickOffPosition.FrontCorner4.X, Team1KickOffPosition.FrontCorner4.Y), -0.75 * Math.PI);

        private KickOffStruct Team0KickOffFrontCorner5 = new KickOffStruct(new Vector2(Team0KickOffPosition.FrontCorner5.X, Team0KickOffPosition.FrontCorner5.Y), 0.75 * Math.PI);
        private KickOffStruct Team1KickOffFrontCorner5 = new KickOffStruct(new Vector2(Team1KickOffPosition.FrontCorner5.X, Team1KickOffPosition.FrontCorner5.Y), -0.75 * Math.PI);

        private KickOffStruct Team0KickOffFrontCorner6 = new KickOffStruct(new Vector2(Team0KickOffPosition.FrontCorner6.X, Team0KickOffPosition.FrontCorner6.Y), 0.75 * Math.PI);
        private KickOffStruct Team1KickOffFrontCorner6 = new KickOffStruct(new Vector2(Team1KickOffPosition.FrontCorner6.X, Team1KickOffPosition.FrontCorner6.Y), -0.75 * Math.PI);

        private KickOffStruct Team0KickOffBackCorner1 = new KickOffStruct(new Vector2(Team0KickOffPosition.BackCorner1.X, Team0KickOffPosition.BackCorner1.Y), 0.50 * Math.PI);
        private KickOffStruct Team1KickOffBackCorner1 = new KickOffStruct(new Vector2(Team1KickOffPosition.BackCorner1.X, Team1KickOffPosition.BackCorner1.Y), -0.50 * Math.PI);

        private KickOffStruct Team0KickOffBackCorner2 = new KickOffStruct(new Vector2(Team0KickOffPosition.BackCorner2.X, Team0KickOffPosition.BackCorner2.Y), 0.50 * Math.PI);
        private KickOffStruct Team1KickOffBackCorner2 = new KickOffStruct(new Vector2(Team1KickOffPosition.BackCorner2.X, Team1KickOffPosition.BackCorner2.Y), -0.50 * Math.PI);

        private KickOffStruct Team0KickOffCenter = new KickOffStruct(new Vector2(Team0KickOffPosition.Center.X, Team0KickOffPosition.Center.Y), 0.50 * Math.PI);
        private KickOffStruct Team1KickOffCenter = new KickOffStruct(new Vector2(Team1KickOffPosition.Center.X, Team1KickOffPosition.Center.Y), -0.50 * Math.PI);

        private KickOffStruct kickOffCenter = new KickOffStruct(new Vector2(0, 4608), -0.5 * Math.PI);
        private KickOffStruct kickOffBackCorner = new KickOffStruct(new Vector2(256, 3840), -0.5 * Math.PI);
        

        public KickOffExample(string botName, int botTeam, int botIndex) : base(botName, botTeam, botIndex)
        {
            gameInfo = new GameInfo(botIndex, botTeam, botName);
        }

        public override Controller GetOutput(rlbot.flat.GameTickPacket gameTickPacket)
        {
            gameInfo.Update(gameTickPacket);

            if (timeout > 6)
            {

                KickOffStruct k = kickOffCenter;
                runKickoff = 0;
                switch (runKickoff)
                {
                    case -2:
                        k = gameInfo.MyCar.Team == 0 ? Team0KickOffBackCorner2 : Team0KickOffBackCorner2;
                        break;
                    case -1:
                        k = gameInfo.MyCar.Team == 0 ? Team0KickOffBackCorner1 : Team0KickOffBackCorner1;
                        break;

                    case 0:
                        k = gameInfo.MyCar.Team == 0 ? Team0KickOffCenter : Team1KickOffCenter;
                        break;
                    case 1:
                        k = gameInfo.MyCar.Team == 0 ? Team0KickOffFrontCorner1 : Team1KickOffFrontCorner1;
                        break;
                    case 2:
                        k = gameInfo.MyCar.Team == 0 ? Team0KickOffFrontCorner2 : Team1KickOffFrontCorner2;
                        break;
                    case 3:
                        k = gameInfo.MyCar.Team == 0 ? Team0KickOffFrontCorner3 : Team1KickOffFrontCorner3;
                        break;
                    case 4:
                        k = gameInfo.MyCar.Team == 0 ? Team0KickOffFrontCorner4 : Team1KickOffFrontCorner4;
                        break;
                    case 5:
                        k = gameInfo.MyCar.Team == 0 ? Team0KickOffFrontCorner5 : Team1KickOffFrontCorner5;
                        break;
                    case 6:
                        k = gameInfo.MyCar.Team == 0 ? Team0KickOffFrontCorner6 : Team1KickOffFrontCorner6;
                        runKickoff = 0;
                        break;
                }

               

                

                GameState gamestate = new GameState();
                gamestate.BallState.PhysicsState.Location = new DesiredVector3(0, 0, 100);
                gamestate.BallState.PhysicsState.AngularVelocity = new DesiredVector3(0, 0, 0);
                gamestate.BallState.PhysicsState.Velocity = new DesiredVector3(0, 0, 0);

                CarState carstate = new CarState();
                carstate.PhysicsState.AngularVelocity = new DesiredVector3(0, 0, 0);
                carstate.PhysicsState.Velocity = new DesiredVector3(0, 0, 0);
                carstate.PhysicsState.Location = new DesiredVector3(new Vector3(k.Position, 17));
                carstate.PhysicsState.Rotation = new DesiredRotator(0, k.Yaw, 0);
                carstate.Boost = 33;

                gamestate.SetCarState(index, carstate);

                SetGameState(gamestate);

                timeout = 0;
                kickoff = null;
            }

            timeout += gameInfo.DeltaTime;

            Controller controller = new Controller();

            if (timeout > 2)
            {
                if (kickoff == null)
                    kickoff = new KickOff(gameInfo.Cars[index]);

                controller = kickoff.Step(0.0166667f);
            }

            return controller;
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
