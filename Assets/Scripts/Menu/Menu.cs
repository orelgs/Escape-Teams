using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public new string name;
    public bool IsOpen { get; private set; } = false;

    public void Open()
    {
        IsOpen = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        IsOpen = false;
        gameObject.SetActive(false);
    }
}
