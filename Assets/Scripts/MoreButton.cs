using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MoreButton : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private RectTransform panelRect;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.25f;

    private Coroutine fadeCoroutine;
    private bool isOpen;

    private void Start()
    {
        isOpen = false;

        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;
        }

        if (panel != null)
            panel.SetActive(false);
    }

    private void Update()
    {
        if (!isOpen)
            return;

        // Новый Input System
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition =
                Touchscreen.current.primaryTouch.position.ReadValue();

            CheckOutsidePanel(touchPosition);
        }

        // Мышь для тестирования в Unity Editor
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            CheckOutsidePanel(mousePosition);
        }
    }

    private void CheckOutsidePanel(Vector2 screenPosition)
    {
        // Если нажали непосредственно внутри панели
        if (panelRect != null &&
            RectTransformUtility.RectangleContainsScreenPoint(
                panelRect,
                screenPosition,
                null))
        {
            return;
        }

        // Нажали вне панели → закрываем
        CloseMorePanel();
    }

    public void OpenMorePanel()
    {
        if (isOpen)
            return;

        isOpen = true;

        if (panel != null)
            panel.SetActive(true);

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(
            FadePanel(0f, 1f)
        );
    }

    public void CloseMorePanel()
    {
        if (!isOpen)
            return;

        isOpen = false;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(
            FadePanel(1f, 0f)
        );
    }

    private IEnumerator FadePanel(float startAlpha, float targetAlpha)
    {
        float time = 0f;

        // Пока появляется — можно взаимодействовать
        panelCanvasGroup.interactable = targetAlpha > 0f;
        panelCanvasGroup.blocksRaycasts = targetAlpha > 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float progress = time / fadeDuration;

            // Плавная анимация
            progress = Mathf.SmoothStep(0f, 1f, progress);

            panelCanvasGroup.alpha =
                Mathf.Lerp(startAlpha, targetAlpha, progress);

            yield return null;
        }

        panelCanvasGroup.alpha = targetAlpha;

        if (targetAlpha <= 0f)
        {
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;

            if (panel != null)
                panel.SetActive(false);
        }

        fadeCoroutine = null;
    }
}