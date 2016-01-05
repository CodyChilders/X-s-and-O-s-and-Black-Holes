using UnityEngine;
using System.Collections;

public class TTTGameManager : MonoBehaviour
{
    private int turnCounter;
    private bool playerOnesTurn;

    public Vector3 cursorStartPosition = new Vector3(0, 0, 26); //This vector3 starts a cursor in the top right corner of the board

    public GameObject p1Cursor;
    public GameObject p2Cursor;

    public float minTimeBetweenCursorMoves = 1f / 5f;
    private bool okayToMoveCursor = true;

    private AudioManager audio;

    private const float oneFrameSeconds = 1f / 60f;
    private int framesJoystickHeldDownFor;
    private int framesBeforeJoystickDelayUnlock = 50;

    void Start()
    {
        ConfigGame();
        audio = GetComponent<AudioManager>();
    }

    private void ConfigGame()
    {
        //move them both to a starting position
        p1Cursor.transform.position = cursorStartPosition;
        p2Cursor.transform.position = cursorStartPosition;
        p2Cursor.gameObject.SetActive(false); //this gets toggled when the turns change
        turnCounter = 0;
        framesJoystickHeldDownFor = 0;
        playerOnesTurn = true;
    }

    //This acts as the update function, but it is only called by InputManager because that controls the state.
    //It is redirected here on an update tick if appropriate
    public void UpdateTTT()
    {
        string playerPrefix = (playerOnesTurn ? "P1_" : "P2_");
        GameObject currentCursor = (playerOnesTurn ? p1Cursor : p2Cursor);
        
        //Handle movement
        Vector2 delta = new Vector2();
        delta.x = Input.GetAxis(playerPrefix + "Horizontal");
        delta.y = Input.GetAxis(playerPrefix + "Vertical");
        bool joystickTiltedEnoughToMove = false;
        bool disableJoystickSpeedup = false;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            joystickTiltedEnoughToMove = true;
            if(okayToMoveCursor)
            {
                int c = 0;
                if (delta.x > 0.5f)
                    c = 1;
                else if (delta.x < -0.5f)
                    c = -1;
                currentCursor.transform.Translate(Vector3.right * c);
                if (currentCursor.transform.position.x < 0 || currentCursor.transform.position.x > 26) //it is off the board, undo
                {
                    currentCursor.transform.Translate(Vector3.right * -c);
                    audio.TTTEdgeBoundry();
                }
                disableJoystickSpeedup = ConfigureCursorForNextAction();
            }
            
        }
        else if (Mathf.Abs(delta.x) <= Mathf.Abs(delta.y))
        {
            joystickTiltedEnoughToMove = true;
            if (okayToMoveCursor)
            {
                int c = 0;
                if (delta.y > 0.5f)
                    c = -1;
                else if (delta.y < -0.5f)
                    c = 1;
                currentCursor.transform.Translate(Vector3.forward * c);
                if (currentCursor.transform.position.z < 0 || currentCursor.transform.position.z > 26) //it is off the board, undo
                {
                    currentCursor.transform.Translate(Vector3.forward * -c);
                    audio.TTTEdgeBoundry();
                }
                disableJoystickSpeedup = ConfigureCursorForNextAction();
            }
        }

        if (disableJoystickSpeedup)
        {
            framesJoystickHeldDownFor = 0; //Reset this counter so the joystick won't run super fast starting for the next player
        }
        #region debug keyboard controls
        /*
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentCursor.transform.Translate(Vector3.left);
            if (currentCursor.transform.position.x < 0) //it is off the board, undo
            {
                currentCursor.transform.Translate(Vector3.right);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentCursor.transform.Translate(Vector3.right);
            if (currentCursor.transform.position.x > 26) //it is off the board, undo
            {
                currentCursor.transform.Translate(Vector3.left);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentCursor.transform.Translate(Vector3.forward);
            if (currentCursor.transform.position.z > 26) //it is off the board, undo
            {
                currentCursor.transform.Translate(Vector3.back);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentCursor.transform.Translate(Vector3.back);
            if (currentCursor.transform.position.z < 0) //it is off the board, undo
            {
                currentCursor.transform.Translate(Vector3.forward);
            }
        }
         * */
        #endregion
        //Handle actions
        if (Input.GetButtonDown(playerPrefix + "Primary"))
        {
            bool successful = ActivateCell();
            if (successful)
            {
                SwitchPlayers();
                audio.TTTPlacePiece();
            }
            else
            {
                audio.TTTFailPlacePiece();
            }
        }
    }

    //This function returns false if the fast or slow cursor state should not be changed
    //This function returns true if it should be changed
    private bool ConfigureCursorForNextAction()
    {
        bool returnVal;
        if (Input.GetKey(KeyCode.Space))
            print("Break");
        float resetTime;
        okayToMoveCursor = false;
        framesJoystickHeldDownFor++;
        if (framesJoystickHeldDownFor >= framesBeforeJoystickDelayUnlock)
        {
            resetTime = oneFrameSeconds;
            returnVal = false;
        }
        else
        {
            resetTime = minTimeBetweenCursorMoves;
            returnVal = true;
        }
        Invoke("AllowCursorToMove", resetTime);
        return returnVal;
    }

    private void AllowCursorToMove()
    {
        okayToMoveCursor = true;
    }

    public void SwitchPlayers()
    {
        if (playerOnesTurn)
        {
            p1Cursor.gameObject.SetActive(false);
            p2Cursor.gameObject.SetActive(true);
        }
        else
        {
            p1Cursor.gameObject.SetActive(true);
            p2Cursor.gameObject.SetActive(false);
        }
        playerOnesTurn = !playerOnesTurn;
    }

    private bool ActivateCell()
    {
        GameObject cellUnderCursor = null; //this will hold the cell when we eventually find it
        GameObject currentCursor = (playerOnesTurn ? p1Cursor : p2Cursor);
        //raycast downward from the cursor
        Vector3 raycastStartPosition = currentCursor.transform.position + 2 * Vector3.up; //Start a little above it so the collider doesn't enclose it
        Ray raycastRay = new Ray(raycastStartPosition, Vector3.down);
        RaycastHit[] rchs = Physics.RaycastAll(raycastRay, 15); //15 is plenty far, and won't kill the efficiency too much
        foreach (RaycastHit rch in rchs)
        {
            cellUnderCursor = rch.collider.gameObject;
            if (!cellUnderCursor.CompareTag("Cell"))
            {
                continue;
            }
            CellStatus cs = cellUnderCursor.GetComponent<CellStatus>();
            CellStatus.Piece valueAtThisCell = cs.CurrentPiece;
            if (valueAtThisCell == CellStatus.Piece.X || valueAtThisCell == CellStatus.Piece.O)
            {
                return false; //This is already defined.  Return false and don't do anything
            }
            CellStatus.Piece pieceToPlay = (playerOnesTurn ? CellStatus.Piece.X : CellStatus.Piece.O);
            cs.PlayPiece(pieceToPlay);
            return true;
        }
        //if it got all the way through this loop, it missed somehow.  Print a warning and return false
        Debug.LogWarningFormat("No cell found under tic tac toe marker {0}", currentCursor.gameObject.name);
        return false;
    }
}
