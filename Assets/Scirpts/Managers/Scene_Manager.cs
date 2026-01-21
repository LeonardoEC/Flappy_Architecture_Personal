using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public event Action<Scene> OnSceneLoaded;
    public event Action<bool> OnSceneChanged;

    public const string SecurityScene = "Manager_Scene";
    public const string FirstScene = "Menu_Main";
    public const string InjectionScene = "UI_InGame";
    public const string GameScenesFolderPath = "/Game/";
    void SubscribeToEvents()
    {
        SceneManager.sceneLoaded += HandleScene;
        SceneManager.sceneLoaded += StartScene;
        SceneManager.sceneLoaded += HandleSceneInjection;
    }

    void UnSubscriBeFromEvents()
    {
        SceneManager.sceneLoaded -= HandleScene;
        SceneManager.sceneLoaded -= StartScene;
        SceneManager.sceneLoaded -= HandleSceneInjection;
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnSubscriBeFromEvents();
    }

    void HandleScene(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded?.Invoke(scene);
        OnSceneChanged?.Invoke(true);
    }

    void StartScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SecurityScene)
        {
            SceneManager.LoadScene(FirstScene);
        }
    }

    void HandleSceneInjection(Scene scene, LoadSceneMode mode)
    {
        if(scene.path.Contains(GameScenesFolderPath))
        {
            if(!SceneManager.GetSceneByName(InjectionScene).isLoaded)
            {
                SceneManager.LoadSceneAsync(InjectionScene, LoadSceneMode.Additive);
            }
        }
    }

    public void LoadScene(string scene)
    {
        if(SceneManager.GetActiveScene().name != scene && scene != SecurityScene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}
