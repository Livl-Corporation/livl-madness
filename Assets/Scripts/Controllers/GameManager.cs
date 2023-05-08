using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float skyboxRotationSpeed = 0.5f;

    public static GameManager Instance;

    [SerializeField]
    private GameObject sceneCamera;
    
    [SerializeField] private Timer timer;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }

        Debug.LogError("More than one GameManager in scene.");
    }

    private void Start()
    {
        timer.StartTimer();
    }
    
    private void Update()
    {
        MoveSkybox();
    }
    
    public Timer GetTimer()
    {
        return timer;
    }

    private void MoveSkybox()
    {
        var skybox = RenderSettings.skybox;
        var rotation = skybox.GetFloat("_Rotation");
        skybox.SetFloat("_Rotation", rotation + skyboxRotationSpeed * Time.deltaTime);
    }

    public static void SetSceneCameraActive(bool isActive)
    {
        if(Instance.sceneCamera == null)
        {
            return;
        }

        Instance.sceneCamera.SetActive(isActive);
    }

    private void OnDestroy()
    {
        var skybox = RenderSettings.skybox;
        skybox.SetFloat("_Rotation", 0);
    }
}
