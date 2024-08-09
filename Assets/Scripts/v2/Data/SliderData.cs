using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/SliderData", fileName = "Slider Data")]
public class SliderData : ScriptableObject
{
    public float force = 20f;
    public Vector2 StrikeZoneSize = new Vector2(0.1f, 0.1f);

    //slider data
    public float sliderForce = 2f;
    public float sliderDuration = 0.5f;
}
