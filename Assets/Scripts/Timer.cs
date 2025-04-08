using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private float currentTime;

    private void Update()
    {
        currentTime += Time.deltaTime;
        timerText.text = currentTime.ToString("F0");
    }
}