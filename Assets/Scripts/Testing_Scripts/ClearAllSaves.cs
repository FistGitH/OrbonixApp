using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class ClearAllSaves : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        AccountData.Logout();

        Debug.Log("All saved account data cleared.");
    }
}