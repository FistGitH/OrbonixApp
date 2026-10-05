using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NavigationPanel : MonoBehaviour
{
    [Header("More Panel")]
    [SerializeField] private GameObject morePanel;
    [SerializeField] private CanvasGroup morePanelCanvasGroup;
    [SerializeField] private RectTransform morePanelRect;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.25f;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxbutton;

    private Coroutine fadeCoroutine;
    private bool isOpenMorePanel;

    private void Start()
    {
        //More button
        isOpenMorePanel = false;

        if (morePanelCanvasGroup != null)
        {
            morePanelCanvasGroup.alpha = 0f;
            morePanelCanvasGroup.interactable = false;
            morePanelCanvasGroup.blocksRaycasts = false;
        }

        if (morePanel != null)
            morePanel.SetActive(false);

        //Home button


        //AI button
    }

    private void Update()
    {
        if (!isOpenMorePanel)
            return;

        CheckIfClickOutsideMore();
    }

    //==============MORE BUTTON==============

    private void CheckIfClickOutsideMore()
    {
        // Новый Input System
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition =
                Touchscreen.current.primaryTouch.position.ReadValue();

            CheckOutsidePanelMore(touchPosition);
        }

        // Мышь для тестирования в Unity Editor
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            CheckOutsidePanelMore(mousePosition);
        }
    }
    private void CheckOutsidePanelMore(Vector2 screenPosition)
    {
        // Если нажали непосредственно внутри панели
        if (morePanelRect != null &&
            RectTransformUtility.RectangleContainsScreenPoint(
                morePanelRect,
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
        if (isOpenMorePanel)
            return;

        isOpenMorePanel = true;

        sfxbutton?.Play();

        if (morePanel != null)
            morePanel.SetActive(true);

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(
            FadeMorePanel(0f, 1f)
        );
    }
    public void CloseMorePanel()
    {
        if (!isOpenMorePanel)
            return;

        isOpenMorePanel = false;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(
            FadeMorePanel(1f, 0f)
        );
    }
    private IEnumerator FadeMorePanel(float startAlpha, float targetAlpha)
    {
        float time = 0f;

        // Пока появляется — можно взаимодействовать
        morePanelCanvasGroup.interactable = targetAlpha > 0f;
        morePanelCanvasGroup.blocksRaycasts = targetAlpha > 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float progress = time / fadeDuration;

            // Плавная анимация
            progress = Mathf.SmoothStep(0f, 1f, progress);

            morePanelCanvasGroup.alpha =
                Mathf.Lerp(startAlpha, targetAlpha, progress);

            yield return null;
        }

        morePanelCanvasGroup.alpha = targetAlpha;

        if (targetAlpha <= 0f)
        {
            morePanelCanvasGroup.interactable = false;
            morePanelCanvasGroup.blocksRaycasts = false;

            if (morePanel != null)
                morePanel.SetActive(false);
        }

        fadeCoroutine = null;
    }


}