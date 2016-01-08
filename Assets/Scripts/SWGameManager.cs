using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SWGameManager : MonoBehaviour
{
    public GameObject p1Ship;
    public GameObject p2Ship;

    public GameObject p1ShipStartPosition;
    public GameObject p2ShipStartPosition;

    //UI elements
    public Canvas tutorialCanvas;
    //These are the "P1 press A" and "Ready" UI elements for 
    public Text[] tutorialP1Items = new Text[2];
    public Text[] tutorialP2Items = new Text[2];
    private const int TutorialStep1 = 0;
    private const int TutorialStep2 = 1;
    private bool p1ReadTutorial = false;
    private bool p2ReadTutorial = false;
    public Text countdownFirstElement;

    private InputManager im;

    public enum SWState { Tutorial, PausedIntro, Game };
    private SWState currentState;

    private bool shownTutorial = false;

    void Start()
    {
        im = GetComponent<InputManager>();
    }

    public void UpdateSW()
    {
        switch (currentState)
        {
            case SWState.Game:
                //nothing needs to run every frame in this state
                break;
            case SWState.PausedIntro:
                //nothing needs to run every frame in this state
                break;
            case SWState.Tutorial:
                //wait for player input so both know the instructions, then switch to paused intro
                UpdateTutorial();
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

    private void UpdateTutorial()
    {
        //check button press for player 1
        if(Input.GetButtonDown("P1_Primary") && !p1ReadTutorial)
        {
            p1ReadTutorial = true;
            tutorialP1Items[TutorialStep1].gameObject.SetActive(false);
            tutorialP1Items[TutorialStep2].gameObject.SetActive(true);
            
        }
        //check press for p2
        if (Input.GetButtonDown("P2_Primary") && !p2ReadTutorial)
        {
            p2ReadTutorial = true;
            tutorialP2Items[TutorialStep1].gameObject.SetActive(false);
            tutorialP2Items[TutorialStep2].gameObject.SetActive(true);
        }
        //if both states are resolved, switch to paused intro
        if(p1ReadTutorial && p2ReadTutorial)
            SwitchToPausedIntro();
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
        tutorialCanvas.gameObject.SetActive(true);
        tutorialP1Items[TutorialStep1].gameObject.SetActive(true);
        tutorialP2Items[TutorialStep1].gameObject.SetActive(true);
        tutorialP1Items[TutorialStep2].gameObject.SetActive(false);
        tutorialP2Items[TutorialStep2].gameObject.SetActive(false);
    }

    private void SwitchFromTutorial()
    {
        tutorialCanvas.gameObject.SetActive(false);
    }

    private void SwitchToPausedIntro()
    {
        if (currentState == SWState.Tutorial)
            SwitchFromTutorial();
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
