
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/ForkData", fileName = "Fork Data")]
public class ForkData : ScriptableObject
{
    public float force = 20f;
    public Vector2 StrikeZoneSize = new Vector2(0.1f, 0.1f);

    //fork ball data
    public float forkForce = 4f;
    public float forkDuration = 0.5f;
}
