using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameObjects
{
    public abstract class Source
    {
        public abstract bool sourceSend(int turn, Chn channel);

        public abstract bool emitToDir(Dir dir, int turn, Chn channel);
    }

    public class TimerSource : Source
    {
        public override bool sourceSend(int turn, Chn channel)
        {
            return true;
        }

        public override bool emitToDir(Dir dir, int turn, Chn channel)
        {
            return true;
        }
    }
}