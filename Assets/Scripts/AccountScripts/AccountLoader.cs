using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AccountLoader : MonoBehaviour
{
    [SerializeField]
    private AccountAPI accountAPI;

    private bool isCheckingAccount = false;


    private void Awake()
    {
        bool hasSavedAccount =
            AccountData.LoadAccount();


        if (!hasSavedAccount)
        {
            Debug.Log(
                "No saved account."
            );

            return;
        }


        Debug.Log(
            "Saved account found. Checking session..."
        );


        StartCoroutine(
            CheckSavedAccount()
        );
    }


    private IEnumerator CheckSavedAccount()
    {
        if (isCheckingAccount)
            yield break;


        isCheckingAccount = true;


        if (accountAPI == null)
        {
            Debug.LogError(
                "AccountAPI is not assigned."
            );

            isCheckingAccount = false;

            yield break;
        }


        bool finished = false;
        bool success = false;


        yield return StartCoroutine(
            accountAPI.GetMe(
                response =>
                {
                    finished = true;


                    if (response == null)
                    {
                        Debug.LogWarning(
                            "Could not check saved account."
                        );

                        return;
                    }


                    if (!string.IsNullOrEmpty(
                        response.error))
                    {
                        Debug.LogWarning(
                            response.error
                        );

                        return;
                    }


                    if (response.user == null)
                    {
                        return;
                    }


                    // Обновляем данные с сервера
                    AccountData.Id =
                        response.user.id;

                    AccountData.FirstName =
                        response.user.firstName;

                    AccountData.LastName =
                        response.user.lastName;

                    AccountData.Email =
                        response.user.email;

                    AccountData.Language =
                        response.user.language;

                    AccountData.IsLoggedIn = true;

                    AccountData.SaveAccount();

                    success = true;
                }
            )
        );


        isCheckingAccount = false;


        if (finished && success)
        {
            Debug.Log(
                "Account restored: " +
                AccountData.Email
            );


            SceneManager.LoadScene(
                "HomeMenu_Scene"
            );
        }
        else
        {
            Debug.LogWarning(
                "Saved session is no longer valid."
            );

            // Token больше не работает
            AccountData.Logout();
        }
    }
}