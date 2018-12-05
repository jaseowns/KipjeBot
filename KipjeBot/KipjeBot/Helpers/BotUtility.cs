using KipjeBot.Helpers;
using System;
using SystemVector2 = System.Numerics.Vector2;
using SystemVector3 = System.Numerics.Vector3;

namespace KipjeBot.Utility
{
    public static class BotUtility
    {
        public static double Quad(double a, double b, double c)
        {
            var inside = Math.Pow(b, 2) - (4 * a * c);
            if (inside < 0 || a == 0)
            {
                return 0.1;
            }
            var n = ((-b - Math.Sqrt(inside)) / (2 * a));
            var p = ((-b + Math.Sqrt(inside)) / (2 * a));

            if (p > n)
                return p;

            return n;
        }

        public static double TimeZ(Ball ball)
        {
            var rate = 0.97f;
            var check = -325;
            return Quad(check, ball.Velocity.Y * rate, ball.Position.Y - 92.75);
        }

        public static bool BallReady(GameInfo game)
        {
            if (Math.Abs(game.Ball.Velocity.Z) < 150 && TimeZ(game.Ball) < 1)
            {
                return true;
            }
            return false;
        }

        public static double BallProject(GameInfo game)
        {
            var goal = new SystemVector3(0, -MathUtility.Sign(game.MyCar.Team) * game.FieldHelper.FIELD_LENGTH / 2, 100);
            var goal_to_ball = (game.Ball.Position - goal).Normalize();
            var difference = game.MyCar.Position - game.Ball.Position;
            return MathUtility.MultiplyVector3(difference, goal_to_ball);
        }

        public static double Magnitude(SystemVector3 vector3)
        {
            return Math.Sqrt((vector3.X * vector3.X) + (vector3.Y * vector3.Y) + (vector3.Z * vector3.Z));
        }

        public static SystemVector3 Normalize(this SystemVector3 value)
        {
            var magnitude = Magnitude(value);
            if (magnitude != 0)
            {
                return new SystemVector3((float)(value.X / magnitude), (float)(value.Y / magnitude), (float)(value.Z / magnitude));
            }
            return new SystemVector3(0, 0, 0);
        }

        public static SystemVector3 ToLocal(SystemVector3 target, Car agent)
        {
            var difference = target - agent.Position;

            var x = Calculations.MultiplyVector3(difference, new SystemVector3(0,0,0));
            var y = Calculations.MultiplyVector3(difference, new SystemVector3(0, 0, 0));
            var z = Calculations.MultiplyVector3(difference, new SystemVector3(0, 0, 0));

            //var x = Calculations.MultiplyVector3(difference, agent.Matrix[0]);
            //var y = Calculations.MultiplyVector3(difference, agent.Matrix[1]);
            //var z = Calculations.MultiplyVector3(difference, agent.Matrix[2]);
            return new SystemVector3(x, y, z);
        }

    }
}
