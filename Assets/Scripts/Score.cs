using UnityEngine;
using TMPro;
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
    public TextMeshProUGUI coinsCollectedText;
    public TextMeshProUGUI finalScoreText;
    bool gameOver;

    PlayerData loadedData;
    public UnityEvent coinCollected;
    public void SetAudio(bool audio)
    {
        loadedData.audio = audio;
        SaveDataToPrefs();
    }
    public bool GetAudio()
    {
        return loadedData.audio;
    }

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
        
        Player.startGame.AddListener(StartGame);
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
    }


    public void CoinCollected()
    {
        if (gameOver) { return; }
        coinsCollected++;
        coinScore += 20;
        combinedScore = score + coinScore;
        scoreText.SetText(combinedScore.ToString());
        coinCollected?.Invoke();
    }

    public void SetFinalStats()
    {
        gameOver = true;
        coinsCollectedText.SetText("Coins Collected:" + coinsCollected.ToString());
        finalScoreText.SetText("Final Score:" + combinedScore.ToString());
        loadedData.highScore = combinedScore;
        SaveDataToPrefs();
    }
}