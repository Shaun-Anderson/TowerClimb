using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    //enums for the state of the slingshot, the 
    //state of the game and the state of the bird
    public enum SlingshotState
    {
        Idle,
        UserPulling,
        BirdFlying
    }

    public enum PlayerState
    {
        Idle,
        Dashing,
        Playing,
        Won,
        Lost
    }


    public enum BirdState
    {
        BeforeThrown,
        Thrown
    }
