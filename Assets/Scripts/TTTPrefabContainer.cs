using UnityEngine;
using System.Collections;

//This class exists because Unity makes it hard to assign prefabs to objects only used internally, rather than components
//Use it as a container for objects that need to be instantiated in deeply recursive code
public class TTTPrefabContainer : MonoBehaviour
{
    public static GameObject largeBoard;
    public static GameObject mediumBoard;
    public static GameObject smallBoard;
    public static GameObject cell;
}
