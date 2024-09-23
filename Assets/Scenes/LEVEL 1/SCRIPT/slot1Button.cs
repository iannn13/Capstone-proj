using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slot1Button : MonoBehaviour
{
    public GameObject lunchboxInteract;

    public void OnShowButtonClick()
    {
        lunchboxInteract.SetActive(!lunchboxInteract.activeSelf); // Toggle visibility
    }
}