using UnityEngine;

public class RegisterOrLogin : MonoBehaviour
{
    [Header("Login and Register")]
    [SerializeField] private GameObject RegisterPanel;
    [SerializeField] private GameObject LoginPanel;

    [Header("Buttons")]
    [SerializeField] private GameObject RegisterText;
    [SerializeField] private GameObject LoginText;




    private void Awake()
    {
        StartRegistartion();
    }

    public void StartRegistartion()
    {
        RegisterPanel.SetActive(true);
        LoginPanel.SetActive(false);

        RegisterText.SetActive(true);
        LoginText.SetActive(false);
    }

    public void StartLogin()
    {
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(true);

        RegisterText.SetActive(false);
        LoginText.SetActive(true);
    }
}
