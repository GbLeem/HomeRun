using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/BallDataV2", fileName = "Ball Data V2")]
public class BallDataV2 : ScriptableObject
{
    public float force = 40f;
    public Vector2 StrikeZoneSize = new Vector2(0.1f, 0.1f);

    public Vector2 maxStrikeZone;
    public Vector2 minStrikeZone;

    //breaking ball data
    public float breakingballForce = 2f;
    public float breakingballDuration = 0.5f;

    //fork ball
    public float delayTime = 0f;

    public Vector3 forceDir;

    public string ballname;

    //curve ball
    public Vector3 curveForce = new Vector3(0, 0, 0);
}
