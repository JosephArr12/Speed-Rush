using UnityEngine;
using PrimeTween;
public class UIAnimations : MonoBehaviour
{
    public float animationDuration;
    public float animationStrength;

    public void SelectAnimation(GameObject selectable)
    {
        Tween.Scale(selectable.transform, Vector3.one * animationStrength, animationDuration, useUnscaledTime:true);
    }

    public void DeselectAnimation(GameObject selectable)
    {
        Tween.Scale(selectable.transform, Vector3.one, animationDuration, useUnscaledTime: true);
    }
}

