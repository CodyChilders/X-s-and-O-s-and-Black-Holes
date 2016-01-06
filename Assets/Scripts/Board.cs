using UnityEngine;
using System.Collections;

public class Board
{
    public enum CellStates { Empty, P1, P2 };

    private GameObject[,] cells = new GameObject[3, 3];
    public GameObject lines; //This will hold the lines associated with this instance

    protected int movesPerformed;
    protected CellStates winner;

    #region constructors
    public Board()
    {
        movesPerformed = 0;
        winner = CellStates.Empty;
    }
     
    public Board(int x, int y)
    {
        CreateNewBoard(x, y);
        movesPerformed = 0;
        winner = CellStates.Empty;
    }
    #endregion

    #region build step
    private void CreateNewBoard(int x, int y)
    {
        //build cells
        for(int i = 0; i < cells.GetLength(0); i++)
        {
            for(int j = 0; j < cells.GetLength(1); j++)
            {
                Vector3 position = new Vector3(x + i, 0, y + j);
                cells[i, j] = GameObject.Instantiate(TTTPrefabContainer.cell, position, Quaternion.identity) as GameObject;
                cells[i, j].transform.parent = TTTPrefabContainer.tttParentObject.transform;
            }
        }
        //build lines
        Vector3 newPosition = new Vector3(x + 1, 0, y + 1);
        lines = GameObject.Instantiate(TTTPrefabContainer.smallBoard, newPosition, Quaternion.identity) as GameObject;
        lines.transform.parent = TTTPrefabContainer.tttParentObject.transform;
    }
    #endregion

    #region win calculations
    //This method allows the derived version to recurse properly, and this redirects the base case to the right method
    public virtual void CheckWin()
    {
        CheckForWin();
    }

    public virtual void CheckForWin()
    {
        bool foundWin = false;
        //check all 8 ways a win can happen, for each player
        for(int i = 1; i <= 2; i++)
        {
            //There are a lot of goto's in this method.
            //Since checking is fairly expensive, once we find a single match, others won't change anything
            //The gotos spare the computer this unnecessary expense
            CellStates desiredPlayer = (i == 1 ? CellStates.P1 : CellStates.P2);
            //rows
            if(CheckCoordinates(0, 0, 0, 1, 0, 2, desiredPlayer))
            {
                foundWin = true;
                goto EndOfWinChecks;
            }
            if (CheckCoordinates(1, 0, 1, 1, 1, 2, desiredPlayer))
            {
                foundWin = true;
                goto EndOfWinChecks;
            }
            if (CheckCoordinates(2, 0, 2, 1, 2, 2, desiredPlayer))
            {
                foundWin = true;
                goto EndOfWinChecks;
            }
            //columns
            if (CheckCoordinates(0, 0, 1, 0, 2, 0, desiredPlayer))
            {
                foundWin = true;
                goto EndOfWinChecks;
            }
            if (CheckCoordinates(0, 1, 1, 1, 2, 1, desiredPlayer))
            {
                foundWin = true;
                goto EndOfWinChecks;
            }
            if (CheckCoordinates(0, 2, 1, 2, 2, 2, desiredPlayer))
            {
                foundWin = true;
                goto EndOfWinChecks;
            }
            //diagonals
            if (CheckCoordinates(0, 0, 1, 1, 2, 2, desiredPlayer))
            {
                foundWin = true;
                goto EndOfWinChecks;
            }
            if (CheckCoordinates(2, 0, 1, 1, 0, 2, desiredPlayer))
            {
                foundWin = true;
                goto EndOfWinChecks;
            }
        EndOfWinChecks:
            if(foundWin)
            {
                winner = (i == 1 ? CellStates.P1 : CellStates.P2);
                DisableBoard();
                return;
            }
        }
        if (winner == CellStates.Empty && ThisBoardIsFull())
        {
            GameObject.Find("Controller Scripts").GetComponent<InputManager>().SetContestedBoard(this);
        }
    }

    private bool ThisBoardIsFull()
    {
        int spacesFilled = 0;
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                CellStatus.Piece p = cells[i, j].GetComponent<CellStatus>().CurrentPiece;
                if(p != CellStatus.Piece.None)
                {
                    spacesFilled++;
                }
            }
        }
        return spacesFilled == 9;
    }

    //it is easier for the caller to pass in 6, but easier for the function to process 3
    //use this to make them meet in the middle
    protected bool CheckCoordinates(int aa, int ab, int ba, int bb, int ca, int cb, CellStates player)
    {
        Vector2 a = new Vector2(aa, ab);
        Vector2 b = new Vector2(ba, bb);
        Vector2 c = new Vector2(ca, cb);
        return CheckForMatch(player, a, b, c);
    }

    //check if all coordinates passed in match
    protected virtual bool CheckForMatch(CellStates player, params Vector2[] coordinates)
    {
        int matches = 0;
        for (int i = 0; i < coordinates.Length; i++ )
        {
            Vector2 v = coordinates[i];
            //retrieve the correct cell
            GameObject currentCell = cells[(int)v.x, (int)v.y];
            CellStatus cellStatus = currentCell.GetComponent<CellStatus>();
            if (cellStatus == null)
            {
                Debug.LogError("Cell missing its CellStatus component");
            }
            //get its current value
            CellStatus.Piece displayedValue = cellStatus.CurrentPiece;
            //compare to confirm match
            //p1 == x, p2 == y
            if (player == CellStates.P1 && displayedValue == CellStatus.Piece.X ||
                player == CellStates.P2 && displayedValue == CellStatus.Piece.O)
            {
                matches++;
            }
        }
        return matches == coordinates.Length;
    }

    //When a board is won, disable all of them from further use
    protected virtual void DisableBoard()
    {
        for(int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                GameObject currentCell = cells[i, j];
                CellStatus cs = currentCell.GetComponent<CellStatus>();
                if (cs == null)
                    Debug.LogError("Unable to find CellStatus component on a cell");
                cs.ForcePiece( (winner == CellStates.P1 ? CellStatus.Piece.X : CellStatus.Piece.O ));
                cs.IsActive = false;
                lines.gameObject.SetActive(false);
            }
        }
    }

    public CellStates Winner
    {
        get
        {
            return winner;
        }
        set //Set recursively fixes the data structure beneath it whenever it is called so the graphical display is always correct
        {
            winner = value;
            CellStatus.Piece pieceToForce = (value == CellStates.P1 ? CellStatus.Piece.X : CellStatus.Piece.O);
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i, j].GetComponent<CellStatus>().ForcePiece(pieceToForce);
                }
            }
            lines.gameObject.SetActive(false);
        }
    }
    #endregion
}
