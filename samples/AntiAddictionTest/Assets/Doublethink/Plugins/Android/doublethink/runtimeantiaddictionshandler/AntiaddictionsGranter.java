package com.doublethink.runtimeantiaddictionshandler;

import com.unity3d.player.UnityPlayer;
import com.xmiles.antiaddictionsdk.api.AntiAddictionAPI;
import com.xmiles.antiaddictionsdk.api.ILogoutCallback;
import com.xmiles.antiaddictionsdk.api.LoginCallbackAdapter;
import com.xmiles.antiaddictionsdk.net.decode.GameAccountLoginResponse;

public class AntiaddictionsGranter {
    
    public static boolean Android_ShowLogin(){
        //入口页面 建议在游戏主页面 调用登陆，
         return AntiAddictionAPI.getInstance().checkAndLogin(new LoginCallbackAdapter() {
            @Override
            public void onUserProtocolClicked() {
                UnitySendMessage("SinglePolicyCallbackInternal","UserAgreement");
            }

            @Override
            public void onPrivacyPolicyClicked() {
                UnitySendMessage("SinglePolicyCallbackInternal","PrivacyAgreement");
            }

            @Override
            public void onLoginSuccess(GameAccountLoginResponse response) {
                String strParam = AntiAddictionAPI.getInstance().getUserName()+","
                        +response.getIsAdult()+","
                        +AntiAddictionAPI.getInstance().getUserDeviceId()+","
                        +AntiAddictionAPI.getInstance().getUserToken();
                UnitySendMessage("SingleLoginSuccessCallbackInternal",strParam);
            }

            @Override
            public void onRegisterSuccess() {
                UnitySendMessage("SingleRegisterSuccessCallbackInternal","Success");
            }

            @Override
            public void onLoginFailed(String errorMsg) {
                UnitySendMessage("SingleLoginFailedCallbackInternal",errorMsg);
            }
        });
    }

    public static void Android_ShowLoginOut(){
        AntiAddictionAPI.getInstance().logout(new ILogoutCallback() {
            @Override
            public void onLogout() {
                UnitySendMessage("SingleLoginOutSuccessCallbackInternal","Success");
            }
        });
    }

    private static void UnitySendMessage(String func, String strParam){
        UnityPlayer.UnitySendMessage("AntiaddictionRequesterListener", func, strParam);
    }
}