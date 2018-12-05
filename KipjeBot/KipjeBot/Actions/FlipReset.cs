﻿using System;
using System.Numerics;

using RLBotDotNet;

using KipjeBot;
using KipjeBot.Utility;

namespace KipjeBot.Actions
{
    public class FlipReset
    {
        public Vector3 Target;
        public Car Car;
        public float StartTime;
        public float ArrivalTime;

        public bool Finished { get; private set; } = false;

        const float jump_t = 0.25f;
        const float jump_dx = 100.0f;
        const float jump_dv = 600.0f;

        private DoubleJump doubleJump;

        public FlipReset(Car car, Vector3 target, float startTime, float arrivalTime)
        {
            Target = target;
            Car = car;
            StartTime = startTime;
            ArrivalTime = arrivalTime;

            if (car.HasWheelContact)
            {
                doubleJump = new DoubleJump();
            }
        }

        public Controller Step(Ball ball, float dt, float currentTime)
        {
            float delta_t = ArrivalTime - currentTime;
            Vector3 A = CalculateCourse(Car, Target, delta_t);

            Controller c = new Controller();

            Vector3 dir = Vector3.Normalize(A);

            if (doubleJump != null && !doubleJump.Finished)
            {
                c = doubleJump.Step();
            }
            else
            {
                Quaternion t;

                Vector3 predicted = Car.Position + Car.Velocity * delta_t + 0.5f * -650f * delta_t * delta_t * Vector3.UnitZ;

                if ((predicted - Target).Length() > 50)
                {
                    t = MathUtility.LookAt(dir, Car.Up);
                }
                else
                {
                    Vector3 d = ball.Position - Car.Position;
                    Vector3 r = new Vector3(d.Y, -d.X, 0);

                    t = MathUtility.LookAt(Vector3.Normalize(Vector3.Cross(d, r)), -Vector3.Normalize(d));
                }

                Vector3 inputs = RotationController.GetInputs(Car, t, dt);

                c.Roll = inputs.X;
                c.Pitch = inputs.Y;
                c.Yaw = inputs.Z;

                if (MathUtility.Angle(Car.Rotation, MathUtility.LookAt(dir, Car.Up)) < 0.4f)
                    c.Boost = true;

                if (Car.CanDodge)
                    Finished = true;
            }

            if (currentTime > ArrivalTime)
                Finished = true;

            return c;
        }

        public static Vector3 CalculateCourse(Car car, Vector3 target, float time)
        {
            Vector3 z = Vector3.UnitZ;
            Vector3 P = target;
            Vector3 x0 = car.Position;
            Vector3 v0 = car.Velocity;

            float delta_t = time;

            if (car.HasWheelContact)
            {
                v0 += car.Up * jump_dv;
                x0 += car.Up * jump_dx;
            }

            float g = -650.0f;
            float a = 9.0f;

            Vector3 A = P - x0 - v0 * delta_t - 0.5f * g * delta_t * delta_t * z;

            Vector3 dir = Vector3.Normalize(A);

            // estimate the time required to turn
            float phi = MathUtility.Angle(car.Rotation, MathUtility.LookAt(dir, car.Up));

            float T = (float)(0.7 * (2.0 * Math.Sqrt(phi / a)));

            // see if the boost acceleration needed to reach the target is achievable
            return dir * 2.0f * A.Length() / ((delta_t - T) * (delta_t - T));
        }
    }
}
