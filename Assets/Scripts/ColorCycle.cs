using UnityEngine;

public class ColorCycle : MonoBehaviour
{
    public Renderer targetRenderer; // Assign this in the inspector (like a cube or sphere)
    public float cycleSpeed = 1f; // Adjust to control how fast the color changes

    private void Update()
    {
        float hue = Mathf.Repeat(Time.time * cycleSpeed, 1f); // Loops between 0 and 1
        Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and value
        targetRenderer.material.color = color;
    }
}