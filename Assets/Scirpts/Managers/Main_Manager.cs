using UnityEngine;
public class Main_Manager : MonoBehaviour
{
    public static Main_Manager Instance {  get; private set; }

    [HideInInspector] public Scene_Manager Scene_Manager { get; private set; }
    [HideInInspector] public Game_Manager Game_Manager { get; private set; }
    [HideInInspector] public Settings_Manager Settings_Manager { get; private set; }
    void InitializeMainManager()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void LoadManagers()
    {
        if(Scene_Manager == null)
        {
            Scene_Manager = GetComponentInChildren<Scene_Manager>();
        }
        if(Game_Manager == null)
        {
            Game_Manager = GetComponentInChildren<Game_Manager>();
        }
        if(Settings_Manager == null)
        {
            Settings_Manager = GetComponentInChildren<Settings_Manager>();
        }
    }

    void LoadConnection()
    {
        if(Scene_Manager != null)
        {
            Scene_Manager.OnSceneChanged += ResetSignals;
        }

        if (Settings_Manager != null)
        {
            Setting_Menu.OnMenuOpened += Settings_Manager.HandleMenuConnection;
            Setting_Menu.OnMenuClosed += Settings_Manager.HandleMenuDisconnection;
        }

    }

    void DisconectConnection()
    {
        if (Scene_Manager != null)
        {
            Scene_Manager.OnSceneChanged -= ResetSignals;
        }

        if (Settings_Manager == null)
        {
            Setting_Menu.OnMenuOpened -= Settings_Manager.HandleMenuConnection;
            Setting_Menu.OnMenuClosed -= Settings_Manager.HandleMenuDisconnection;
        }
    }

    private void OnEnable()
    {
        InitializeMainManager();
        LoadManagers();
        LoadConnection();
    }

    private void OnDisable()
    {
        DisconectConnection();
    }

    private void Start()
    {
        InitialiceGame();
    }

    void InitialiceGame()
    {
        if(Game_Manager != null)
        {
            Game_Manager.ChangeState(GameState.Waiting);
        }
        if (Settings_Manager != null)
        {
            Settings_Manager.InitialResolution();
            Settings_Manager.InitialVolumen();
        }
    }

    void ResetSignals(bool reset)
    {
        Game_Manager.ResetSignalInGame(reset);
    }
}
