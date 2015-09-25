using UnityEngine;
using System.Collections;

public class SWGameManager : MonoBehaviour
{
    public GameObject p1Ship;
    public GameObject p2Ship;


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
        print("SWGameManager.InitSW called");
    }
}
