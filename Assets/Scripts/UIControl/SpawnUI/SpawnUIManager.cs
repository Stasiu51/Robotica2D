using GameObjects;
using UnityEngine;

namespace UIControl
{
    public class SpawnUIManager :MonoBehaviour
    {
        [SerializeField] private EditUIEventInvoker _editUiEventInvoker;
        [SerializeField] private SpawnSubUIManager[] _spawnSubs = new SpawnSubUIManager[6];

        public void setSpawnToggles(SpawnBlock.SpawnRouting spawnRouting)
        {
            foreach (Dir dir in Dir.allDirs())
            {
                _spawnSubs[dir.N].setToggles(
                    spawnRouting.getConnected(Dir.E,dir,Chn.Red),
                    spawnRouting.getConnected(Dir.E,dir,Chn.Yellow),
                    spawnRouting.getConnected(Dir.E,dir,Chn.Blue)
                    );
            }
        }

        public void spawnToggleClicked(Dir dir, Chn channel, bool toggled) =>
            _editUiEventInvoker.spawnToggleClicked(dir, channel, toggled);



    }
}