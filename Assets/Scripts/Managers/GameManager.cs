using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private List<ManagerBase> managers = new List<ManagerBase>();

    // 특정 매니저에 접근해야 할 경우를 위한 제네릭 메서드
    public T GetManager<T>() where T : ManagerBase
    {
        foreach (var manager in managers)
        {
            if (manager is T typedManager)
            {
                return typedManager;
            }
        }
        return null;
    }

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
        // 필요한 매니저들을 순서대로 추가
        managers.Add(gameObject.AddComponent<TimeManager>());
        managers.Add(gameObject.AddComponent<CharacterManager>());
        managers.Add(gameObject.AddComponent<ScheduleManager>());
        managers.Add(gameObject.AddComponent<EventManager>());
        managers.Add(gameObject.AddComponent<DialogueManager>());
        managers.Add(gameObject.AddComponent<SoundManager>());
        managers.Add(gameObject.AddComponent<SaveManager>());
        managers.Add(gameObject.AddComponent<SceneLoader>());
        managers.Add(gameObject.AddComponent<ShopManager>());
        managers.Add(gameObject.AddComponent<EndingManager>());
        managers.Add(gameObject.AddComponent<BackgroundManager>());
        managers.Add(gameObject.AddComponent<EventCGManager>());
        managers.Add(gameObject.AddComponent<UIGameManager>()); // UI 매니저는 다른 매니저들의 이벤트에 의존하므로 마지막에 초기화

        // 모든 매니저 초기화
        foreach (var manager in managers)
        {
            manager.ManagedInitialize();
        }

        Debug.Log("All managers initialized.");
    }

    public void StartNewGame()
    {
        // 새 게임 시작 시 데이터 초기화
        GetManager<CharacterManager>().LoadPlayerData(); // 기본값으로 플레이어 데이터 생성
        GetManager<TimeManager>().ResetTime();
        GetManager<ScheduleManager>().Initialize(); // TODO: 이 부분도 ManagedInitialize로 통합 고려

        GetManager<SceneLoader>().LoadScene("Game");
    }

    void Update()
    {
        // TimeManager의 Tick은 계속 호출되어야 함
        GetManager<TimeManager>()?.Tick(Time.deltaTime);
    }
}