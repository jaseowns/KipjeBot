using System;
using System.Numerics;

using RLBotDotNet;

namespace KipjeBot.Actions
{
    public class Dodge
    {
        public float Time;
        public Vector2 Direction;

        public bool Finished { get; private set; } = false;

        private Car car;
        private float timeElapsed = 0;

        public Dodge(Car car, float time, Vector2 direction)
        {
            Time = time;
            Direction = direction;
            this.car = car;
        }

        public Controller Step(float dt)
        {
            Controller c = new Controller();

            if (car.HasWheelContact)
            {
                c.Jump = true;
            }

            if (timeElapsed > Time)
            {
                c.Jump = true;
                c.Pitch = Direction.X;
                c.Yaw = Direction.Y;
            }

            if (car.DoubleJumped)
            {
                Finished = true;
            }

            timeElapsed += dt;
            return c;
        }
    }


    public class SideDodge
    {
        public float Time;
        public float NextJump = .6f;
        public float BoostStop = 2.0f;
        public float SteerStart = .01f;
        public float SteerStop = .3f;

        public Vector2 Direction;

        public bool Finished { get; private set; } = false;

        private Car car;
        private float timeElapsed = 0;
        private float leanDirection = 1;

        public SideDodge(Car car, float lean = 1)
        {
            this.leanDirection = lean;
            this.car = car;
        }

        public SideDodge(Car car, float time, Vector2 direction)
        {
            Time = time;
            Direction = direction;
            this.car = car;
        }

        public Controller Step(float dt)
        {
            Controller c = new Controller();

            var rightYaw = .75f * leanDirection;

            // Turn car a bit
            if (timeElapsed == 0)
            {
                c.Yaw = rightYaw;
            }

            // First jump
            if (car.HasWheelContact)
            {
                c.Jump = true;
            }

            if (timeElapsed >= SteerStart && timeElapsed <= SteerStop)
            {
                c.Steer = rightYaw;
                c.Yaw = rightYaw;
            }
            else if (timeElapsed > SteerStop)
            {
                c.Steer = 0;
                c.Yaw = -rightYaw;
            }

            if (timeElapsed > NextJump)
            {
                c.Jump = true;
            }

            if (timeElapsed >= BoostStop)
            {
                Finished = true;
            }

            c.Boost = timeElapsed <= BoostStop;
            timeElapsed += dt;
            return c;
        }
    }

    public class PrettyGoodSideDodge
    {
        public float Time;
        public float NextJump = .6f;
        public float BoostStop = .9f;
        public float SteerStart = .01f;
        public float SteerStop = .3f;

        public Vector2 Direction;

        public bool Finished { get; private set; } = false;

        private Car car;
        private float timeElapsed = 0;

        public PrettyGoodSideDodge(Car car)
        {
            this.car = car;
        }

        public PrettyGoodSideDodge(Car car, float time, Vector2 direction)
        {
            Time = time;
            Direction = direction;
            this.car = car;
        }

        public Controller Step(float dt)
        {
            Controller c = new Controller();

            var rightYaw = 1.0f;

            // Turn car a bit
            if (timeElapsed == 0)
            {
                c.Yaw = rightYaw;
            }

            // First jump
            if (car.HasWheelContact)
            {
                c.Jump = true;
            }

            if (timeElapsed >= SteerStart && timeElapsed <= SteerStop)
            {
                c.Steer = rightYaw;
                c.Yaw = rightYaw;
            }
            else if (timeElapsed > SteerStop)
            {
                c.Steer = 0;
                c.Yaw = -rightYaw;
            }

            if (timeElapsed > NextJump)
            {
                c.Jump = true;
            }

            if (car.HasWheelContact && timeElapsed >= BoostStop)
            {
                Finished = true;
            }

            c.Boost = timeElapsed <= BoostStop;
            timeElapsed += dt;
            return c;
        }
    }
}
