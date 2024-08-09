using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/FourSeamData", fileName = "FourSeam Data")]
public class FourSeamData : ScriptableObject
{
    public float force = 20f;
    public Vector2 StrikeZoneSize = new Vector2(0.1f, 0.1f);
}
