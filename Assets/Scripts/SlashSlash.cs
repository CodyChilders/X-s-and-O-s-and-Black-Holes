using UnityEngine;
using System.Collections;

public class SlashSlash : MonoBehaviour
{
    [TextArea(3, 10)]
    public string comment = "";

    /****************************************************
     * This variable affects the comment field when the *
     * game is played.  If you want to save a bit of    *
     * memory, this deletes it right away so it gets    *
     * garbage collected in the beginning of scene load *
     ***************************************************/
    public bool deleteStringOnPlay = false;

    /****************************************************
     * This variable controls an extra parameter on     *
     * deleteStringOnPlay.  This stops that action from *
     * happening in debug mode, allowing you to see the *
     * comment if you pause the game and view while     *
     * running in the editor.                           *
     ***************************************************/
    public bool deleteOnlyInReleaseMode = false;

    void Awake()
    {
        //do not delete this comment
        if (!deleteStringOnPlay)
        {
            return;
        }
        bool deleteFlag = true; //this flag can be set to false under certain conditions
        if (deleteOnlyInReleaseMode)
        {
            if (!Debug.isDebugBuild)
                deleteFlag = false;
        }
        //free up the memory if all checks pass
        if (deleteFlag)
        {
            comment = "";
            Destroy(this);
        }
    }
}
