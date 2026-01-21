using System;
using UnityEngine;

public class Main_Menu : MonoBehaviour
{

    public void ButtonStartGame()
    {
        Main_Manager.Instance.Game_Manager.StartGame();
    }

    public void ButtonSetting()
    {
        Handle_Main_Menu.RequestChange(MenuState.Settings);
    }

    public void ButtonQuitGame()
    {
        Main_Manager.Instance.Game_Manager.QuitGame();
    }


}
