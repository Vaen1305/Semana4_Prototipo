using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject pausePanel;
    public Slider lifeSlider;
    public TMP_Text pointsText;

    [Header("Player Reference")]
    public PlayerController player;

    private void Awake()
    {
        if(Instance == null)
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
        InitializeGame();
    }

    private void InitializeGame()
    {
        Time.timeScale = 1f;
        lifeSlider.maxValue = player.MaxLife;
        lifeSlider.value = player.CurrentLife;
        
        PlayerController.OnLifeChanged += UpdateLifeUI;
        PlayerController.OnPointsChanged += UpdatePointsUI;
        PlayerController.OnPlayerDeath += HandleDefeat;
        PlayerController.OnPlayerWin += HandleVictory;
    }

    private void OnDestroy()
    {
        PlayerController.OnLifeChanged -= UpdateLifeUI;
        PlayerController.OnPointsChanged -= UpdatePointsUI;
        PlayerController.OnPlayerDeath -= HandleDefeat;
        PlayerController.OnPlayerWin -= HandleVictory;
    }

    private void UpdateLifeUI(int newLife)
    {
        lifeSlider.value = newLife;
    }

    private void UpdatePointsUI(int newPoints)
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