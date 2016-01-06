using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SWGameManager : MonoBehaviour
{
    public GameObject p1Ship;
    public GameObject p2Ship;

    public GameObject p1ShipStartPosition;
    public GameObject p2ShipStartPosition;

    public Text countdownFirstElement;

    private InputManager im;

    public enum SWState { Tutorial, PausedIntro, Game };
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
            SwitchToPausedIntro();
        }
        else
        {
            SwitchToTutorial();
            shownTutorial = true;
        }
    }

    private void SetShips()
    {
        p1Ship.transform.position = p1ShipStartPosition.transform.position;
        p2Ship.transform.position = p2ShipStartPosition.transform.position;
        p1Ship.transform.LookAt(p2Ship.transform);
        p2Ship.transform.LookAt(p1Ship.transform);
        //now disable them so that they aren't pulled in while the intro stuff is running
        p1Ship.GetComponent<ShipControls>().ignoreBlackHolePull = true;
        p2Ship.GetComponent<ShipControls>().ignoreBlackHolePull = true;
    }

    private void ActivateShips()
    {
        p1Ship.GetComponent<ShipControls>().ignoreBlackHolePull = false;
        p2Ship.GetComponent<ShipControls>().ignoreBlackHolePull = false;
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

    #region state switching
    private void SwitchToGame()
    {
        currentState = SWState.Game;
        ActivateShips();
    }

    private void SwitchToTutorial()
    {
        currentState = SWState.Tutorial;
    }

    private void SwitchToPausedIntro()
    {
        currentState = SWState.PausedIntro;
        countdownFirstElement.gameObject.SetActive(true);
        Invoke("SwitchToGame", 5); //5 second countdown to wait for, so the game will start at the same time it finishes
    }

    #endregion

    public SWState GetState
    {
        get
        {
            return currentState;
        }
    }
}
