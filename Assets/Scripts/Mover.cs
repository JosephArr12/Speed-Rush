using UnityEngine;
using UnityEngine.Events;
public class Mover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 10f;
    public bool gameOver;
    public float speedIncrease = 10f;
    public float maxSpeed = 50f;
    public float currentSpeed;
    //

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            transform.position += Vector3.back * currentSpeed * Time.deltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            gameOver = true;
            Debug.Log("Hit player");
            GameManager.gameManager.GameOver();
            Spawner.instance.StopSpawning();
            Score.instance.SetFinalStats();
           //Time.timeScale = 0f;
        }
    }

    public void SetDifficultyLevel(float difficultyLevel)
    {
        currentSpeed = Mathf.Clamp(currentSpeed + (speedIncrease * difficultyLevel), speed, maxSpeed);
    }
}