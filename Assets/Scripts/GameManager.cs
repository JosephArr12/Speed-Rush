using UnityEngine;
using UnityEngine.SceneManagement;
using PrimeTween;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public CanvasGroup gameOverCG;
    public AudioSource audioSource;
    public UnityEvent gameOver = new UnityEvent();
    public bool isGameOver;

    private void Awake()
    {
        gameManager = this;
    }

    private void Start()
    {
        audioSource.mute = Score.instance.GetAudio();
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOver?.Invoke();
        SoundManager.instance.GameOver();
        Tween.Alpha(gameOverCG, 1f, 1f);
        Mover[] movers = FindObjectsOfType<Mover>();
        foreach (Mover mover in movers)
        {
            mover.gameOver = true; // Replace with the actual function you want to call
        }

    }
    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void Audio()
    {
        audioSource.mute = !audioSource.mute;
        Score.instance.SetAudio(audioSource.mute);
    }
}