using UnityEngine;
using TMPro;

public class PasswordVisibility : MonoBehaviour
{
    [Header("Password Input Field")]
    [SerializeField] private TMP_InputField passwordInput;

    [Header("Password Hide/Open Icons")]
    [SerializeField] private GameObject OpenIcon;
    [SerializeField] private GameObject CloseIcon;


    private bool passwordVisible = false;


    private void Start()
    {
        OpenIcon.SetActive(false);
        HidePassword();
    }


    public void TogglePasswordVisibility()
    {
        passwordVisible = !passwordVisible;

        if (passwordVisible)
        {
            ShowPassword();
        }
        else
        {
            HidePassword();
        }

        // Обновляем отображение текста
        passwordInput.ForceLabelUpdate();
    }


    private void ShowPassword()
    {
        OpenIcon.SetActive(true);
        CloseIcon.SetActive(false);


        passwordInput.contentType =
            TMP_InputField.ContentType.Standard;

        passwordVisible = true;

        passwordInput.ForceLabelUpdate();
    }


    private void HidePassword()
    {
        OpenIcon.SetActive(false);
        CloseIcon.SetActive(true);

        passwordInput.contentType =
            TMP_InputField.ContentType.Password;

        passwordVisible = false;

        passwordInput.ForceLabelUpdate();
    }
}