using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance;

    private void Awake()
    {
        // Если AccountManager уже существует,
        // второй экземпляр уничтожаем
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Не уничтожать при смене сцен
        DontDestroyOnLoad(gameObject);

        Debug.Log("AccountManager initialized.");
    }


    public bool IsLoggedIn()
    {
        return AccountData.IsLoggedIn;
    }


    public string GetFirstName()
    {
        return AccountData.FirstName;
    }


    public string GetLastName()
    {
        return AccountData.LastName;
    }


    public string GetEmail()
    {
        return AccountData.Email;
    }


    public string GetUserId()
    {
        return AccountData.Id;
    }


    public string GetLanguage()
    {
        return AccountData.Language;
    }


    public string GetToken()
    {
        return AccountData.Token;
    }
}