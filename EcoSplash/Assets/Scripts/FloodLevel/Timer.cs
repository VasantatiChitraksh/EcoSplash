using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    public static GamePlayingClockUI Instance { get; private set; }
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 200f;

    private void Awake()
    {
        Instance = this;
        gamePlayingTimer = gamePlayingTimerMax;
    }
    private void Update()
    {
        gamePlayingTimer -= Time.deltaTime;
        timerImage.fillAmount = 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public float GetTime()
    {
        return gamePlayingTimer;
    }
}
