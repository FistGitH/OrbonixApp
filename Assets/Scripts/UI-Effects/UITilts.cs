using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UITilt : MonoBehaviour
{
    [Header("Максимальное смещение UI")]
    public Vector2 maxOffset = new Vector2(30f, 30f);

    [Header("Угол для максимального смещения")]
    [Range(1f, 60f)]
    public float maxAngle = 20f;

    [Header("Плавность движения")]
    [Min(0.1f)]
    public float smoothness = 8f;

    public bool invertX;
    public bool invertY;

    private RectTransform rect;
    private Vector2 initialPosition;
    private Quaternion neutralRotation;
    private float calibrationTime;
    private bool calibrated;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        initialPosition = rect.anchoredPosition;
    }

    private void OnEnable()
    {
        if (!SystemInfo.supportsGyroscope)
        {
            Debug.LogWarning("На устройстве нет гироскопа.", this);
            enabled = false;
            return;
        }

        Input.gyro.enabled = true;
        Recalibrate();
    }

    // Можно также вызвать из кнопки UI.
    public void Recalibrate()
    {
        calibrated = false;

        // Даем датчику время начать выдавать данные.
        calibrationTime = Time.unscaledTime + 0.5f;
    }

    private void Update()
    {
        if (!calibrated)
        {
            if (Time.unscaledTime < calibrationTime)
                return;

            neutralRotation = GetRotation();
            calibrated = true;
        }

        Quaternion difference =
            Quaternion.Inverse(neutralRotation) * GetRotation();

        Vector3 angles = difference.eulerAngles;

        float horizontal = Mathf.DeltaAngle(0f, angles.y);
        float vertical = -Mathf.DeltaAngle(0f, angles.x);

        if (invertX) horizontal = -horizontal;
        if (invertY) vertical = -vertical;

        float angleLimit = Mathf.Max(1f, maxAngle);

        Vector2 offset = new Vector2(
            Mathf.Clamp(horizontal / angleLimit, -1f, 1f) * maxOffset.x,
            Mathf.Clamp(vertical / angleLimit, -1f, 1f) * maxOffset.y
        );

        float smoothing =
            1f - Mathf.Exp(-smoothness * Time.unscaledDeltaTime);

        rect.anchoredPosition = Vector2.Lerp(
            rect.anchoredPosition,
            initialPosition + offset,
            smoothing
        );
    }

    private static Quaternion GetRotation()
    {
        Quaternion q = Input.gyro.attitude;

        // Перевод системы координат гироскопа в систему Unity.
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && isActiveAndEnabled)
            Recalibrate();
    }

    private void OnDisable()
    {
        if (rect != null)
            rect.anchoredPosition = initialPosition;
    }
}