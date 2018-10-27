using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HierarchyHelp {

    public static List<Transform> FillListWithChildren(Transform parent)
    {
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
        List<Transform> toReturn = new List<Transform>();

        for (int i = 1; i < allChildren.Length; i++)
        {
            toReturn.Add(allChildren[i]);
        }

        return toReturn;
    }

    public static void ChangeLayerOfParentAndChildren(GameObject parent, int layerIndex)
    {
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            child.gameObject.layer = layerIndex;
        }
    }

    public static Transform FindChildByName(Transform parent, string childName)
    {
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
        for (int i = 1; i < allChildren.Length; i++)
        {
            if (allChildren[i].gameObject.name == childName)
            {
                return allChildren[i];
            }
        }

        Debug.LogError("No child with name " + childName + " was found");
        return null;
    }

    public static void ChangeScaleOfParentAndChildren(Transform parent, Vector3 scale)
    {
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren)
        {
            child.localScale = scale;
        }
    }

    public static bool IsInArray(string toCheck, string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == toCheck)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsInArray(int toCheck, int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == toCheck)
            {
                return true;
            }
        }

        return false;
    }
}
