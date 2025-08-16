using UnityEngine;
using System.Collections.Generic;
using PrimeTween;
public class DestroyTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Mover>()){
            Destroy(other.gameObject);
            //Tween.Delay(0.5f, () => { CheckObstacles(); });
        }        
    }

    // if there are no more obstacles left and the game has stopped the wave, start the next wave
    void CheckObstacles()
    {
        // Find all objects of type MyComponent in the scene
        Mover[] components = FindObjectsByType<Mover>(FindObjectsSortMode.None);
        if(components.Length == 0 && Spawner.waveActive == false)
        {
            Debug.Log("STOP WAVE!");
            Debug.Log("Start next wave");
            FindObjectOfType<Spawner>().StartWave();
            //Start next wave;
        }
        Debug.Log(components.Length);
    }
}