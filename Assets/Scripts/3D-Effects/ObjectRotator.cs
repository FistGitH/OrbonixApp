using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 30f;

    [Tooltip("Выбери ось вращения объекта")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    private void Update()
    {
        transform.Rotate(
            rotationAxis.normalized,
            rotationSpeed * Time.deltaTime,
            Space.Self
        );
    }
}