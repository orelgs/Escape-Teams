using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private List<Menu> menus;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        foreach (Menu menu in menus)
        {
            if (menu.name == menuName)
            {
                menu.Open();
            }
            else if (menu.IsOpen)
            {
                menu.Close();
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        OpenMenu(menu.name);
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
