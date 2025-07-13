using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TimeManager TimeManager { get; private set; }
    public ScheduleManager ScheduleManager { get; private set; }
    public CharacterManager CharacterManager { get; private set; }
    public UIGameManager UIGameManager { get; private set; }
    public EventManager EventManager { get; private set; }
    public SaveManager SaveManager { get; private set; }
    public ShopManager ShopManager { get; private set; }
    public EndingManager EndingManager { get; private set; }
    public BackgroundManager BackgroundManager { get; private set; }
    public EventCGManager EventCGManager { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public DialogueManager DialogueManager { get; private set; }
    public SceneLoader SceneLoader { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        TimeManager = gameObject.AddComponent<TimeManager>();
        ScheduleManager = gameObject.AddComponent<ScheduleManager>();
        CharacterManager = gameObject.AddComponent<CharacterManager>();
        UIGameManager = gameObject.AddComponent<UIGameManager>();
        EventManager = gameObject.AddComponent<EventManager>();
        SaveManager = gameObject.AddComponent<SaveManager>();
        ShopManager = gameObject.AddComponent<ShopManager>();
        EndingManager = gameObject.AddComponent<EndingManager>();
        BackgroundManager = gameObject.AddComponent<BackgroundManager>();
        EventCGManager = gameObject.AddComponent<EventCGManager>();
        SoundManager = gameObject.AddComponent<SoundManager>();
        DialogueManager = gameObject.AddComponent<DialogueManager>();
        SceneLoader = gameObject.AddComponent<SceneLoader>();

        TimeManager.Initialize();
        ScheduleManager.Initialize();
        CharacterManager.Initialize();
        UIGameManager.Initialize();
        EventManager.Initialize();
        SaveManager.Initialize();
        ShopManager.Initialize();
        EndingManager.Initialize();
        BackgroundManager.Initialize();
        EventCGManager.Initialize();
        SoundManager.Initialize();
        DialogueManager.Initialize();
        SceneLoader.Initialize();

        Debug.Log("All managers initialized.");
    }

    public void StartNewGame()
    {
        // 새 게임 시작 시 데이터 초기화
        CharacterManager.LoadPlayerData(); // 기본값으로 플레이어 데이터 생성
        TimeManager.ResetTime();
        ScheduleManager.Initialize();

        SceneLoader.LoadScene("Game");
    }

    void Update()
    {
        if (TimeManager != null)
        {
            TimeManager.Tick(Time.deltaTime);
        }
    }
}