using UnityEngine;

public class SelectedArrow : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1.0f;
    [SerializeField] private float moveSpeed = 0.5f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.localPosition = startPosition + new Vector3(0, offset, 0);
    }
}
