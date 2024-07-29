using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/BallData", fileName = "Ball Data")]
public class BallData : ScriptableObject
{
    //public Transform startPosition;
    //public Vector3 endPosition;
    //public Transform middlePosition;

    public float force = 40f;
    public float breakForce = 20f;
}
