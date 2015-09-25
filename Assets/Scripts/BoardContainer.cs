using UnityEngine;
using System.Collections;

public class BoardContainer : Board
{
    private Board[,] board;
    private int depth;
    private Vector2 position;

    #region constructors
    public BoardContainer()
    {
        //Unnecessary, but this has to satisfy the compiler
    }

    public BoardContainer(int x, int y, int depth)
    {
        position = new Vector2(x, y);
        this.depth = depth;
        CreateNewBoard();
    }
    #endregion

    private void CreateNewBoard()
    {
        const float size = 3f;
        board = new Board[3, 3];
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if(depth == 1)
                {
                    board[i, j] = new Board(Mathf.RoundToInt(position.x + i * size),
                                            Mathf.RoundToInt(position.y + j * size));
                }
                else
                {
                    board[i, j] = new BoardContainer(Mathf.RoundToInt(position.x + i * size * 3),
                                                     Mathf.RoundToInt(position.y + j * size * 3),
                                                     depth - 1);
                }
            }
        }
        //finally, create the lines that are supposed to go with this
        GameObject linesToClone = null;
        Vector3 linesPosition = Vector3.zero; //Offset given to the position determined by which lines you are cloning
        switch(depth)
        {
            case 1:
                linesToClone = TTTPrefabContainer.mediumBoard;
                linesPosition = new Vector3(position.x + 4f, 0, position.y + 4f);
                break;
            case 2:
                linesToClone = TTTPrefabContainer.largeBoard;
                linesPosition = new Vector3(position.x + 13f, 0, position.y + 13f);
                break;
            default:
                Debug.LogErrorFormat("Depth set to an invalid number: {0}", depth);
                linesToClone = TTTPrefabContainer.largeBoard;
                break;
        }
        lines = GameObject.Instantiate(linesToClone, linesPosition, Quaternion.identity) as GameObject;
        lines.transform.parent = TTTPrefabContainer.tttParentObject.transform;
    }

    public override void CheckWin()
    {
        //first, check the lower order boards if necessary
        if (winner == CellStates.Empty)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j].CheckWin();
                }
            }
        }
        CheckForWin();
    }

    public override void CheckForWin()
    {
        bool foundWin = false;
        //check all 8 ways a win can happen, for each player
        for (int i = 1; i <= 2; i++)
        {
            //There are a lot of goto's in this method.
            //Since checking is fairly expensive, once we find a single match, others won't change anything
            //The gotos spare the computer this unnecessary expense
            CellStates desiredPlayer = (i == 1 ? CellStates.P1 : CellStates.P2);
            //rows
            if (CheckCoordinates(0, 0, 0, 1, 0, 2, desiredPlayer))
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
                //The goto is unnecessary because it goes straight there anyway
            }
        EndOfWinChecks:
            if (foundWin)
            {
                winner = (i == 1 ? CellStates.P1 : CellStates.P2);
                DisableBoard();
                return;
            }
        }
    }

    protected override bool CheckForMatch(CellStates player, params Vector2[] coordinates)
    {
        int matches = 0;
        foreach (Vector2 v in coordinates)
        {
            //retrieve the correct cell
            Board currentCell = board[(int)v.x, (int)v.y];
            //get its current value
            CellStates displayedValue = currentCell.Winner;
            //compare to confirm match
            //p1 == x, p2 == y
            if (player == displayedValue) //Player will be either p1 or p2, and can check against the winner field directly
            {
                matches++;
            }
        }
        return matches == coordinates.Length;
    }

    //When a board is won, disable all of them from further use
    protected override void DisableBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Board currentCell = board[i, j];
                //resolve based on which is the type of the base class
                if (currentCell.GetType() == typeof(Board))
                {
                    currentCell.Winner = winner;
                    currentCell.lines.gameObject.SetActive(false);
                }
                else if (currentCell.GetType() == typeof(BoardContainer))
                {
                    //CheckWin(); This line caused a stack overflow?
                    Debug.Log("Win checking for board container happens here");
                    OverwriteBoardWithWinner();
                }
            }
        }
        lines.gameObject.SetActive(false);
    }

    //This method finds all cells in the game, and forces them to take the winner's piece
    //This breaks the generazation of the data structure, as this method assumes that the
    //instance of BoardContainer calling this method is the top level.  This can be assumed
    //since the models aren't generalized like the original 2D implementation.
    private void OverwriteBoardWithWinner()
    {
        //Forcing a win condition code
        CellStatus.Piece winningPiece = (winner == CellStates.P1 ? CellStatus.Piece.X : CellStatus.Piece.O);
        GameObject[] allCells = GameObject.FindGameObjectsWithTag("Cell");
        foreach (GameObject go in allCells)
        {
            go.GetComponent<CellStatus>().ForcePiece(winningPiece);
        }
        //Hiding the lines code
        GameObject[] allLines = GameObject.FindGameObjectsWithTag("Lines");
        foreach (GameObject go in allLines)
        {
            go.SetActive(false);
        }
    }
}
