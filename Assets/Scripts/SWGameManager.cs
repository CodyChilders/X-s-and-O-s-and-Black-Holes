using UnityEngine;
using System.Collections;

public class SWGameManager : MonoBehaviour
{
    public GameObject p1Ship;
    public GameObject p2Ship;

    public GameObject p1ShipStartPosition;
    public GameObject p2ShipStartPosition;

    void Start()
    {

    }

    public void UpdateSW()
    {
        print("SWGameManager.UpdateSW called");
    }

    //Use this method to reset score counters, etc
    //and set the ships to their starting position
    public void InitSW()
    {
        SetShips();
        SetCameras();
    }

    private void SetShips()
    {
        p1Ship.transform.position = p1ShipStartPosition.transform.position;
        p2Ship.transform.position = p2ShipStartPosition.transform.position;
        p1Ship.transform.LookAt(p2Ship.transform);
        p2Ship.transform.LookAt(p1Ship.transform);
    }

    private void SetCameras()
    {
        print("SetCameras called");
    }
}
