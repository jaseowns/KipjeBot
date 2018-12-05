using System;
using System.Numerics;

using RLBotDotNet;

using KipjeBot.Utility;
using KipjeBot.Helpers;

namespace KipjeBot.Actions
{

    // TODO: Clean this shit up
    // Team 0 Kick offs
    public static class Team0KickOffPosition
    {
        public static Vector3 FrontCorner1 = new Vector3(1952, -2464, 17);
        public static Vector3 FrontCorner2 = new Vector3(1452, -2140, 17);
        public static Vector3 FrontCorner3 = new Vector3(-1952, -2464, 17);
        public static Vector3 FrontCorner4 = new Vector3(-1452, -2140, 17);
        public static Vector3 FrontCorner5 = new Vector3(2047, -2560, 17);
        public static Vector3 FrontCorner6 = new Vector3(-2047, -2560, 17);
        public static Vector3 BackCorner1 = new Vector3(256, -3840, 17);
        public static Vector3 BackCorner2 = new Vector3(-256, -3840, 17);
        public static Vector3 Center = new Vector3(0, -4608, 17);
    }

    // Team 1 Kick offs
    public static class Team1KickOffPosition
    {
        public static Vector3 FrontCorner1 = new Vector3(1952, 2464, 17);
        public static Vector3 FrontCorner2 = new Vector3(1452, 2140, 17);
        public static Vector3 FrontCorner3 = new Vector3(-1952, 2464, 17);
        public static Vector3 FrontCorner4 = new Vector3(-1452, 2140, 17);
        public static Vector3 FrontCorner5 = new Vector3(-2047, 2560, 17);
        public static Vector3 FrontCorner6 = new Vector3(2047, 2560, 17);
        public static Vector3 BackCorner1 = new Vector3(256, 3840, 17);
        public static Vector3 BackCorner2 = new Vector3(-256, 3840, 17);
        public static Vector3 Center = new Vector3(0, 4608, 17);
    }

    public enum KickOffPositions {
        FrontCorner1, FrontCorner2, FrontCorner3, FrontCorner4, FrontCorner5, FrontCorner6, BackCorner1, BackCorner2, Center, Unknown
    };

    public class KickOff
    {
        private Dodge dodge;
        private SideDodge sideDodge;
        private Car car;
        private Vector3 position;

        private KickOffPositions kickOffPosition;


        public string RenderDebug()
        {
            return car.Name + " " + kickOffPosition.ToString();
        }

        public string Debug()
        {
            return car.Position.Debug() + " " + car.Name + " " + kickOffPosition.ToString();
            if (car.Team == 0)
            {
                return car.Position.Debug() + " " + car.Team + " " + kickOffPosition.ToString();
            }

            return car.Team + " " + kickOffPosition.ToString();
        }

        public KickOff(Car car)
        {
            this.car = car;
            kickOffPosition = GetKickOffPosition(car.Position, car.Team);
            if (kickOffPosition == KickOffPositions.Unknown)
            {
                //Console.WriteLine(Debug());
            }
                
        }

        public KickOff(Car car, Vector3 position)
        {
            this.car = car;
            this.position = position;
            kickOffPosition = GetKickOffPosition(car.Position, car.Team);
            if (kickOffPosition == KickOffPositions.Unknown)
            {
                //Console.WriteLine(Debug());
            }

        }

        public Controller Step(float dt)
        {
            //Console.WriteLine(Debug());
            switch (kickOffPosition)
            {
                case KickOffPositions.Center:
                    return KickOffCenter(dt);

                case KickOffPositions.BackCorner1:
                    return KickOffBackCorner(dt);

                case KickOffPositions.BackCorner2:
                    return KickOffBackCorner(dt);

                case KickOffPositions.FrontCorner1:
                    return KickOffFrontCorner(dt);

                default:
                    return DefaultKickoff(dt);
            }
        }

        private Controller DefaultKickoff(float dt)
        {
            var controller = new Controller();
            controller.Boost = true;
            var ballLocation = position;
            var carLocation = car.Position;
            var carRotation = car.Rotation;

            // Calculate to get the angle from the front of the bot's car to the ball.
            var botToTargetAngle = Math.Atan2(ballLocation.Y - carLocation.Y, ballLocation.X - carLocation.X);
            var botFrontToTargetAngle = botToTargetAngle - carRotation.ToRotationAxis().Z;
            var final = (Math.Pow((10 * botFrontToTargetAngle + MathUtility.Sign(botFrontToTargetAngle)), 3)) / 20;
            controller.Steer = MathUtility.Clip((float)final, -1, 1);
            controller.Throttle = 1;

            return controller;
        }

        private Controller KickOffBackCorner(float dt)
        {
            var controller = new Controller();
            Console.WriteLine(Math.Abs(car.Position.Y));

            controller.Throttle = 1;

            var leanValue = 1f;

            if (car.Team == 0 && kickOffPosition == KickOffPositions.BackCorner1 || car.Team == 1 && kickOffPosition == KickOffPositions.BackCorner2)
            {
                leanValue = -1f;
            }


            if (Math.Abs(car.Position.Y) >= 3800 && Math.Abs(car.Position.Y) <= 3900)
            {
                if (sideDodge == null)
                    sideDodge = new SideDodge(car, leanValue);

                // left forward dodge = new Dodge(car, 0.2f, new Vector2(0, -1));
                // right forward dodge = new Dodge(car, 0.2f, new Vector2(0, 1));

                // front dodge = new Dodge(car, 0.2f, new Vector2(-1, 1));
                // double jump = new Dodge(car, 0.2f, new Vector2(0, 0));
                // backflip = new Dodge(car, 0.2f, new Vector2(1, 0));

                controller = sideDodge.Step(dt);
            }
            else if (sideDodge != null)
            {
                controller = sideDodge.Step(dt);
            }

            if (sideDodge != null && sideDodge.Finished)
            {
                sideDodge = null;
                if (dodge == null)
                    dodge = new Dodge(car, 0.2f, new Vector2(-1, 0));
            }


            if (dodge != null)
            {

                controller = dodge.Step(dt);
                controller.Throttle = 1;
            }


            //if (controller.Throttle = 1)

            /*if (Math.Abs(car.Position.Y) > 3700) // Boost in a straight line.
            {
                controller.Boost = true;
            }
            else if (Math.Abs(car.Position.Y) > 1000) // Dodge forward.
            {
                if (dodge == null)
                    dodge = new Dodge(car, 0.2f, new Vector2(-1, 0));

                controller = dodge.Step(dt);

                controller.Boost = Math.Abs(car.Position.Y) > 3000; // Make sure we keep boosting during the first part of the dodge.
            }
            else if (Math.Abs(car.Position.Y) > 700)
            {
                dodge = null;
            }
            else // Final dodge when we are close to the ball.
            {
                if (dodge == null)
                    dodge = new Dodge(car, 0.2f, new Vector2(-1, 0));

                controller = dodge.Step(dt);
            }

            controller.Throttle = 1; // No reason not to hold throttle.
            */
            return controller;
        }

        private Controller KickOffCenter(float dt)
        {
            var controller = new Controller();
            Console.WriteLine(Math.Abs(car.Position.Y));

            controller.Throttle = 1;

            if (Math.Abs(car.Position.Y) >= 4205 && Math.Abs(car.Position.Y) <= 4265)
            {
                if (sideDodge == null)
                    sideDodge = new SideDodge(car);

                // left forward dodge = new Dodge(car, 0.2f, new Vector2(0, -1));
                // right forward dodge = new Dodge(car, 0.2f, new Vector2(0, 1));

                // front dodge = new Dodge(car, 0.2f, new Vector2(-1, 1));
                // double jump = new Dodge(car, 0.2f, new Vector2(0, 0));
                // backflip = new Dodge(car, 0.2f, new Vector2(1, 0));

                controller = sideDodge.Step(dt);
            }
            else if (sideDodge != null)
            {
                controller = sideDodge.Step(dt);
            }

            if (sideDodge != null && sideDodge.Finished)
            {
                sideDodge = null;
                if (dodge == null)
                    dodge = new Dodge(car, 0.2f, new Vector2(-1, 0));
            }


            if (dodge != null)
            {

                controller = dodge.Step(dt);
                controller.Throttle = 1;
            }


            //if (controller.Throttle = 1)

            /*if (Math.Abs(car.Position.Y) > 3700) // Boost in a straight line.
            {
                controller.Boost = true;
            }
            else if (Math.Abs(car.Position.Y) > 1000) // Dodge forward.
            {
                if (dodge == null)
                    dodge = new Dodge(car, 0.2f, new Vector2(-1, 0));

                controller = dodge.Step(dt);

                controller.Boost = Math.Abs(car.Position.Y) > 3000; // Make sure we keep boosting during the first part of the dodge.
            }
            else if (Math.Abs(car.Position.Y) > 700)
            {
                dodge = null;
            }
            else // Final dodge when we are close to the ball.
            {
                if (dodge == null)
                    dodge = new Dodge(car, 0.2f, new Vector2(-1, 0));

                controller = dodge.Step(dt);
            }

            controller.Throttle = 1; // No reason not to hold throttle.
            */
            return controller;
        }

        private Controller KickOffCenterSlow(float dt)
        {
            var controller = new Controller();
            Console.WriteLine(Math.Abs(car.Position.Y));
            if (Math.Abs(car.Position.Y) > 3700) // Boost in a straight line.
            {
                controller.Boost = true;
            }
            else if (Math.Abs(car.Position.Y) > 1000) // Dodge forward.
            {
                if (dodge == null)
                    dodge = new Dodge(car, 0.2f, new Vector2(-1, 0));

                controller = dodge.Step(dt);

                controller.Boost = Math.Abs(car.Position.Y) > 3000; // Make sure we keep boosting during the first part of the dodge.
            }
            else if (Math.Abs(car.Position.Y) > 700)
            {
                dodge = null;
            }
            else // Final dodge when we are close to the ball.
            {
                if (dodge == null)
                    dodge = new Dodge(car, 0.2f, new Vector2(-1, 0));

                controller = dodge.Step(dt);
            }

            controller.Throttle = 1; // No reason not to hold throttle.

            return controller;
        }

        private Controller KickOffFrontCorner(float dt)
        {
            float distance = (new Vector2(car.Position.X, car.Position.Y) - new Vector2(0, 0)).Length();

            Controller controller = new Controller();

            if (car.Velocity.Length() < 1000)
            {
                controller.Boost = true;

                Vector3 local = Vector3.Transform(-car.Position, Quaternion.Inverse(car.Rotation));
                controller.Steer = MathUtility.Clip((float)Math.Atan2(local.Y, local.X) * 0.5f, -1, 1);
            }
            else if (distance > 2400)
            {
                if (dodge == null)
                    dodge = new Dodge(car, 0.13f, new Vector2(-1, 0));

                controller = dodge.Step(dt);

                controller.Boost = true;
            }
            else if (distance > 500)
            {
                Vector3 local = Vector3.Transform(-car.Position, Quaternion.Inverse(car.Rotation));
                controller.Steer = MathUtility.Clip((float)Math.Atan2(local.Y, local.X) * 10, -1, 1);

                controller.Boost = distance < 2000 && car.HasWheelContact;

                dodge = null;
            }
            else
            {
                if (dodge == null)
                {
                    Vector3 local = Vector3.Normalize(Vector3.Transform(-car.Position, Quaternion.Inverse(car.Rotation)));
                    dodge = new Dodge(car, 0.2f, new Vector2(-local.X, local.Y));
                }

                controller = dodge.Step(dt);
            }


            controller.Throttle = 1; // No reason not to hold throttle.

            return controller;
        }

        public static KickOffPositions GetKickOffPosition(Vector3 location, int team)
        {
            //location = Vector3.Abs(location);
            const int distance = 15;

            if (team == 0)
            {
                if (Team0KickOffPosition.FrontCorner1.WithinDistance(location, distance))
                {
                    return KickOffPositions.FrontCorner1;
                }

                if (Team0KickOffPosition.FrontCorner2.WithinDistance(location, distance))
                {
                    return KickOffPositions.FrontCorner2;
                }

                if (Team0KickOffPosition.FrontCorner3.WithinDistance(location, distance))
                {
                    return KickOffPositions.FrontCorner3;
                }

                if (Team0KickOffPosition.FrontCorner4.WithinDistance(location, distance))
                {
                    return KickOffPositions.FrontCorner4;
                }

                if (Team0KickOffPosition.FrontCorner5.WithinDistance(location, distance))
                {
                    return KickOffPositions.FrontCorner5;
                }

                if (Team0KickOffPosition.FrontCorner6.WithinDistance(location, distance))
                {
                    return KickOffPositions.FrontCorner6;
                }

                if (Team0KickOffPosition.BackCorner1.WithinDistance(location, distance))
                {
                    return KickOffPositions.BackCorner1;
                }

                if (Team0KickOffPosition.BackCorner2.WithinDistance(location, distance))
                {
                    return KickOffPositions.BackCorner2;
                }

                if (Team0KickOffPosition.Center.WithinDistance(location, distance))
                {
                    return KickOffPositions.Center;
                }
            }

            if (Team1KickOffPosition.FrontCorner1.WithinDistance(location, distance))
            {
                return KickOffPositions.FrontCorner1;
            }

            if (Team1KickOffPosition.FrontCorner2.WithinDistance(location, distance))
            {
                return KickOffPositions.FrontCorner2;
            }

            if (Team1KickOffPosition.FrontCorner3.WithinDistance(location, distance))
            {
                return KickOffPositions.FrontCorner3;
            }

            if (Team1KickOffPosition.FrontCorner4.WithinDistance(location, distance))
            {
                return KickOffPositions.FrontCorner4;
            }

            if (Team1KickOffPosition.FrontCorner5.WithinDistance(location, distance))
            {
                return KickOffPositions.FrontCorner5;
            }


            if (Team1KickOffPosition.FrontCorner6.WithinDistance(location, distance))
            {
                return KickOffPositions.FrontCorner6;
            }

            if (Team1KickOffPosition.BackCorner1.WithinDistance(location, distance))
            {
                return KickOffPositions.BackCorner1;
            }

            if (Team1KickOffPosition.BackCorner2.WithinDistance(location, distance))
            {
                return KickOffPositions.BackCorner2;
            }

            if (Team1KickOffPosition.Center.WithinDistance(location, distance))
            {
                return KickOffPositions.Center;
            }



            return KickOffPositions.Unknown;
        }


    }
}
