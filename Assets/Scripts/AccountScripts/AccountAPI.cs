using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text;

public class AccountAPI : MonoBehaviour
{
    private const string BASE_URL = "https://orbonix.net";


    [Serializable]
    public class RegisterRequest
    {
        public string firstName;
        public string lastName;
        public string email;
        public string password;
        public string language;
        public string deviceName;
    }


    [Serializable]
    public class LoginRequest
    {
        public string email;
        public string password;
        public string deviceName;
    }


    [Serializable]
    public class UserData
    {
        public string id;
        public string firstName;
        public string lastName;
        public string email;
        public string language;
        public bool avatar;
    }


    [Serializable]
    public class AuthResponse
    {
        public string token;
        public int expiresIn;
        public UserData user;

        public string error;
    }


    [Serializable]
    public class MeResponse
    {
        public UserData user;

        public string error;
    }


    public IEnumerator Register(
        string firstName,
        string lastName,
        string email,
        string password,
        Action<AuthResponse> callback)
    {
        RegisterRequest data = new RegisterRequest
        {
            firstName = firstName,
            lastName = lastName,
            email = email,
            password = password,
            language = "en",
            deviceName = SystemInfo.deviceName
        };

        string json = JsonUtility.ToJson(data);

        using UnityWebRequest request =
            new UnityWebRequest(
                BASE_URL + "/api/app/register",
                "POST"
            );

        byte[] body =
            Encoding.UTF8.GetBytes(json);

        request.uploadHandler =
            new UploadHandlerRaw(body);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        yield return request.SendWebRequest();


        string responseText =
            request.downloadHandler.text;


        if (request.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                "Register error: " +
                responseText
            );

            AuthResponse errorResponse =
                JsonUtility.FromJson<AuthResponse>(
                    responseText
                );

            callback?.Invoke(errorResponse);

            yield break;
        }


        AuthResponse response =
            JsonUtility.FromJson<AuthResponse>(
                responseText
            );

        callback?.Invoke(response);
    }


    public IEnumerator Login(
        string email,
        string password,
        Action<AuthResponse> callback)
    {
        LoginRequest data = new LoginRequest
        {
            email = email,
            password = password,
            deviceName = SystemInfo.deviceName
        };

        string json =
            JsonUtility.ToJson(data);


        using UnityWebRequest request =
            new UnityWebRequest(
                BASE_URL + "/api/app/login",
                "POST"
            );

        byte[] body =
            Encoding.UTF8.GetBytes(json);

        request.uploadHandler =
            new UploadHandlerRaw(body);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );


        yield return request.SendWebRequest();


        string responseText =
            request.downloadHandler.text;


        if (request.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                "Login error: " +
                responseText
            );

            AuthResponse errorResponse =
                JsonUtility.FromJson<AuthResponse>(
                    responseText
                );

            callback?.Invoke(errorResponse);

            yield break;
        }


        AuthResponse response =
            JsonUtility.FromJson<AuthResponse>(
                responseText
            );

        callback?.Invoke(response);
    }


    public IEnumerator GetMe(
        Action<MeResponse> callback)
    {
        using UnityWebRequest request =
            UnityWebRequest.Get(
                BASE_URL + "/api/app/me"
            );


        request.SetRequestHeader(
            "Authorization",
            "Bearer " + AccountData.Token
        );


        yield return request.SendWebRequest();


        string responseText =
            request.downloadHandler.text;


        if (request.result !=
            UnityWebRequest.Result.Success)
        {
            MeResponse errorResponse =
                JsonUtility.FromJson<MeResponse>(
                    responseText
                );

            callback?.Invoke(errorResponse);

            yield break;
        }


        MeResponse response =
            JsonUtility.FromJson<MeResponse>(
                responseText
            );


        callback?.Invoke(response);
    }


    public IEnumerator Logout(
        Action<bool> callback = null)
    {
        using UnityWebRequest request =
            new UnityWebRequest(
                BASE_URL + "/api/app/logout",
                "POST"
            );

        request.downloadHandler =
            new DownloadHandlerBuffer();


        request.SetRequestHeader(
            "Authorization",
            "Bearer " + AccountData.Token
        );


        yield return request.SendWebRequest();


        bool success =
            request.result ==
            UnityWebRequest.Result.Success;


        if (success)
        {
            AccountData.Logout();
        }


        callback?.Invoke(success);
    }
}