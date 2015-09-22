using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    public enum GameState { None, TicTacToe, Spacewar };
    private GameState currentState;
    public GameState defaultState = GameState.TicTacToe;

    private TTTGameManager ttt;
    private SWGameManager sw;
    private BoardContainer parentBoard;

    void Start()
    {
        SetCurrentState(defaultState);
        ttt = GetComponent<TTTGameManager>();
        sw = GetComponent<SWGameManager>();
        parentBoard = GetComponent<BuildTicTacToeBoard>().parentBoard;
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
        }
    }

    private void SetCurrentState(GameState state)
    {
        currentState = defaultState;
    }

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

    private void SWUpdate()
    {
        if (sw)
            //sw.UpdateSW();
            print("dummy line");
        else
            Debug.LogErrorFormat("SWGameManager component missing from {0}", this.gameObject.name);
    }

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
}
