using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private float currentTime;

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        timerText.text = $"{currentTime.ToString("F0")}s";
    }

    public void ResetTimer()
    {
        currentTime = 0f;
    }
}
