using System;
using rlbot.flat;
using RLBotDotNet.GameState;
using SystemVector2 = System.Numerics.Vector2;
using SystemVector3 = System.Numerics.Vector3;

namespace KipjeBot.Helpers
{
    public static class ExtMethods
    {
        public static bool WithinDistance(this SystemVector3 value, SystemVector3 location, int distance)
        {
            return (location - value).Length() < distance;
        }

        public static SystemVector3 ToVector3(this DesiredVector3 value)
        {
            return new SystemVector3(value.X.GetValueOrDefault(0), value.Y.GetValueOrDefault(0), value.Z.GetValueOrDefault(0));
        }

        public static string Debug(this Vector3 value)
        {
            return string.Format("X:{0} Y:{1} Z:{2}", value.X, value.Y, value.Z);
        }

        public static string Debug(this SystemVector3 value)
        {
            return string.Format("X:{0} Y:{1} Z:{2}", value.X, value.Y, value.Z);
        }

        public static string Debug(this Rotator value)
        {
            return string.Format("X (Roll):{0} Y(Pitch):{1} Z(Yaw):{2}", value.Roll, value.Pitch, value.Yaw);
        }



        public static string Debug(this RLBotDotNet.Controller value)
        {
            return string.Format("Throttle: {0} Steer: {1} Pitch: {2}  Yaw: {3} Roll: {4} Jump: {5} Boost: {6} Handbrake: {7}", value.Throttle, value.Steer, value.Pitch, value.Yaw, value.Roll, value.Jump, value.Boost, value.Handbrake);
        }
    }
}
