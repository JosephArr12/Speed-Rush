using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using PrimeTween;
[System.Serializable]
public class PlayerData
{
    public int highScore;
    //if mute is false audio is on, if mute is true audio is off
    public bool mute;
}

    public class MenuManager : MonoBehaviour
{
    public GameObject controlsScreen;
    public GameObject mainScreen;
    public TextMeshProUGUI highScore;
    public AudioSource source;
    public Toggle audioToggle;

    PlayerData loadedData;

    PlayerData LoadAllData()
    {
        string json = PlayerPrefs.GetString("PlayerData", "");
        if (!string.IsNullOrEmpty(json))
        {
            return JsonUtility.FromJson<PlayerData>(json);
        }
        return new PlayerData(); // Default scores will be 0
    }

    public void SaveDataToPrefs()
    {
        PlayerData data = loadedData;
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }

    private void Start()
    {
        
        loadedData = LoadAllData();
        highScore.SetText("High Score: " + loadedData.highScore.ToString());
        audioToggle.isOn = !loadedData.mute;
        source.mute = !audioToggle.isOn;
    }
    public void Controls()
    {
        controlsScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    public void Audio()
    {
        source.mute = !audioToggle.isOn;
        loadedData.mute = source.mute;
        SaveDataToPrefs();
    }

    public void Main()
    {
        mainScreen.SetActive(true);
        controlsScreen.SetActive(false);
    }

    public void Play()
    {
        Tween.Delay(0.5f,()=> { SceneManager.LoadScene("Game"); });
       
    }

    public void Quit()
    {
        Tween.Delay(0.5f, () => { Application.Quit(); });
    }

    public float animationDuration;
    public float animationStrength;

    public void SelectAnimation(GameObject selectable)
    {
        Tween.Scale(selectable.transform, Vector3.one * animationStrength, animationDuration);
    }

    public void DeselectAnimation(GameObject selectable)
    {
        Tween.Scale(selectable.transform, Vector3.one, animationDuration);
    }
}