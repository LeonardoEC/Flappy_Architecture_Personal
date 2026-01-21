using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Setting_Menu : MonoBehaviour
{
    // Señal de enable o disable del componente
    public static event Action<Setting_Menu> OnMenuOpened;
    public static event Action<Setting_Menu> OnMenuClosed;

    int _indexResolution;
    public event Action<bool> onScreenMode;
    public event Action<float> onVolumeChanged;

    // componentes
    TMP_Dropdown Ui_Resolution;
    Slider Ui_Volumen;
    Toggle Ui_ScreenMode;

    void SignalEmitor()
    {
        OnMenuOpened?.Invoke(this);
    }

    void DisconectSignal()
    {
        OnMenuClosed?.Invoke(this);
    }

    void LoadComponentes()
    {
        if(Ui_Resolution == null)
        {
            Ui_Resolution = GetComponentInChildren<TMP_Dropdown>(true);
        }
        if(Ui_Volumen == null)
        {
            Ui_Volumen = GetComponentInChildren<Slider>(true);
        }
        if(Ui_ScreenMode == null)
        {
            Ui_ScreenMode = GetComponentInChildren<Toggle>(true);
        }
    }

    void ListenerComponents()
    {
        if(Ui_Volumen != null)
        {
            Ui_Volumen.onValueChanged.AddListener(SendVolemen);
        }
        if(Ui_ScreenMode != null)
        {
            Ui_ScreenMode.onValueChanged.AddListener(SendScreenMode);
        }
    }

    private void OnEnable()
    {
        SignalEmitor();
        ListenerComponents();
    }

    private void OnDisable()
    {
        DisconectSignal();
    }

    private void Awake()
    {
        LoadComponentes();
    }

    private void Start()
    {
        ConfigurarDropdownResoluciones();
    }


    void ConfigurarDropdownResoluciones()
    {
        // 1. Obtenemos las resoluciones
        Resolution[] resolucionesSoportadas = Screen.resolutions;

        // 2. Limpiamos y preparamos
        //Ui_Resolution.ClearOptions();
        List<string> opciones = new List<string>();

        // 3. Solo llenamos la lista (¡Sin buscar la activa!)
        for (int i = 0; i < resolucionesSoportadas.Length; i++)
        {
            string opcion = resolucionesSoportadas[i].width + " x " + resolucionesSoportadas[i].height;
            opciones.Add(opcion);
        }

        // 4. Entregamos las opciones
        Ui_Resolution.AddOptions(opciones);
    }

    public void SendResolution()
    {
        Main_Manager.Instance.Settings_Manager.SetNewResolution(_indexResolution);
    }

    void SendScreenMode(bool value)
    {
        onScreenMode?.Invoke(value);
    }    


    void SendVolemen(float valor)
    {
        onVolumeChanged?.Invoke(valor);
    }

    public void UpdateVisual(int indexResolution, bool isFullScreen, float volumen)
    {

        Ui_Resolution.value = indexResolution;
        Ui_ScreenMode.isOn = isFullScreen;
        Ui_Volumen.value = volumen;

        Ui_Resolution.RefreshShownValue();
    }

    public void BackMenu()
    {
        Handle_Main_Menu.RequestChange(MenuState.Main);
    }
}
