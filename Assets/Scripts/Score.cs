using UnityEngine;
using TMPro;
using PrimeTween;
using UnityEngine.Events;
public class Score : MonoBehaviour
{
    public static Score instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool gameStarted;
    int score;
    int prevScore;
    int coinScore;
    int coinsCollected;
    int combinedScore;
    public int increaseDifficultyAmount;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreToBeatText;
    public TextMeshProUGUI coinsCollectedText;
    public TextMeshProUGUI finalScoreText;
    public  AudioSource[] allAudioSources;
    bool gameOver;
    bool beatHighScore;
    PlayerData loadedData;
    public UnityEvent coinCollected;

    public void SaveDataToPrefs()
    {
        PlayerData data = loadedData;
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }

    private void Awake()
    {
        instance = this;
        loadedData = LoadAllData();
    }

    private void Start()
    {
        // Find all active AudioSource components in the scene and store them in the array
        allAudioSources = FindObjectsOfType<AudioSource>();

        // You can now iterate through the array and perform operations on each AudioSource
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.mute = loadedData.mute;
        }

        Player.startGame.AddListener(StartGame);

        if (loadedData.highScore <= 0)
        {
            scoreToBeatText.gameObject.SetActive(false);
        }
        else
        {
            scoreToBeatText.gameObject.SetActive(true);
            scoreToBeatText.SetText("Score to beat : " + loadedData.highScore.ToString());
        }
    }

    PlayerData LoadAllData()
    {
        string json = PlayerPrefs.GetString("PlayerData", "");
        if (!string.IsNullOrEmpty(json))
        {
            return JsonUtility.FromJson<PlayerData>(json);
        }
        return new PlayerData(); // Default scores will be 0
    }

    void StartGame()
    {
        InvokeRepeating(nameof(AddScore), 0f, 0.05f);
    }

    void AddScore()
    {
        if (gameOver) { return; }
        score += 1;
        if(score >= increaseDifficultyAmount + prevScore)
        {
            prevScore = score;
            Debug.Log("Increase speed");
        }
        combinedScore = score + coinScore;
        scoreText.SetText(combinedScore.ToString());
        CheckAgainstHighScore();
    }

    void CheckAgainstHighScore()
    {
        if(combinedScore > loadedData.highScore && !beatHighScore)
        {
            scoreToBeatText.gameObject.SetActive(false);
            beatHighScore = true;
            Tween.PunchScale(scoreToBeatText.transform, Vector3.one * 3f, 1f, 10);

        }
    }


    public void CoinCollected()
    {
        if (gameOver) { return; }
        coinsCollected++;
        coinScore += 20;
        combinedScore = score + coinScore;
        scoreText.SetText(combinedScore.ToString());
        coinCollected?.Invoke();
        CheckAgainstHighScore();
    }

    public void SetFinalStats()
    {
        gameOver = true;
        coinsCollectedText.SetText("Coins Collected : " + coinsCollected.ToString());
        finalScoreText.SetText("Final Score : " + combinedScore.ToString());
        if(combinedScore > loadedData.highScore)
        {
            loadedData.highScore = combinedScore;
        }
        SaveDataToPrefs();
    }
}