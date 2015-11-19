using UnityEngine;
using System.Collections;

public class SWGameManager : MonoBehaviour
{
    public GameObject p1Ship;
    public GameObject p2Ship;

    public GameObject p1ShipStartPosition;
    public GameObject p2ShipStartPosition;

    private InputManager im;

    void Start()
    {
        im = GetComponent<InputManager>();
    }

    public void UpdateSW()
    {
        
    }

    //Use this method to reset score counters, etc
    //and set the ships to their starting position
    public void InitSW()
    {
        SetShips();
    }

    private void SetShips()
    {
        p1Ship.transform.position = p1ShipStartPosition.transform.position;
        p2Ship.transform.position = p2ShipStartPosition.transform.position;
        p1Ship.transform.LookAt(p2Ship.transform);
        p2Ship.transform.LookAt(p1Ship.transform);
    }

    public void MarkShipAsDead(GameObject ship)
    {
        if (ship == p1Ship)
        {
            //player one died
            ResolveGame(1);
        }
        else if (ship == p2Ship)
        {
            //player two died
            ResolveGame(2);
        }
        else
        {
            Debug.LogWarning("An object that was not a ship was marked as dead. Ignoring.");
        }
    }

    private void ResolveGame(int winner)
    {
        if (winner != 1 || winner != 2)
        {
            Debug.LogWarning("Space War winner marked as an invalid player");
            return;
        }
        Board.CellStates win = (winner == 1 ? Board.CellStates.P1 : Board.CellStates.P2 );
        im.ResolveContestedBoard(win);
    }
}
