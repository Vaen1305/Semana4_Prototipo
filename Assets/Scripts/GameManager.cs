using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.GameEvents;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject pausePanel;
    public Slider lifeSlider;
    public TMP_Text pointsText;

    [Header("Player Reference")]
    public PlayerController player;

    [Header("Game Events")]
    public GameIntEvent lifeEvent;
    public GameIntEvent pointsEvent;
    public GameEvent winEvent;
    public GameEvent lossEvent;

    [Header("Listeners")]
    public GameIntEventListener lifeEventListener;
    public GameIntEventListener pointsEventListener;
    public GameEventListener winEventListener;
    public GameEventListener lossEventListener;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (player != null)
        {
            lifeSlider.maxValue = player.MaxLife;
            lifeSlider.value = player.CurrentLife;
        }

        lifeEventListener.response.AddListener(UpdateLifeUI);
        pointsEventListener.response.AddListener(UpdatePointsUI);

        winEventListener.response.AddListener(HandleVictory);
        lossEventListener.response.AddListener(HandleDefeat);
    }

    private void OnDestroy()
    {
        lifeEventListener.response.RemoveListener(UpdateLifeUI);
        pointsEventListener.response.RemoveListener(UpdatePointsUI);

        winEventListener.response.RemoveListener(HandleVictory);
        lossEventListener.response.RemoveListener(HandleDefeat);
    }

    public void UpdateLifeUI(int newLife)
    {
        lifeSlider.value = newLife;
    }

    public void UpdatePointsUI(int newPoints)
    {
        pointsText.text = $"Puntos: {newPoints}";
    }

    private void HandleDefeat()
    {
        SceneManager.LoadScene("YouLoss");
    }

    private void HandleVictory()
    {
        SceneManager.LoadScene("YouWin");
    }

    public void TogglePause()
    {
        bool isPaused = !pausePanel.activeSelf;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

