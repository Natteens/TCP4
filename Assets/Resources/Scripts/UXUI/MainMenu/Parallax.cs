using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float offsetMultiplier = 1f;
    public float smoothTime = .3f;

    private Vector3 startPosition;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector2 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPosition = startPosition + new Vector3(offset.x, offset.y, 0) * offsetMultiplier;
        targetPosition.z = startPosition.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
