using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        this.transform.localPosition = new Vector3(
            2050,
            this.transform.localPosition.y,
            this.transform.localPosition.z);
    }
}
