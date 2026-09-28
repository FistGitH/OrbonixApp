using UnityEngine;

public static class AccountData
{
    public static string Id = "";
    public static string FirstName = "";
    public static string LastName = "";
    public static string Email = "";
    public static string Language = "en";
    public static string Token = "";

    public static bool IsLoggedIn = false;


    public static void SetAccount(
        string id,
        string firstName,
        string lastName,
        string email,
        string language,
        string token)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Language = language;
        Token = token;

        IsLoggedIn = true;

        SaveAccount();
    }


    public static void SaveAccount()
    {
        PlayerPrefs.SetString("Account_Id", Id);
        PlayerPrefs.SetString("Account_FirstName", FirstName);
        PlayerPrefs.SetString("Account_LastName", LastName);
        PlayerPrefs.SetString("Account_Email", Email);
        PlayerPrefs.SetString("Account_Language", Language);
        PlayerPrefs.SetString("Account_Token", Token);

        PlayerPrefs.SetInt(
            "Account_IsLoggedIn",
            IsLoggedIn ? 1 : 0
        );

        PlayerPrefs.Save();
    }


    public static bool LoadAccount()
    {
        if (!PlayerPrefs.HasKey("Account_Token"))
            return false;

        string savedToken =
            PlayerPrefs.GetString(
                "Account_Token",
                ""
            );

        if (string.IsNullOrEmpty(savedToken))
            return false;


        Id =
            PlayerPrefs.GetString(
                "Account_Id",
                ""
            );

        FirstName =
            PlayerPrefs.GetString(
                "Account_FirstName",
                ""
            );

        LastName =
            PlayerPrefs.GetString(
                "Account_LastName",
                ""
            );

        Email =
            PlayerPrefs.GetString(
                "Account_Email",
                ""
            );

        Language =
            PlayerPrefs.GetString(
                "Account_Language",
                "en"
            );

        Token = savedToken;

        IsLoggedIn =
            PlayerPrefs.GetInt(
                "Account_IsLoggedIn",
                0
            ) == 1;


        return IsLoggedIn;
    }


    public static void Logout()
    {
        Id = "";
        FirstName = "";
        LastName = "";
        Email = "";
        Language = "en";
        Token = "";

        IsLoggedIn = false;

        PlayerPrefs.DeleteKey("Account_Id");
        PlayerPrefs.DeleteKey("Account_FirstName");
        PlayerPrefs.DeleteKey("Account_LastName");
        PlayerPrefs.DeleteKey("Account_Email");
        PlayerPrefs.DeleteKey("Account_Language");
        PlayerPrefs.DeleteKey("Account_Token");
        PlayerPrefs.DeleteKey("Account_IsLoggedIn");

        PlayerPrefs.Save();
    }
}