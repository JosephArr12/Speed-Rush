using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    public GameObject obstacle;
    public Vector3 obstacleSpawn;
    public Vector2 scaleRange;
    public Vector2 xRange;
    public Vector3 obstacleScale;
    public float lowHight;
    public float highHight;
    public float xRandom;
    public float yRandom;
    public float spawnTime = 2f;
    public float maxSpawnTime = 2f;
    public float minSpawnTime = 0.1f;
    public float spawnDecrease = 0.1f;
    public float difficultyLevel = 0;
    private Coroutine spawnCoroutine;
    public TextMeshProUGUI waveText;
    public GameObject collectibles;
    bool gameOver;

    private void Awake()
    {
        instance = this;
        GameManager.gameManager.gameOver.AddListener(EndGame);
    }

    void EndGame()
    {
        gameOver = true;
    }

    void Start()
    {
        StartWave();
    }
    public int secondsAfterWave;
    public int waveTime = 15;
    public int betweenWavesTime = 15;
    public static bool waveActive = false;
    public int lastWave;
    IEnumerator SpawnLoop()
    {

        // Spawn obstacles during the wave time
        while (!gameOver)
        {
            Spawn();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public float collectiblesOffset;
    public float collectiblesHeight;
    void Spawn()
    {
        GameObject o = Instantiate(obstacle, obstacleSpawn, Quaternion.identity);
        float s = Random.Range(scaleRange.x, scaleRange.y);
        float i = Mathf.InverseLerp(scaleRange.x, scaleRange.y, s);
        float movementRange = Mathf.Lerp(xRange.x, xRange.y, i);
        int heightChoice = Random.Range(0, 2);
        float height;
        if (heightChoice == 0)
        {
            height = lowHight;
        }
        else
        {
            height = highHight;
        }
        o.transform.localScale = new Vector3(Random.Range(scaleRange.x, scaleRange.y), o.transform.localScale.y, o.transform.localScale.z);
        o.transform.position = new Vector3(Random.Range(-movementRange, movementRange), height, o.transform.position.z);
        Color randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        o.transform.GetComponent<MeshRenderer>().material.color = randomColor;
        o.GetComponent<Mover>().SetDifficultyLevel(difficultyLevel);

        //Spawn collecticles
        GameObject collectible = Instantiate(collectibles);
        collectible.transform.SetPositionAndRotation(new(o.transform.position.x, collectiblesHeight, o.transform.position.z + collectiblesOffset), Quaternion.identity);
        collectible.transform.SetParent(o.transform, true);
 
    }

    public void StartWave()
    {
        if (gameOver) { return; }
        Debug.Log("new wave");
        difficultyLevel = difficultyLevel + 1f;
        spawnTime -= spawnDecrease;
        spawnTime = Mathf.Clamp(spawnTime, minSpawnTime, maxSpawnTime);
        Tween.Delay(secondsAfterWave - 1f, () =>
        {
            SoundManager.instance.PlayNextLevel();
            waveText.gameObject.SetActive(true);
        });
        if (difficultyLevel > lastWave)
        {
            waveText.SetText("Final Wave Starting!");
        }
        else
        {
            waveText.SetText("Wave " + difficultyLevel + " Starting!");
        }

        

        Tween.Delay(secondsAfterWave + 1f, () =>
        {
            
            waveText.gameObject.SetActive(false);
            spawnCoroutine = StartCoroutine(SpawnLoop());
            //StartCoroutine(SpawnLoop());
            waveActive = true;


            if (difficultyLevel > lastWave)
            {
                return;
            }
            Tween.Delay(waveTime, () => {
                waveActive = false;
                if (spawnCoroutine != null)
                {
                    StopCoroutine(spawnCoroutine);
                    Tween.Delay(betweenWavesTime, () =>
                    {
                        StartWave();
                    });
                    
                }
            });
        });

        

 

    }

    public void StopSpawning()
    {
        //Game over
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
            gameOver = true;
    }
}