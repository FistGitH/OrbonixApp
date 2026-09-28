using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Net.Mail;

public class Login : MonoBehaviour
{
    [Header("API")]
    [SerializeField]
    private AccountAPI accountAPI;


    [Header("Input Fields")]
    [SerializeField]
    private TMP_InputField emailInput;

    [SerializeField]
    private TMP_InputField passwordInput;


    [Header("UI")]
    [SerializeField]
    private TMP_Text messageText;


    private bool isLoggingIn;


    public void LoginAccount()
    {
        if (isLoggingIn)
            return;


        string email =
            emailInput.text.Trim().ToLower();

        string password =
            passwordInput.text;


        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            ShowMessage(
                "Please fill in all fields."
            );

            return;
        }


        if (!IsValidEmail(email))
        {
            ShowMessage(
                "Enter a valid email address."
            );

            return;
        }


        if (accountAPI == null)
        {
            ShowMessage(
                "Account API is not connected."
            );

            return;
        }


        isLoggingIn = true;

        ShowMessage("Signing in...");


        StartCoroutine(
            accountAPI.Login(
                email,
                password,

                response =>
                {
                    isLoggingIn = false;


                    if (response == null)
                    {
                        ShowMessage(
                            "Server error."
                        );

                        return;
                    }


                    if (!string.IsNullOrEmpty(
                        response.error))
                    {
                        ShowMessage(
                            response.error
                        );

                        return;
                    }


                    if (response.user == null ||
                        string.IsNullOrEmpty(
                            response.token))
                    {
                        ShowMessage(
                            "Invalid server response."
                        );

                        return;
                    }


                    // Сохраняем аккаунт
                    AccountData.SetAccount(
                        response.user.id,
                        response.user.firstName,
                        response.user.lastName,
                        response.user.email,
                        response.user.language,
                        response.token
                    );


                    passwordInput.text = "";


                    Debug.Log(
                        "Logged in: " +
                        AccountData.Email
                    );


                    // Переходим в Home Menu
                    SceneManager.LoadScene(
                        "HomeMenu_Scene"
                    );
                }
            )
        );
    }


    private bool IsValidEmail(string email)
    {
        try
        {
            MailAddress address =
                new MailAddress(email);

            return address.Address == email;
        }
        catch
        {
            return false;
        }
    }


    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }

        Debug.Log(message);
    }
}