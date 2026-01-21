using System;
using UnityEngine;

public class Settings_Manager : MonoBehaviour
{
    private const string ResWidthKey = "ResolutionWidth";
    private const string ResHeightKey = "ResolutionHeight";

    private const string ScreenModeKey = "IsFullScreen";

    private const string VolumenKey = "MasterVolume";

    int ValueActiveResIndex;
    bool ValueActiveScreenMode;
    float ValueActiveVolumen;



    public void HandleMenuConnection(Setting_Menu menu)
    {
        if(menu)
        {
            menu.onScreenMode += SaveScreenMode;
            menu.onVolumeChanged += SaveVolumen;

            menu.UpdateVisual(ValueActiveResIndex, ValueActiveScreenMode, ValueActiveVolumen);
        }
    }

    public void HandleMenuDisconnection(Setting_Menu menu)
    {
        menu.onScreenMode -= SaveScreenMode;
        menu.onVolumeChanged -= SaveVolumen;
    }

    public void SaveResolution(int width, int height)
    {
        IndexResolutionActive(width, height);
        PlayerPrefs.SetInt(ResWidthKey, width);
        PlayerPrefs.SetInt(ResHeightKey, height);
        PlayerPrefs.Save();
    }
    
    public void SaveScreenMode(bool isFullScreen)
    {
        ValueActiveScreenMode = isFullScreen;
        int fullScreenValue = isFullScreen ? 1 : 0;
        PlayerPrefs.SetInt(ScreenModeKey, fullScreenValue);
        PlayerPrefs.Save();
    }

    public void SaveVolumen(float volumen)
    {
        ValueActiveVolumen = volumen;
        PlayerPrefs.SetFloat(VolumenKey, volumen);
        PlayerPrefs.Save();
    }


    // Valores predeterminados
    public void InitialResolution()
    {
        if(PlayerPrefs.HasKey(ResHeightKey) || PlayerPrefs.HasKey(ResWidthKey))
        {
            IndexResolutionActive(PlayerPrefs.GetInt(ResWidthKey), PlayerPrefs.GetInt(ResHeightKey));

            if (PlayerPrefs.HasKey(ScreenModeKey))
            {
                int width = PlayerPrefs.GetInt(ResWidthKey);
                int heigth = PlayerPrefs.GetInt(ResHeightKey);
                int mode = (PlayerPrefs.GetInt(ScreenModeKey));

                bool isFullScreen = (mode == 1);

                Screen.SetResolution(width, heigth, isFullScreen);

                ValueActiveScreenMode = isFullScreen;
            }
            else
            {
                int width = PlayerPrefs.GetInt(ResWidthKey);
                int heigth = PlayerPrefs.GetInt(ResHeightKey);
                int mode = PlayerPrefs.GetInt(ScreenModeKey, 0);

                bool isFullScreen = (mode == 1);

                Screen.SetResolution(width, heigth, isFullScreen);

                ValueActiveScreenMode = isFullScreen;
            }
        }
        else
        {

            int width = PlayerPrefs.GetInt(ResWidthKey, Screen.currentResolution.width);
            int height = PlayerPrefs.GetInt(ResHeightKey, Screen.currentResolution.height);
            int screemMode = PlayerPrefs.GetInt(ScreenModeKey, 0);

            bool isFullScreen = (screemMode == 1);

            Screen.SetResolution(width, height, isFullScreen);

            ValueActiveScreenMode = isFullScreen;
            IndexResolutionActive(width, height);
        }
    }

    public void InitialVolumen()
    {
        if(PlayerPrefs.HasKey(VolumenKey))
        {
            float volumen = PlayerPrefs.GetFloat(VolumenKey);
            AudioListener.volume = volumen;
            ValueActiveVolumen = volumen;
        }
        else
        {
            float volumen = PlayerPrefs.GetFloat(VolumenKey, 0.5f);
            AudioListener.volume = volumen;
            ValueActiveVolumen = volumen;
        }
    }

    public void SetNewResolution(int indexResolution)
    {
        Resolution newResolution = Screen.resolutions[indexResolution];
        SaveResolution(newResolution.width, newResolution.height);
        bool isFullScreen = (PlayerPrefs.GetInt(ScreenModeKey, 0) == 1);
        ValueActiveResIndex = indexResolution;
        Screen.SetResolution(newResolution.width, newResolution.height, isFullScreen);
    }

    void IndexResolutionActive(int width, int height)
    {
        Resolution[] resolucionesSoportadas = Screen.resolutions;

        for (int i = 0; i < resolucionesSoportadas.Length; i++)
        {
            if (resolucionesSoportadas[i].width == width &&
                resolucionesSoportadas[i].height == height)
            {
                ValueActiveResIndex = i;
            }
        }
    }



}
