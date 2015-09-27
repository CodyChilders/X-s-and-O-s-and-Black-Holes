using UnityEngine;
using System.Collections;

public class BlackHoleBehavior : MonoBehaviour
{
    public GameObject p1Ship;
    public GameObject p2Ship;

    public float eventHorizon = 55f;

    private Vector3 thisPosition; //constant since this doesn't move
    private InputManager im;

    void Start()
    {
        thisPosition = this.transform.position;
        im = GameObject.Find("Controller Scripts").GetComponent<InputManager>();
    }

    void Update()
    {
        bool p1Hit = CheckForKill(p1Ship);
        bool p2Hit = CheckForKill(p2Ship);
        if (p1Hit)
        {
            im.ResolveContestedBoard(Board.CellStates.P1);
            return;
        }
        if (p2Hit)
        {
            im.ResolveContestedBoard(Board.CellStates.P2);
            return;
        }
    }

    private bool CheckForKill(GameObject ship)
    {
        Vector3 thatPosition = ship.transform.position;
        float distance = Vector3.Distance(thisPosition, thatPosition);
        return distance <= eventHorizon;
    }
}
