using System;
using Doublethink.Scripts.Extentions;
using UnityEngine;

namespace Doublethink.Scripts.Services.Antiaddictions
{
    public class AntiaddictionsHandler : MonoBehaviour
    {
        private const String PluginListenerName = "AntiaddictionRequesterListener"; // must match UnitySendMessage call in Java

        private AndroidJavaClass _antiaddictionsGranterClass;
        private AndroidJavaObject _currentActivity;

        public static Action<AgreementLinkEnum> OnOpenAgreementLink;
        public static Action<AntiadictiionsType> OnLoginSuccessFinished;
        public static Action OnRegisterSuccessFinished;
        public static Action<String> OnLoginFailedFinished;
        public static Action OnLoginOutSuccessFinished;

        public static AntiaddictionsHandler Instance;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _antiaddictionsGranterClass = new AndroidJavaClass("com.doublethink.runtimeantiaddictionshandler.AntiaddictionsGranter");
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayerActivity");
            _currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            if (name != PluginListenerName)
            {
                name = PluginListenerName;
            }
            DontDestroyOnLoad(gameObject);

            Initialization();
        }

        private void Initialization()
        {
            if (_currentActivity != null)
                Instance._antiaddictionsGranterClass.CallStatic("Android_Initialization", _currentActivity);
        }

        public Boolean Login(Boolean isAutoLogin = false)
        {
            return Instance._antiaddictionsGranterClass.CallStatic<Boolean>("Android_ShowLogin", isAutoLogin);
        }

        public void LoginOut()
        {
            Instance._antiaddictionsGranterClass.CallStatic("Android_ShowLoginOut");
        }

        #region Antiaddications callback
        private void SinglePolicyCallbackInternal(String result)
        {
            AntiaddicationsLogger("ready jump agreement link type: " + result);
            var agreementLinkEnum = AgreementLinkEnum.NONE;
            if (result.Equals("UserAgreement"))
            {
                agreementLinkEnum = AgreementLinkEnum.UserAgreement;
            }else if (result.Equals("PrivacyAgreement"))
            {
                agreementLinkEnum = AgreementLinkEnum.PrivacyAgreement;
            }
            OnOpenAgreementLink.SafeInvoke(agreementLinkEnum);
        }

        private void SingleLoginSuccessCallbackInternal(string result)
        {
            AntiaddicationsLogger("login success return data info: " + result);
            if (!String.IsNullOrEmpty(result)) {
                String[] arrResult = result.Split(',');
                AntiadictiionsType type = new AntiadictiionsType();
                type.userId = arrResult[0];
                type.isAdult = Boolean.Parse(arrResult[1]);
                type.userDeviceId = arrResult[2];
                type.UserToken = arrResult[2];
                OnLoginSuccessFinished.SafeInvoke(type);
            }
            else
            {
                OnLoginSuccessFinished.SafeInvoke(null);
            }
        }

        private void SingleRegisterSuccessCallbackInternal(String result)
        {
            AntiaddicationsLogger("register success");
            OnRegisterSuccessFinished.SafeInvoke();
        }
        private void SingleLoginFailedCallbackInternal(String result)
        {
            AntiaddicationsLogger("login failed reruen error code and error message: " + result);
            OnLoginFailedFinished.SafeInvoke(result);
        }

        private void SingleLoginOutSuccessCallbackInternal(String result)
        {
            AntiaddicationsLogger("loginout success");
            OnLoginOutSuccessFinished.SafeInvoke();
        }
        #endregion

        private void AntiaddicationsLogger(string msg)
        {
            Debug.Log("Antiaddications " + msg);
        }
    }
}
