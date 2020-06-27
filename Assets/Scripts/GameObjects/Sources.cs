using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameObjects
{
    public abstract class Source
    {
        public abstract bool sourceSend(int turn, int channel);

        public abstract bool emitToDir(Dir dir, int turn, int channel);
    }

    public class TimerSource : Source
    {
        public override bool sourceSend(int turn, int channel)
        {
            return true;
        }

        public override bool emitToDir(Dir dir, int turn, int channel)
        {
            return true;
        }
    }
}