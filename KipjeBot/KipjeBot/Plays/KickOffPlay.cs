using KipjeBot.Actions;
using KipjeBot.Helpers;
using KipjeBot.Utility;
using RLBotDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SystemVector2 = System.Numerics.Vector2;
using System.Text;
using System.Threading.Tasks;

namespace KipjeBot.Plays
{
    public class KickOffPlay : BasePlay
    {
        private KickOff kickoff;
        private float timeout = 0;
        private bool updateController = true;

        public KickOffPlay() : base()
        {
            Type = "KickOff";
        }

        public override bool Available(GameInfo game)
        {
            //game.Playbook.Step(game.DeltaTime);            
            if (game.MyCar.Velocity == new Vector3(0,0,0) && game.Ball.Rotation.X == 0 && game.Ball.Rotation.Y == 0 && game.Ball.Rotation.Z == 0)
            {
                return true;
            }
            return false;
        }

        public override GameInfo Execute(GameInfo gameInfo)
        {
            gameInfo = base.Execute(gameInfo);
            timeout += gameInfo.DeltaTime;
            Controller controller = new Controller();
            /*Console.WriteLine(gameInfo.MyCar.Position.Debug());
            Console.WriteLine(gameInfo.MyCar.Rotation.Z);
            Console.WriteLine(gameInfo.MyCar.Rotation.ToRotationAxis().Debug());*/

            if (kickoff == null)
                kickoff = new KickOff(gameInfo.MyCar);

            //gameInfo.Renderer.DrawString2D("KickOff:" + kickoff.RenderDebug(), ColorWheel.CyanAqua, new SystemVector2(0, 20), 2, 2);
            
            if (updateController)
            {
                controller = kickoff.Step(0.0166667f);
            }


            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Green, Team0KickOffPosition.FrontCorner1, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.CyanAqua, Team0KickOffPosition.FrontCorner2, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Green, Team0KickOffPosition.FrontCorner3, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.CyanAqua, Team0KickOffPosition.FrontCorner4, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Yellow, Team0KickOffPosition.FrontCorner5, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Yellow, Team0KickOffPosition.FrontCorner6, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Blue, Team0KickOffPosition.Center, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Red, Team0KickOffPosition.BackCorner1, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Red, Team0KickOffPosition.BackCorner2, 10, 10, true);

            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Green, Team1KickOffPosition.FrontCorner1, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.CyanAqua, Team1KickOffPosition.FrontCorner2, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Green, Team1KickOffPosition.FrontCorner3, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.CyanAqua, Team1KickOffPosition.FrontCorner4, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Yellow, Team1KickOffPosition.FrontCorner5, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Yellow, Team1KickOffPosition.FrontCorner6, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Blue, Team1KickOffPosition.Center, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Red, Team1KickOffPosition.BackCorner1, 10, 10, true);
            gameInfo.Renderer.DrawCenteredRectangle3D(ColorWheel.Red, Team1KickOffPosition.BackCorner2, 10, 10, true);

            if (timeout > 15 || Duration.Seconds > 10)
            {
                kickoff = null;
                Expired = true;
                timeout = 0;
            }

            //gameInfo.Renderer.DrawString2D("Angel" + final.ToString(), ColorWheel.CyanAqua, new Vector2(580, 725), 2, 2);
            //gameInfo.Renderer.DrawString2D("Steer: " + controller.Steer.ToString(), ColorWheel.CyanAqua, new Vector2(590, 765), 2, 2);
            //gameInfo.Renderer.DrawString2D("Distance: " + distance, ColorWheel.Red, new Vector2(590, 785), 2, 2);
            gameInfo.Renderer.DrawString2D("Duration: " + Duration.Milliseconds.ToString(), ColorWheel.Red, new Vector2(590, 800), 2, 2);

            gameInfo.Playbook.Setup(controller);
            return gameInfo;
        }
    }
}
