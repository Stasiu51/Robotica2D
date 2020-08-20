using System.Collections;
using System.Collections.Generic;
using GameObjects;
using UnityEngine;

public class ConnectionsController : MonoBehaviour
{
    public GameObject[] connections = new GameObject[6];

    public void setConnection(Dir dir, bool connected) => connections[dir.N].SetActive(connected);

}
