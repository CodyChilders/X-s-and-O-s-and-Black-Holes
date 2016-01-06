using UnityEngine;
using System.Collections;

public class SWGameManager : MonoBehaviour
{
    public GameObject p1Ship;
    public GameObject p2Ship;

    public GameObject p1ShipStartPosition;
    public GameObject p2ShipStartPosition;

    private InputManager im;

    private enum SWState { Tutorial, PausedIntro, Game };
    private SWState currentState;

    private bool shownTutorial;

    void Start()
    {
        im = GetComponent<InputManager>();
        shownTutorial = false;
    }

    public void UpdateSW()
    {
        switch (currentState)
        {
            case SWState.Game:
                //nothing needs to run every frame in this state
                break;
            case SWState.PausedIntro:
                //resolve countdown timers while player gets ready
                break;
            case SWState.Tutorial:
                //wait for player input so both know the instructions, then switch to paused intro
                break;
        }
    }

    //Use this method to reset score counters, game state, etc, and set the ships to their starting position
    public void InitSW()
    {
        SetShips();
        if (shownTutorial)
        {
            currentState = SWState.PausedIntro;
        }
        else
        {
            currentState = SWState.Tutorial;
            shownTutorial = true;
        }
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
