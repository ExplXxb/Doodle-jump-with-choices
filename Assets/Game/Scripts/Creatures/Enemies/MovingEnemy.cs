using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    private void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(0.1f, 0f, 0f) * Time.fixedDeltaTime;
    }
}
