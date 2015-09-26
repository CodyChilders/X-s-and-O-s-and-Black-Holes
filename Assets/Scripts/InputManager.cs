using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    public enum GameState { None, TicTacToe, Spacewar, Idle };
    private GameState currentState;
    public GameState defaultState = GameState.TicTacToe;

    public GameObject tttObjects;
    public GameObject swObjects;

    private TTTGameManager ttt;
    private SWGameManager sw;
    private BoardContainer parentBoard;

    //this will be assigned somewhere in the tic tac toe code whenever there is a tie to break
    //it will be resolved and reset in the space war code once the tie has been broken
    private Board contestedBoard;

    #region unity functions
    void Start()
    {
        ttt = GetComponent<TTTGameManager>();
        sw = GetComponent<SWGameManager>();
        parentBoard = GetComponent<BuildTicTacToeBoard>().parentBoard;
        SetCurrentState(defaultState);
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

    public void ResolveContestedBoard(Board.CellStates tieBreaker)
    {
        contestedBoard.Winner = tieBreaker;
        SwitchToState(GameState.TicTacToe);
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
    }

    private void SwitchToSW()
    {
        currentState = GameState.Spacewar;
        tttObjects.SetActive(false);
        swObjects.SetActive(true);
        sw.InitSW();
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
