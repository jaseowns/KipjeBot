using KipjeBot;
using KipjeBot.Actions;
using RLBotDotNet.GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KickOffTests
{
    class Program
    {
        static void Main(string[] args)
        {
          
            var startPosition = new Vector3(0, -4607.8f, 17.01f);
            var startRotation = new Vector3(-0.007605204f, 0.007605204f, 1.570783f);

            GameState gamestate = new GameState();
            gamestate.BallState.PhysicsState.Location = new DesiredVector3(0, 0, 100);
            gamestate.BallState.PhysicsState.AngularVelocity = new DesiredVector3(0, 0, 0);
            gamestate.BallState.PhysicsState.Velocity = new DesiredVector3(0, 0, 0);

            CarState carstate = new CarState();
            carstate.PhysicsState.AngularVelocity = new DesiredVector3(0, 0, 0);
            carstate.PhysicsState.Velocity = new DesiredVector3(0, 0, 0);
            carstate.PhysicsState.Location = new DesiredVector3(startPosition);

            carstate.PhysicsState.Rotation = new DesiredRotator(0, startRotation.Z, 0);
            carstate.Boost = 33;

            var gameInfo = new GameInfo(0, 0, "DadBot");
            gameInfo.Update(gamestate, carstate);

            var kickoff = new KickOff(gameInfo.MyCar);

            //Console.WriteLine(kickoff.Debug());

        }
    }
}
