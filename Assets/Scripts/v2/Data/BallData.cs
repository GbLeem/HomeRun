using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/BallData", fileName = "Ball Data")]
public class BallData : ScriptableObject
{
    //public Transform startPosition;
    //public Vector3 endPosition;
    //public Transform middlePosition;

    public float force = 40f;
    public float breakForce = 20f;
    public Vector2 StrikeZoneSize = new Vector2(0.1f, 0.1f);

    //slider data
    public float sliderForce = 2f;
    public float sliderDuration = 0.5f;
    
    //fork ball data
    public float forkForce = 2f;
    public float forkDuration = 0.5f;
}
