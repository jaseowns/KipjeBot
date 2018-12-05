using KipjeBot.Utility;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KipjeBot.Actions
{
    public class ChaseBall
    {
        private Dodge dodge;
        private Car car;
        private Ball ball;

        public ChaseBall(Car car, Ball ball)
        {
            this.car = car;
            this.ball = ball;
        }

        public Controller Step(float dt)
        {
            Controller controller = new Controller();

            double botToTargetAngle = Math.Atan2(ball.Position.Y - car.Position.Y, ball.Position.X - car.Position.X);
            double botFrontToTargetAngle = botToTargetAngle - car.Rotation.Z;
            // Correct the angle
            if (botFrontToTargetAngle < -Math.PI)
                botFrontToTargetAngle += 2 * Math.PI;
            if (botFrontToTargetAngle > Math.PI)
                botFrontToTargetAngle -= 2 * Math.PI;

            // Decide which way to steer in order to get to the ball.
            if (botFrontToTargetAngle > 0)
                controller.Steer = 1;
            else
                controller.Steer = -1;

            controller.Throttle = 1;
            return controller;
        }
    }
}
