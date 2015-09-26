using UnityEngine;
using System.Collections;

public class ShowEnemyShip : MonoBehaviour
{
    public GameObject cam;
    private Camera c;

    private int playerNumber;

    void Start()
    {
        c = cam.GetComponent<Camera>();
        string s = this.gameObject.name;
        if (s.Contains("1"))
        {
            playerNumber = 1;
        }
        else if (s.Contains("2"))
        {
            playerNumber = 2;
        }
        else
        {
            Debug.LogErrorFormat("Unknown player number on ship: {0}", s);
            playerNumber = 3;
        }
    }

    void OnGUI()
    {
        Vector3 screenPoint = c.WorldToScreenPoint(this.transform.position);
        //for debugging
        screenPoint += Random.insideUnitSphere * 5;
        GUI.Box(new Rect(screenPoint.x, screenPoint.y, 100, 100), this.gameObject.name);
    }
}
