using UnityEngine;
using System.Collections;

public class CellStatus : MonoBehaviour
{
    //references for instantiation
    public GameObject x;
    public GameObject o;

    //the current played piece in this cell, null if empty
    public GameObject playedPiece = null;

    public enum Piece { X, O, None };
    private Piece playedPieceType = Piece.None;
    private bool currentlyActive; //True while it can be placed here.  False if the board it is in has already been won

    void Start()
    {
        currentlyActive = true;
    }

    //This is for normal game purposes.  It will refuse to do anything if the space is already defined
    public void PlayPiece(Piece which)
    {
        if (playedPiece || !currentlyActive || which == Piece.None)
            return;
        GameObject instantiateThis = (which == Piece.X ? x : o);
        playedPiece = Instantiate(instantiateThis, this.transform.position, Quaternion.identity) as GameObject;
        playedPieceType = which;
    }

    //This is used for win conditions only.  It forces to play a piece over whatever may or may not be here already
    public void ForcePiece(Piece which)
    {
        if (playedPiece) //If a piece has already been defined, destroy it
        {
            Destroy(playedPiece);
        }
        //set the right flags
        playedPieceType = which;
        currentlyActive = false;
        //create the new piece in this place
        GameObject instantiateThis = (which == Piece.X ? x : o);
        playedPiece = Instantiate(instantiateThis, this.transform.position, Quaternion.identity) as GameObject;
    }

    public Piece CurrentPiece
    {
        get
        {
            return playedPieceType;
        }
    }

    public bool IsActive
    {
        get
        {
            return currentlyActive;
        }
        set
        {
            currentlyActive = value;
        }
    }
}
