using UnityEngine;
using System.Collections;

public class GetAllChildGameObjects : MonoBehaviour 
{
    public static GameObject[] GetAllChildren(GameObject parent)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        GameObject[] objects = new GameObject[children.Length];
        for (int i = 0; i < children.Length; i++)
        {
            Transform child = children[i];
            objects[i] = child.gameObject;
        }
        return objects;
    }
}
