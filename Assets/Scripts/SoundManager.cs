using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource;
    public AudioSource footStepAudioSource;
    public AudioClip collectCoin;
    public AudioClip footsteps;
    public AudioClip jump;
    public AudioClip slide;
    public AudioClip land;
    public AudioClip gameOver;
    public AudioClip nextLevel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Score.instance.coinCollected.AddListener(PlayCollectCoin);
        PlayFootStepSounds();
    }

    public void PlayFootStepSounds()
    {
        if (GameManager.gameManager.isGameOver) { return; }
            footStepAudioSource.Play();
    }

    void PlayCollectCoin()
    {
        audioSource.PlayOneShot(collectCoin);

    }

    public void PlaySlide()
    {
        footStepAudioSource.Stop();
        audioSource.PlayOneShot(slide);
    }

    public void PlayJump()
    {
        footStepAudioSource.Stop();
        audioSource.PlayOneShot(jump);
    }

    public void PlayLand()
    {
        footStepAudioSource.Stop();
        audioSource.PlayOneShot(land);
    }
    public void PlayNextLevel()
    {
        audioSource.PlayOneShot(nextLevel);
    }
    public void GameOver() 
    {
        audioSource.PlayOneShot(gameOver);
        footStepAudioSource.Stop();
    }
}