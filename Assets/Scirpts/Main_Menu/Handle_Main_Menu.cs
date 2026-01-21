using System;
using UnityEngine;


public class Handle_Main_Menu : MonoBehaviour
{
    Main_Menu _main_Menu;
    Setting_Menu _setting_Menu;

    /*
     * Recordatorio pasar todos los satelites a un unico script con tiempos de ejecusion para
     * Asegurirar la correcta gestion de tiempos y sincronizacion
     * Preguntar a geminis sobre mi sistema de orquestadores con el Main y los componentes...
     * Los componentes solo operan logicas el main las ejecuta, centralizando cada setelite en un envio de informacion
     * al planeta que recibe y opera esa informacion en tiempo y forma adecuados
    */

    public static event Action<MenuState> OnRequestMenuChane;

    public static void RequestChange(MenuState newState)
    {
        OnRequestMenuChane?.Invoke(newState);
    }

    public MenuState currentMenuStet { get; private set; }

    public void ChangeStateMenu(MenuState state)
    {
        currentMenuStet = state;

        switch (currentMenuStet)
        {
            case MenuState.Main:
                _main_Menu.gameObject.SetActive(true);
                _setting_Menu.gameObject.SetActive(false);
                break;
            case MenuState.Hidden:
                _main_Menu.gameObject.SetActive(false);
                _setting_Menu.gameObject.SetActive(false);
                break;
            case MenuState.Settings:
                _main_Menu.gameObject.SetActive(false);
                _setting_Menu.gameObject.SetActive(true);
                break;
        }
    }

    void LoadComponents()
    {
        if (_main_Menu == null)
        {
            _main_Menu = GetComponentInChildren<Main_Menu>(true);
        }
        if (_setting_Menu == null)
        {
            _setting_Menu = GetComponentInChildren<Setting_Menu>(true);
        }
    }

    void SuscribeToEvents()
    {
        Main_Manager.Instance.Game_Manager.onStateGameCharged += HandleUIMenu;
        OnRequestMenuChane += ChangeStateMenu;
    }

    void UnSubscriBeFromEvents()
    {
        Main_Manager.Instance.Game_Manager.onStateGameCharged -= HandleUIMenu;
        OnRequestMenuChane += ChangeStateMenu;
    }

    private void OnEnable()
    {
        LoadComponents();
        SuscribeToEvents();
    }

    private void OnDisable()
    {
        UnSubscriBeFromEvents();
    }

    void HandleUIMenu(GameState gameState)
    {
        if (gameState == GameState.Waiting)
        {
            ChangeStateMenu(MenuState.Main);
        }
        if (gameState == GameState.Playing)
        {
            ChangeStateMenu(MenuState.Hidden);
        }
    }






}
