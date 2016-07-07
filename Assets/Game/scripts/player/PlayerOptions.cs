using UnityEngine;
using System.Collections;
using System;

public class PlayerOptions : MonoBehaviour {

    [SerializeField]
    public JumpTypes selectedJump = new OneJump();

    abstract public class JumpTypes
    {
        public string jumpName;
        public float jumpDuration;
        public int jumpCount;
        abstract public void Jump();
    }

    public class DoubleJump : JumpTypes
    {
        public string jumpName = "Double";
        public float jumpDuration = 1;
        public int jumpCount = 3;
        public override void Jump()
        {
            //initial jump, wait duration, extra jumps until count over.
        }
    }

    public class GlideJump : JumpTypes
    {
        public string jumpName = "Glide";
        public float jumpDuration = 3;
        public int jumpCount = 3;
        public override void Jump()
        {
            //initial jump, extra jumps glide in facing direction.
        }
    }

    public class LiftJump : JumpTypes
    {
        public string jumpName = "Lift";
        public float jumpDuration = 5;
        public int jumpCount = 2;
        public override void Jump()
        {
            //initial jump, second jump slowly glides upwards.
        }
    }

    public class WarpJump : JumpTypes
    {
        public string jumpName = "Warp";
        public float jumpDuration = 1;
        public int jumpCount = 3;
        public override void Jump()
        {
            //initial jump, next 2 jumps warp the player to facing direction.
        }
    }

    public class OneJump : JumpTypes
    {
        public string jumpName = "One Jump";
        public float jumpDuration = 0;
        public int jumpCount = 1;
        public override void Jump()
        {
            //initial jump only.
        }
    }

    public class MagicElement
    {

        public class FireElement
        {
            public enum AvailableJumps
            {
                OneJump,
                LiftJump,
                GlideJump,
                DoubleJump
            }
            //public enum GrenadeTypes
            //{

            //}
            public enum PrimaryAttacks
            {
                Tornado,
                Bolt,
                Orb,
                DoubleOrb
            }
            public enum SpecialAttacks
            {
                Blast,
                Healing,
                Shield
            }
            public enum SuperAttacks
            {
                FasterRecharge,
                LongerSuper,
                SelfRevive
            }
        }

        public class WaterElement
        {

        }

        public class LightningElement
        {

        }
    }
}
