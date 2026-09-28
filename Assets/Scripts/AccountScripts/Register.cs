using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Net.Mail;

public class Register : MonoBehaviour
{
    [Header("API")]
    [SerializeField]
    private AccountAPI accountAPI;


    [Header("Input Fields")]
    [SerializeField]
    private TMP_InputField firstNameInput;

    [SerializeField]
    private TMP_InputField lastNameInput;

    [SerializeField]
    private TMP_InputField emailInput;

    [SerializeField]
    private TMP_InputField passwordInput;


    [Header("UI")]
    [SerializeField]
    private TMP_Text messageText;


    private bool isRegistering;


    public void RegisterAccount()
    {
        if (isRegistering)
            return;


        string firstName =
            firstNameInput.text.Trim();

        string lastName =
            lastNameInput.text.Trim();

        string email =
            emailInput.text.Trim().ToLower();

        string password =
            passwordInput.text;


        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(email) ||
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


        if (password.Length < 12 ||
            password.Length > 128)
        {
            ShowMessage(
                "Password must contain 12–128 characters."
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


        isRegistering = true;

        ShowMessage(
            "Creating account..."
        );


        StartCoroutine(
            accountAPI.Register(
                firstName,
                lastName,
                email,
                password,

                response =>
                {
                    isRegistering = false;


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
                        "Account created: " +
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