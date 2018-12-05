using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KipjeBot.Utility;
using rlbot.flat;
using SystemVector3 = System.Numerics.Vector3;

namespace KipjeBot.Helpers
{
    public static class Calculations
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


        public static double Angle2(SystemVector3 targetLocation, SystemVector3 objectLocation)
        {
            var difference = targetLocation - objectLocation;
            return Math.Atan2(difference.Y, difference.X);
        }


        public static double Distance2D(SystemVector3 target, SystemVector3 agent)
        {
            var difference = target - agent;
            return Math.Sqrt(Math.Pow(difference.X, 2) + Math.Pow(difference.Y, 2));
        }



        public static double Get2DDistance(double x1, double x2, double y1, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public static double Velocity2D(Vector3 target)
        {
            return Math.Sqrt(Math.Pow(target.X, 2) + Math.Pow(target.Y, 2));
        }

        public static double Velocity2D(SystemVector3 target)
        {
            return Math.Sqrt(Math.Pow(target.X, 2) + Math.Pow(target.Y, 2));
        }

        public static float MultiplyVector3(SystemVector3 a, SystemVector3 b)
        {
            return (a.X * b.X + a.X * b.Y + a.Y * b.Y);
        }

        public static SystemVector3 Future(Ball ball, int time)
        {
            var x = ball.Position.X + (ball.Velocity.X * time);
            var y = ball.Position.Y + (ball.Velocity.Y * time);
            var z = ball.Position.Z + (ball.Velocity.Z * time);
            return new SystemVector3(x, y, z);
        }

        public static double dpp(SystemVector3 target_loc, SystemVector3 target_vel, SystemVector3 our_loc, SystemVector3 our_vel)
        {
            var d = Distance2D(target_loc, our_loc);

            if (d != 0)
                return (((target_loc.X - our_loc.X) * (target_vel.X - our_vel.X)) + ((target_loc.Y - our_loc.Y) * (target_vel.Y - our_vel.Y))) / d;
            else
                return 0;
        }

        public static float Steer(double angle)
        {
            var final = (Math.Pow((10 * angle + MathUtility.Sign(angle)), 3)) / 20;
            return (float)MathUtility.Clip(final, -1, 1);
        }
    }
}
