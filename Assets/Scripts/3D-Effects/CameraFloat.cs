using UnityEngine;

public class CameraFloat : MonoBehaviour
{
    [Header("Размах движения по каждой оси")]
    [Range(0f, 0.5f)] public float amplitudeX = 0.15f;
    [Range(0f, 0.5f)] public float amplitudeY = 0.2f;
    [Range(0f, 0.5f)] public float amplitudeZ = 0.1f;

    [Header("Скорость покачивания")]
    [Range(0.05f, 2f)] public float speed = 0.4f;

    private Vector3 startPosition;
    private float time;

    private void OnEnable()
    {
        startPosition = transform.localPosition;
        time = 0f;
    }

    private void Update()
    {
        time += Time.deltaTime * speed;

        Vector3 offset = new Vector3(
            Mathf.Sin(time * 0.8f) * amplitudeX,
            Mathf.Sin(time * 1.1f) * amplitudeY,
            Mathf.Sin(time * 0.6f) * amplitudeZ
        );

        transform.localPosition = startPosition + offset;
    }

    private void OnDisable()
    {
        transform.localPosition = startPosition;
    }
}