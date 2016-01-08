using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour
{
    public enum GameState { None, TicTacToe, Spacewar, Idle };
    private GameState currentState;
    public GameState defaultState = GameState.TicTacToe;

    public GameObject tttObjects;
    public GameObject swObjects;
    public Canvas tttUI;
    public Canvas swUI;

    private TTTGameManager ttt;
    private SWGameManager sw;
    private BoardContainer parentBoard;

    private AudioManager audio;

    //this will be assigned somewhere in the tic tac toe code whenever there is a tie to break
    //it will be resolved and reset in the space war code once the tie has been broken
    private Board contestedBoard;

    #region unity functions
    void Start()
    {
        ttt = GetComponent<TTTGameManager>();
        sw = GetComponent<SWGameManager>();
        parentBoard = GetComponent<BuildTicTacToeBoard>().parentBoard;
        audio = GetComponent<AudioManager>();
        if (Debug.isDebugBuild)
        {
            SetCurrentState(defaultState); //for quick and easy testing
        }
        else
        {
            SetCurrentState(GameState.TicTacToe); //so I don't have to remember to set this flag later
        }
    }

    void Update()
    {
        switch(currentState)
        {
            case GameState.None:
                Debug.LogError("Game state set to none");
                break;
            case GameState.TicTacToe:
                TTTUpdate();
                TTTCheckWinStates();
                break;
            case GameState.Spacewar:
                SWUpdate();
                break;
            case GameState.Idle:
                //should be set only at the end of the game to stop all other game code from running
                break;
        }
    }
    #endregion

    #region Tic Tac Toe functions
    private void TTTUpdate()
    {
        if (ttt)
            ttt.UpdateTTT();
        else
            Debug.LogErrorFormat("TTTGameManager component missing from {0}", this.gameObject.name);
    }

    //This acts a little like Update (arranged that way by the caller)
    //Just kick start the recursive win checking with this
    public void TTTCheckWinStates()
    {
        if (parentBoard != null)
            parentBoard.CheckWin();
        else
            Debug.LogError("Unable to locate the parent game board to check for wins");
    }
    #endregion

    #region Space War functions
    private void SWUpdate()
    {
        if (sw)
            sw.UpdateSW();
        else
            Debug.LogErrorFormat("SWGameManager component missing from {0}", this.gameObject.name);
    }
    #endregion

    #region state management
    public void SetContestedBoard(Board b)
    {
        contestedBoard = b;
        SwitchToState(GameState.Spacewar);
    }

    public void ResolveContestedBoard(GameObject winner)
    {
        Board.CellStates win;
        string name = winner.gameObject.name;
        if (name.Contains("1"))
        {
            win = Board.CellStates.P1;
        }
        else
        {
            win = Board.CellStates.P2;
        }
        ResolveContestedBoard(win);
    }

    public void ResolveContestedBoard(Board.CellStates tieBreaker)
    {
        if (contestedBoard != null)
        {
            contestedBoard.Winner = tieBreaker;
            contestedBoard = null;
            SwitchToState(GameState.TicTacToe);
        }
        else
        {
            Debug.LogWarning("Contested board set to null. If this isn't because it is in debug mode and didn't start in TTT mode, this is a problem");
        }
    }

    #region state switching
    private void SetCurrentState(GameState state)
    {
        SwitchToState(state);
    }

    private void SwitchToState(GameState state)
    {
        switch (state)
        {
            case GameState.None:
                Debug.LogWarning("State set to state \"None\"");
                currentState = GameState.None;
                break;
            case GameState.TicTacToe:
                SwitchToTTT();
                break;
            case GameState.Spacewar:
                SwitchToSW();
                break;
            case GameState.Idle:
                SwitchToIdle();
                break;
        }
    }

    #region specifc state switches
    /********************************************
     * Within this region, set all the specific *
     * parameters of a state switch needed for  *
     * each transition between states           *
     *******************************************/
    private void SwitchToTTT()
    {
        currentState = GameState.TicTacToe;
        tttObjects.SetActive(true);
        swObjects.SetActive(false);
        swUI.gameObject.SetActive(false);
        audio.StartTTTMusic();
        audio.SWTransitionToTTT();
    }

    private void SwitchToSW()
    {
        currentState = GameState.Spacewar;
        tttObjects.SetActive(false);
        swObjects.SetActive(true);
        swUI.gameObject.SetActive(true);
        sw.InitSW();
        audio.StartSWMusic();
        audio.TTTTransitionToSW();
    }

    private void SwitchToIdle()
    {
        currentState = GameState.Idle;
    }
    #endregion
    #endregion

    #region external state access
    public GameState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
        }
    }
    #endregion
    #endregion
}
