using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class SmoothButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(0.75f, 0.75f, 0.75f);
    [SerializeField] private Color pressedColor = new Color(0.55f, 0.55f, 0.55f);

    [Header("Animation")]
    [SerializeField] private float colorSpeed = 8f;
    [SerializeField] private float pressScale = 0.92f;
    [SerializeField] private float animationSpeed = 12f;

    private Image image;
    private Vector3 originalScale;
    private Color targetColor;
    private Vector3 targetScale;

    private void Awake()
    {
        image = GetComponent<Image>();

        originalScale = transform.localScale;
        targetScale = originalScale;
        targetColor = normalColor;

        image.color = normalColor;
    }

    private void Update()
    {
        // Плавное изменение цвета
        image.color = Color.Lerp(
            image.color,
            targetColor,
            colorSpeed * Time.unscaledDeltaTime
        );

        // Плавное изменение размера
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            animationSpeed * Time.unscaledDeltaTime
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetColor = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetColor = normalColor;
        targetScale = originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetColor = pressedColor;
        targetScale = originalScale * pressScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetColor = hoverColor;

        // Небольшой красивый bounce
        StopAllCoroutines();
        StartCoroutine(ClickAnimation());
    }

    private IEnumerator ClickAnimation()
    {
        targetScale = originalScale * 1.04f;

        yield return new WaitForSecondsRealtime(0.08f);

        targetScale = originalScale;
    }
}