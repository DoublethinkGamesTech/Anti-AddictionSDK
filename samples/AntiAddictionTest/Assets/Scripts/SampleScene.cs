using System.Collections;
using System.Collections.Generic;
using Doublethink.Scripts.Services.Antiaddictions;
using UnityEngine;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    [SerializeField]
    private GameObject _loginButton;
    [SerializeField]
    private Text _loginLable;
    private void Awake()
    {
        AntiaddictionsHandler.OnLoginSuccessFinished += OnLoginSuccessFinishedHandle;
        AntiaddictionsHandler.OnRegisterSuccessFinished += OnRegisterSuccessFinishedHandle;
        AntiaddictionsHandler.OnLoginFailedFinished += OnLoginFailedFinishedHandle;
        AntiaddictionsHandler.OnLoginOutSuccessFinished += OnLoginOutSuccessFinishedHandle;
        AntiaddictionsHandler.OnOpenAgreementLink += OnOpenAgreementLinkHandle;
    }

    private void OnDestroy()
    {
        AntiaddictionsHandler.OnLoginSuccessFinished -= OnLoginSuccessFinishedHandle;
        AntiaddictionsHandler.OnRegisterSuccessFinished -= OnRegisterSuccessFinishedHandle;
        AntiaddictionsHandler.OnLoginFailedFinished -= OnLoginFailedFinishedHandle;
        AntiaddictionsHandler.OnLoginOutSuccessFinished -= OnLoginOutSuccessFinishedHandle;
        AntiaddictionsHandler.OnOpenAgreementLink -= OnOpenAgreementLinkHandle;
    }

    private void Start()
    {
        _loginButton.gameObject.SetActive(true);
        if (IsLogin())
        {
            _loginButton.gameObject.SetActive(false);
        }
    }

    private void OnLoginSuccessFinishedHandle(AntiadictiionsType type)
    {
        Debug.Log("Sample login success: " + type.userId+","+type.isAdult+","+type.userDeviceId+","+type.UserToken);
        _loginLable.text = "user id: " + type.userId;
    }

    private void OnRegisterSuccessFinishedHandle()
    {
        Debug.Log("Sample register success");
    }

    private void OnLoginFailedFinishedHandle(string result)
    {
        Debug.Log("Sample login failed: " + result);
    }

    private void OnLoginOutSuccessFinishedHandle()
    {
        Debug.Log("Sample login out success");
        _loginButton.gameObject.SetActive(true);
    }

    private void OnOpenAgreementLinkHandle(AgreementLinkEnum linkEnum)
    {
        Debug.Log("Sample open agreement link: " + linkEnum.ToString());
        if (linkEnum == AgreementLinkEnum.UserAgreement)
        {
            Application.OpenURL("https://www.doublethinkgames.com/about-5");   
        }else if (linkEnum == AgreementLinkEnum.PrivacyAgreement)
        {
            Application.OpenURL("https://www.doublethinkgames.com/about-3");
        }
    }

    public void Login()
    {
        IsLogin();
    }

    public bool IsLogin()
    {
        return AntiaddictionsHandler.Instance.Login();
    }

    public void LoginOut()
    {
        AntiaddictionsHandler.Instance.LoginOut();
    }
}
