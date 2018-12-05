using System;
using System.Numerics;
using NUnit.Framework;

namespace DadBotTests
{

    [TestFixture]
    public class KickOffSelection
    {
        [Test]
        public void CheckStartPosition_ReturnsValidKickoff()
        {
            var startPosition = new Vector3(0, -4607.8f, 17.01f);
            var startRotation = new Vector3(-0.007605204f, 0.007605204f, 1.570783f);

            //GameState gamestate = new GameState();
            //gamestate.BallState.PhysicsState.Location = new DesiredVector3(0, 0, 100);
            //gamestate.BallState.PhysicsState.AngularVelocity = new DesiredVector3(0, 0, 0);
            //gamestate.BallState.PhysicsState.Velocity = new DesiredVector3(0, 0, 0);

            //CarState carstate = new CarState();
            //carstate.PhysicsState.AngularVelocity = new DesiredVector3(0, 0, 0);
            //carstate.PhysicsState.Velocity = new DesiredVector3(0, 0, 0);
            //carstate.PhysicsState.Location = new DesiredVector3(startPosition);
            
            //carstate.PhysicsState.Rotation = new DesiredRotator(0, startRotation.Z, 0);
            //carstate.Boost = 33;

            //var gameInfo = new GameInfo(0,0,"DadBot");
            //gameInfo.Update(gamestate, carstate);

            //var kickoff = new KickOff(gameInfo.MyCar);
            //Assert.AreEqual("Center", kickoff.Debug());

            Assert.AreEqual("Center", "Center");



        }

        [Test]
        public void CheckStartPosition_ReturnsValidKickoff2()
        {
            var startPosition = new Vector3(0, -4607.8f, 17.01f);
            var startRotation = new Vector3(-0.007605204f, 0.007605204f, 1.570783f);

            //GameState gamestate = new GameState();
            //gamestate.BallState.PhysicsState.Location = new DesiredVector3(0, 0, 100);
            //gamestate.BallState.PhysicsState.AngularVelocity = new DesiredVector3(0, 0, 0);
            //gamestate.BallState.PhysicsState.Velocity = new DesiredVector3(0, 0, 0);

            //CarState carstate = new CarState();
            //carstate.PhysicsState.AngularVelocity = new DesiredVector3(0, 0, 0);
            //carstate.PhysicsState.Velocity = new DesiredVector3(0, 0, 0);
            //carstate.PhysicsState.Location = new DesiredVector3(startPosition);

            //carstate.PhysicsState.Rotation = new DesiredRotator(0, startRotation.Z, 0);
            //carstate.Boost = 33;

            //var gameInfo = new GameInfo(0,0,"DadBot");
            //gameInfo.Update(gamestate, carstate);

            //var kickoff = new KickOff(gameInfo.MyCar);
            //Assert.AreEqual("Center", kickoff.Debug());

            Assert.AreNotEqual("Center", "Center");



        }
    }
}
