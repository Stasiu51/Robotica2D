using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameObjects
{
    [Serializable]
    public abstract class Source
    {
        public abstract bool sourceSend(int turn, Chn channel);

        public abstract bool emitToDir(Dir dir, int turn, Chn channel);
    }
    [Serializable]
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
    [Serializable]
    public class TestSource : Source
    {
        public override bool sourceSend(int turn, Chn channel)
        {
            if (channel == Chn.Red && turn % 2 == 0) return true;
            if (channel == Chn.Yellow && turn % 2 == 1) return true;
            return false;
        }

        public override bool emitToDir(Dir dir, int turn, Chn channel)
        {
            if (channel == Chn.Red && turn % 2 == 0) return true;
            if (channel == Chn.Yellow && turn % 2 == 1) return true;
            return false;
        }
    }
}