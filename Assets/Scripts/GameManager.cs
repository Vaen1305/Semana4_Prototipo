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
    public GameIntEvent lifeEvent;    // Evento de vida
    public GameIntEvent pointsEvent;  // Evento de puntos

    [Header("Listeners")]
    public GameIntEventListener lifeEventListener;  // Escucha el evento de vida
    public GameIntEventListener pointsEventListener;  // Escucha el evento de puntos

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

        // Asignar el GameManager como el receptor de los eventos
        lifeEventListener.response.AddListener(UpdateLifeUI);
        pointsEventListener.response.AddListener(UpdatePointsUI);

        GameEvents.OnPlayerLoss += HandleDefeat;
        GameEvents.OnPlayerWin += HandleVictory;
    }

    private void OnDestroy()
    {
        // Desasignar los eventos al destruir el GameManager
        lifeEventListener.response.RemoveListener(UpdateLifeUI);
        pointsEventListener.response.RemoveListener(UpdatePointsUI);

        GameEvents.OnPlayerLoss -= HandleDefeat;
        GameEvents.OnPlayerWin -= HandleVictory;
    }

    // Método que se ejecuta cuando el evento de vida es disparado.
    public void UpdateLifeUI(int newLife)
    {
        lifeSlider.value = newLife;
    }

    // Método que se ejecuta cuando el evento de puntos es disparado.
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
