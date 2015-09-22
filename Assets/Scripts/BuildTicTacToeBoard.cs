using UnityEngine;
using System.Collections;

public class BuildTicTacToeBoard : MonoBehaviour
{
    public const int desiredDepth = 2; //how many recursive levels before you hit a standard game

    public GameObject cellBackground;

    //smallest board grid
    public GameObject smallBoardLines;
    //medium board grid
    public GameObject mediumBoardLines;
    //largest board grid
    public GameObject largeBoardLines;

    public BoardContainer parentBoard;

    void Awake()
    {
        AssignPrefabs();
        parentBoard = new BoardContainer(0, 0, desiredDepth);
    }

    //Use this method to assign the links above (since this is the only 'component' in the build step, but lots of places need some of this data
    private void AssignPrefabs()
    {
        TTTPrefabContainer.largeBoard = largeBoardLines;
        TTTPrefabContainer.mediumBoard = mediumBoardLines;
        TTTPrefabContainer.smallBoard = smallBoardLines;
        TTTPrefabContainer.cell = cellBackground;
    }
}

//This was used temporarily to make sure the prefabs were correct.  Obsolete, but here for reference
/*
    void Start()
    {
        for(int i = 0; i < 27; i++)
        {
            for(int j = 0; j < 27; j++)
            {
                GameObject g = Instantiate(cellBackground, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                if( (i + 2) % 3 == 0 && (j + 2) % 3 == 0)
                {
                    GameObject f = Instantiate(smallBoardLines, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                }
                if( (i + 5) % 9 == 0 && (j + 5) % 9 == 0)
                {
                    GameObject h = Instantiate(mediumBoardLines, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                }
                if( (i + 14) % 27 == 0 && (j + 14) % 27 == 0)
                {
                    GameObject a = Instantiate(largeBoardLines, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                }
            }
        }
    }
 * */
