package com.doublethink.Application;

import android.app.Application;

import com.doublethink.utils.DataParams;
import com.doublethink.utils.DeviceUtils;
import com.xmiles.antiaddictionsdk.api.AntiAddictionAPI;
import com.xmiles.sceneadsdk.adcore.core.SceneAdParams;
import com.xmiles.sceneadsdk.adcore.core.SceneAdSdk;
import com.xmiles.sceneadsdk.deviceActivate.IPrejudgeNatureCallback;
import com.xmiles.sceneadsdk.deviceActivate.PrejudgeNatureBean;

public class GameApplication extends Application {
    @Override
    public void onCreate() {
        super.onCreate();
        preInit();
    }

    /**
     * Pre-initialization
     */
    private void preInit() {
        // 合规处理：调用后，sdk会判断是否已经同意隐私弹窗来禁用androidid，记得必须是用中台的隐私弹窗才能调用
        SceneAdSdk.checkAndroidIdInner(this);
        SceneAdSdk.preInit(this, getSceneAdParams(this));
        SceneAdSdk.prejudgeNatureChannel(new IPrejudgeNatureCallback() {
            @Override
            public void attributionCallback(PrejudgeNatureBean prejudgeNatureBean) {

            }
        });
        // 防沉迷需要监听页面
        AntiAddictionAPI.getInstance().initLifeCycle(this);
    }

    private static SceneAdParams getSceneAdParams(Application context) {
        SceneAdParams params = SceneAdParams.builder()
                .isDebug(DataParams.isDebug)
                .netMode(DataParams.iNetMode)
                .prdid(DataParams.productId)
                //version name
                .appVersion(DeviceUtils.getVersionName(context))
                //version build
                .appVersionCode(DeviceUtils.getVersionCode(context))
                .appName(DataParams.gameName)
                .canShowNotification(false)
                .needKeeplive(false) // 是否保活
                // 开启内置oaid获取
                .needInitOaid(true)
                // 如果自己没有代码申请READ_PHONE_STATE，则设置为false
                .needRequestIMEI(false)
                // 如果自己接入了神策安卓sdk，则设置为false
                .enableInnerTrack(false)
                .build();
        return params;
    }
}
