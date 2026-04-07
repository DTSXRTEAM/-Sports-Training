using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    public GameObject[] activeGameObjects; // Array to hold multiple objects

    public void ActivateObjects()
    {
        foreach (GameObject obj in activeGameObjects)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
