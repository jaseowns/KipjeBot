using KipjeBot.Actions;
using KipjeBot.Helpers;
using KipjeBot.Utility;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SystemVector2 = System.Numerics.Vector2;
using SystemVector3 = System.Numerics.Vector3;

namespace KipjeBot.Plays
{
    public class ChaseBallPlay : BasePlay
    {
        private Dodge dodge;
        private float timeout = 0;

        public ChaseBallPlay()
        {
            Type = "ChaseBall";
        }

        public override bool Available(GameInfo game)
        {
            game.Playbook.Step(game.DeltaTime);
            if (game.Playbook.IsPlayStuck(5.00f))
            {
                return false;
            }

            return true;
            //if (Utility.BallReady(game) && Math.Abs(gameInfo.Ball.Position.Y) < 5050 && Utility.BallProject(game) > 500 - (Calculations.Distance2D(gameInfo.Ball.Position, gameInfo.MyCar.Position) / 2))
            //{
            //    return true;
            //}
            //return false;
        }

        public override GameInfo Execute(GameInfo gameInfo)
        {
            gameInfo = base.Execute(gameInfo);
            timeout += gameInfo.DeltaTime;
            Controller controller = new Controller();

            // getting the coordinates of the goalposts
            var leftPost = new SystemVector3(-MathUtility.Sign(gameInfo.MyCar.Team) * 700, 5100 * -MathUtility.Sign(gameInfo.MyCar.Team), 200);
            var rightPost = new SystemVector3(MathUtility.Sign(gameInfo.MyCar.Team) * 700, 5100 * -MathUtility.Sign(gameInfo.MyCar.Team), 200);
            var center = new SystemVector3(0, 5150 * -MathUtility.Sign(gameInfo.MyCar.Team), 200);

            // time stuff that we don't worry about yet
            var time_guess = 0;
            var bloc = Calculations.Future(gameInfo.Ball, time_guess);

            // vectors from the goalposts to the ball & to Gosling
            var ball_left = Calculations.Angle2(bloc, leftPost);
            var ball_right = Calculations.Angle2(bloc, rightPost);
            var agent_left = Calculations.Angle2(gameInfo.MyCar.Position, leftPost);
            var agent_right = Calculations.Angle2(gameInfo.MyCar.Position, rightPost);

            if (gameInfo.MyCar.Name == "DadBot")
            {
                gameInfo.Renderer.DrawLine3D(ColorWheel.Gray, gameInfo.MyCar.Position, leftPost);
                gameInfo.Renderer.DrawLine3D(ColorWheel.Gray, gameInfo.MyCar.Position, rightPost);
                gameInfo.Renderer.DrawLine3D(ColorWheel.Lime, gameInfo.MyCar.Position, center);
            }


            SystemVector3? goal_target = null;

            //determining if we are left/right/inside of cone
            if (agent_left > ball_left && agent_right > ball_right)
            {
                goal_target = rightPost;
            }
            else if (agent_left > ball_left && agent_right < ball_right)
            {
                goal_target = center;
            }
            else if (agent_left < ball_left && agent_right < ball_right)
            {
                goal_target = leftPost;
            }

            SystemVector3? goal_to_ball = null;
            SystemVector3? goal_to_bot = null;
            var error = 0f;
            if (goal_target.HasValue)
            {

                if (gameInfo.MyCar.Name == "DadBot")
                {
                    gameInfo.Renderer.DrawString2D("goal_target" + goal_target.ToString(), ColorWheel.CyanAqua, new Vector2(20, 20), 2, 2);
                }

                  //if we are outside the cone, this is the same as Gosling's old code
                goal_to_ball = (gameInfo.Ball.Position - goal_target.Value).Normalize();
                goal_to_bot = (gameInfo.MyCar.Position - goal_target.Value).Normalize();
                var difference = goal_to_ball - goal_to_bot;
                error = MathUtility.Clip(Math.Abs(difference.Value.X) + Math.Abs(difference.Value.Y), 1, 10);
            }
            else
            {
                //if we are inside the cone, our line to follow is a vector from the ball to us (although it's still named 'goal_to_ball')
                goal_to_ball = (gameInfo.MyCar.Position - gameInfo.Ball.Position).Normalize();
                error = MathUtility.Clip((float)Calculations.Distance2D(bloc, gameInfo.MyCar.Position) / 1000, (float)0, (float)1);
            }

            if (gameInfo.MyCar.Name == "DadBot")
            {
                gameInfo.Renderer.DrawLine3D(ColorWheel.MagentaFuchsia, gameInfo.MyCar.Position, goal_to_ball.Value);
                if (goal_to_bot.HasValue)
                {
                    gameInfo.Renderer.DrawLine3D(ColorWheel.MagentaFuchsia, gameInfo.MyCar.Position, goal_to_bot.Value);
                }
            }


            //this is measuring how fast the ball is traveling away from us if we were stationary
            var ball_dpp_skew = MathUtility.Clip(Math.Abs(Calculations.dpp(gameInfo.Ball.Position, gameInfo.Ball.Velocity, gameInfo.MyCar.Position, new SystemVector3(0, 0, 0))) / 80, 1, 1.5f);

            //same as Gosling's old distance calculation, but now we consider dpp_skew which helps us handle when the ball is moving
            var target_distance = MathUtility.Clip((40 + Calculations.Distance2D(gameInfo.Ball.Position, gameInfo.MyCar.Position) * (Math.Pow(error, 2))) / 1.8, 0, 4000);
            var target_location = gameInfo.Ball.Position + new SystemVector3((float)((goal_to_ball.Value.X * target_distance) * ball_dpp_skew), (float)(goal_to_ball.Value.Y * target_distance), 0);


            //this also adjusts the target location based on dpp
            var ball_something = Math.Pow(Calculations.dpp(target_location, gameInfo.Ball.Velocity, gameInfo.MyCar.Position, new SystemVector3(0, 0, 0)), 2);

            //if we were stopped, && the ball is moving 100uu/s away from us
            //if (ball_something > 100)
            //{
            //    ball_something = MathUtility.Clip(ball_something, 0, 80);
            //    var correction = gameInfo.Ball.Velocity.Normalize();
            //    correction = new SystemVector3((float)(correction.X * ball_something), (float)(correction.Y * ball_something), (float)(correction.Z * ball_something));
            //    target_location += correction; //we're adding some component of the ball's velocity to the target position so that we are able to hit a faster moving ball better
            //    //it's important that this only happens when the ball is moving away from us.
            //}

            //another target adjustment that applies if the ball is close to the wall
            var extra = 4120 - Math.Abs(target_location.X);
            if (extra < 0)
            {
                // we prevent our target from going outside the wall, && extend it so that Gosling gets closer to the wall before taking a shot, makes things more reliable
                target_location.X = MathUtility.Clip(target_location.X, -4120, 4120);
                target_location.Y = target_location.Y + (-MathUtility.Sign(gameInfo.MyCar.Team) * MathUtility.Clip(extra, -500, 500));
            }


            // TODO: speed
            //getting speed, this would be a good place to modify because it's not very good
            var target_local = BotUtility.ToLocal(gameInfo.Ball.Position, gameInfo.MyCar);
            var angle_to_target = MathUtility.Clip(Math.Atan2(target_local.Y, target_local.X), -3, 3);
            var distance_to_target = Calculations.Distance2D(gameInfo.MyCar.Position, target_location);
            var speed = 2000 - (100 * Math.Pow((1 + angle_to_target), 2));

            //picking our rendered target color based on the speed we want to go
            var colorRed = MathUtility.Clip(((speed / 2300) * 255), 0, 255);
            var colorBlue = MathUtility.Clip(255 - colorRed, 0, 255);

            if (Math.Abs(distance_to_target) > 1000 && gameInfo.MyCar.HasWheelContact)
            {
                //controller.Boost = true;

                /*
                 if (dodge == null)
                     dodge = new Dodge(gameInfo.MyCar, 0.2f, new Vector2(-1, 1));

                 controller = dodge.Step(gameInfo.DeltaTime);
                  */
            }

            if (Math.Abs(distance_to_target) < 1000)
            {

               /*
                if (dodge == null)
                    dodge = new Dodge(gameInfo.MyCar, 0.2f, new Vector2(-1, 1));

                controller = dodge.Step(gameInfo.DeltaTime);
                 */                   
            }

            if (gameInfo.MyCar.Name == "DadBot")
            {
                //gameInfo.Renderer.DrawString2D("Bot : " + bloc.Debug(), ColorWheel.White, new SystemVector2(0, 0), 2, 2);

                //see the rendering tutorial on github about this, just drawing lines from the posts to the ball && one from the ball to the target
                gameInfo.Renderer.DrawLine3D(ColorWheel.White, bloc, leftPost);
                gameInfo.Renderer.DrawLine3D(ColorWheel.Black, bloc, leftPost);
                gameInfo.Renderer.DrawLine3D(ColorWheel.Black, gameInfo.Ball.Position, target_location);
                gameInfo.Renderer.DrawRectangle3D(ColorWheel.Red, target_location, 10, 10, true);

            }

            if (new KickOffPlay().Available(gameInfo))
            {
                Expired = true;
                timeout = 0;
                return gameInfo;
            }



            /*agent.renderer.begin_rendering();
            agent.renderer.draw_line_3d(bloc.data, leftPost.data, agent.renderer.create_color(255, 255, 0, 0));
            agent.renderer.draw_line_3d(bloc.data, rightPost.data, agent.renderer.create_color(255, 0, 255, 0));

            agent.renderer.draw_line_3d(gameInfo.Ball.Position.data, target_location.data, agent.renderer.create_color(255, colorRed, 0, colorBlue));
            agent.renderer.draw_rect_3d(target_location.data, 10, 10, True, agent.renderer.create_color(255, colorRed, 0, colorBlue));
            agent.renderer.end_rendering();
            */

            //if  ballReady(agent) == False or Math.Abs(gameInfo.Ball.Position.Y) > 5050:
            //self.expired = True

            var location = BotUtility.ToLocal(target_location, gameInfo.MyCar);
            var targ = Math.Atan2(location.Y, location.X);

            var ballLocation = gameInfo.Ball.Position;
            var carLocation = gameInfo.MyCar.Position;
            var carRotation = gameInfo.MyCar.Rotation;

            var botToTargetAngle = Math.Atan2(target_location.Y - carLocation.Y, target_location.X - carLocation.X);
            var botFrontToTargetAngle = botToTargetAngle - carRotation.ToRotationAxis().Z;
            var final = (Math.Pow((10 * botFrontToTargetAngle + MathUtility.Sign(botFrontToTargetAngle)), 3)) / 20;

            controller.Steer = Calculations.Steer(final);

            var current_speed = Calculations.Velocity2D(gameInfo.MyCar.Velocity);


            if (speed > current_speed)
            {
                controller.Throttle = 1.0f;
                if (speed > 1400 && Duration.Seconds > 2.2 && current_speed < 2250)
                {
                    //controller.Boost = true;
                }
            }
            else if (speed < current_speed)
            {
                controller.Throttle = -1.0f;
            }


            var distance = MathUtility.Distance2D(gameInfo.Ball.Position, gameInfo.MyCar.Position);
            
            //var ballLocation = gameInfo.Ball.Position;
            //var carLocation = gameInfo.MyCar.Position;
            //var carRotation = gameInfo.MyCar.Rotation;

            //// Calculate to get the angle from the front of the bot's car to the ball.
            //var botToTargetAngle = Math.Atan2(ballLocation.Y - carLocation.Y, ballLocation.X - carLocation.X);
            //var botFrontToTargetAngle = botToTargetAngle - carRotation.ToRotationAxis().Z;
            //var final = (Math.Pow((10 * botFrontToTargetAngle + MathUtility.Sign(botFrontToTargetAngle)), 3)) / 20;
            //controller.Steer = MathUtility.Clip((float)final, -1, 1);
            //controller.Throttle = 1;


            //double botFrontToTargetAngle = botToBallAngle - gameInfo.MyCar.Rotation.Z;
            //// Correct the angle
            //if (botFrontToTargetAngle < -Math.PI)
            //    botFrontToTargetAngle += 2 * Math.PI;
            //if (botFrontToTargetAngle > Math.PI)
            //    botFrontToTargetAngle -= 2 * Math.PI;

            //final = botFrontToTargetAngle;

            //controller.Yaw = MathUtility.Clip((float)final, -1, 1);

            gameInfo.Renderer.DrawString2D("Angel" + final.ToString(), ColorWheel.CyanAqua, new Vector2(580, 725), 2, 2);
            gameInfo.Renderer.DrawString2D("Steer: " + controller.Steer.ToString(), ColorWheel.CyanAqua, new Vector2(590, 765), 2, 2);
            gameInfo.Renderer.DrawString2D("Distance: " + distance, ColorWheel.Red, new Vector2(590, 785), 2, 2);
            gameInfo.Renderer.DrawString2D("Duration: " + Duration.Milliseconds.ToString(), ColorWheel.Red, new Vector2(590, 800), 2, 2);


            gameInfo.Renderer.DrawLine3D(ColorWheel.CyanAqua, gameInfo.MyCar.Position, gameInfo.Ball.Position);


            /*if (timeout > .1)
            {
                if (chaseBall == null)
                    chaseBall = new ChaseBall(gameInfo.MyCar, gameInfo.Ball);
                controller = chaseBall.Step(gameInfo.DeltaTime);
            }*/
            if (Duration.Seconds > 1)
            {
                Expired = true;
            }

            gameInfo.Playbook.Setup(controller);
            return gameInfo;
        }
    }
}
