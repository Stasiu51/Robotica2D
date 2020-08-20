using System.Collections;
using System.Collections.Generic;
using GameControl.SubModes_1.EditMode.ProgrammingMode;
using GameObjects;
using ObjectAccess;
using UIControl;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpawnSubUIManager : MonoBehaviour
{
    public SpawnUIManager _spawnUiManager;
    public int direction;
    private Dir dir;
    private Toggle _redToggle;
    private Toggle _yellowToggle;
    private Toggle _blueToggle;

    void OnEnable()
    {
        dir = Dir.dirFromN(direction);
        if (_redToggle == null || _yellowToggle == null || _blueToggle == null)
        _redToggle = transform.GetChild(0).GetComponent<Toggle>();
        _yellowToggle = transform.GetChild(1).GetComponent<Toggle>();
        _blueToggle = transform.GetChild(2).GetComponent<Toggle>();
    }
    // Update is called once per frame
    public void redToggle()
    {
        _spawnUiManager.spawnToggleClicked(dir, Chn.Red, _redToggle.isOn);
    }
    public void yellowToggle()
    {
        _spawnUiManager.spawnToggleClicked(dir, Chn.Yellow, _yellowToggle.isOn);
    }
    public void blueToggle()
    {
        _spawnUiManager.spawnToggleClicked(dir, Chn.Blue, _blueToggle.isOn);
    }

    public void setToggles(bool redOn, bool yellowOn, bool blueOn)
    {
        _redToggle.isOn = redOn;
        _yellowToggle.isOn = yellowOn;
        _blueToggle.isOn = blueOn;
    }
}
