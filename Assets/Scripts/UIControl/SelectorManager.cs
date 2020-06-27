using System.Collections.Generic;
using GameObjects;
using ObjectAccess;
using UnityEngine;

namespace UIControl
{
    public class SelectorManager : MonoBehaviour
    {
        private GameObject selectPrefab;
        private Dictionary<Block,GameObject> selectHexes = new Dictionary<Block, GameObject>();
        private Block recentSelect;
        public void OnEnable()
        {
            if (selectPrefab == null) selectPrefab = GameObject.Find("ObjectAccess").GetComponent<Prefabs>().selectHex;
            selectHexes = new Dictionary<Block, GameObject>();
            recentSelect = null;
        }

        public Block RecentSelect => recentSelect;

        public List<Block> getSelected()
        {
            return new List<Block>(selectHexes.Keys);
        }

        public bool isSelected(Block block)
        {
            return selectHexes.ContainsKey(block);
        }

        private void setEmphasis(GameObject hex, bool isEmphasised)
        {
            if (isEmphasised)
            {
                hex.transform.localScale = new Vector3(1.57f,1.57f,1f);
                hex.GetComponent<SpriteRenderer>().color = new Color(0f, 0.99f, 1f);
            }
            else
            {
                hex.transform.localScale = new Vector3(1.54f,1.54f,1f);
                hex.GetComponent<SpriteRenderer>().color = new Color(0.77f, 0.82f, 0.85f);
            }
        }

        public void setSelected(Block block, bool isSelected)
        {
            if (selectHexes.ContainsKey(block) && !isSelected)
            {
                setRecent(block, false);
                GameObject hex = selectHexes[block];
                hex.SetActive(false);
                Destroy(hex);
                selectHexes.Remove(block);
            }
            else if (!selectHexes.ContainsKey(block) && isSelected)
            {
                GameObject hex = Instantiate(selectPrefab);
                hex.transform.position = HexGrid.getPosFromCoords(block.getPos());
                selectHexes.Add(block,hex);
            }
        }

        private void setRecent(Block block, bool isRecent)
        {
            if (isRecent)
            {
                if (recentSelect != null) setEmphasis(selectHexes[recentSelect], false);
                setEmphasis(selectHexes[block],true);
            
                recentSelect = block;
            }
            else if (recentSelect == block)
            {
                setEmphasis(selectHexes[block], false);
                recentSelect = null;
            }
        }

        public void switchSelected(Block block)
        {
            if (!isSelected(block))
            {
                setSelected(block, true);
                setRecent(block, true);
            }
            else if (block == recentSelect)
            {
                setSelected(block,false);
                if (selectHexes.Count > 0)
                {
                    setRecent(new List<Block>(selectHexes.Keys)[selectHexes.Count-1], true);
                }
            }
            else //selected, but not recent
            {
                setRecent(block, true);
            }
        }

        public void selectOnly(Block block)
        {
            if (recentSelect == block)
            {
                deselectAll();
            }
            else
            {
                deselectAll();
                setSelected(block, true);
                setRecent(block, true);
            } 
        }
        

        public void OnDisable()
        {
            deselectAll();
        }

        public void deselectAll()
        {
            foreach (KeyValuePair<Block,GameObject> kv in selectHexes)
            {
                kv.Value.SetActive(false);
                Destroy(kv.Value);
            }
            selectHexes = new Dictionary<Block, GameObject>();
            recentSelect = null;
        }
    }
}
