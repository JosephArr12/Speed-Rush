using UnityEngine;
using UnityEngine.SceneManagement;
using PrimeTween;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public CanvasGroup gameOverCG;
    public AudioSource audioSource;
    public UnityEvent gameOver = new UnityEvent();
    public bool isGameOver;
    public GameObject pauseMenu;
    public EventSystem eventSystem;
    public GameObject pauseButton;
    public GameObject endScreenButton;

    private void Awake()
    {
        eventSystem.SetSelectedGameObject(pauseButton);
        gameManager = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void OnEnable()
    {

    }

    private void OnDisable()
    {
        gameOver.RemoveAllListeners();
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOver?.Invoke();
        SoundManager.instance.GameOver();
        eventSystem.SetSelectedGameObject(endScreenButton);
        gameOverCG.gameObject.SetActive(true);
        Tween.Alpha(gameOverCG, 1f, 1f).OnComplete(() => {
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 
            Tween.StopAll(); });
        Mover[] movers = FindObjectsOfType<Mover>();
        foreach (Mover mover in movers)
        {
            mover.gameOver = true; // Replace with the actual function you want to call
        }
    }

    public void Pause()
    {
        if (Time.timeScale == 1f)
        {
            //Pause
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            eventSystem.SetSelectedGameObject(pauseButton);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    public void Quit()
    {
        Tween.StopAll();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        Tween.StopAll();
        SceneManager.LoadScene("Game");
    }

}