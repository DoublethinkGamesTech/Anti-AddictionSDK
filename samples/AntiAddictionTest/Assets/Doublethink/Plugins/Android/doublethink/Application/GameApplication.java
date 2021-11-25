package com.doublethink.Application;

import android.app.Application;

import com.doublethink.utils.DeviceUtils;
import com.xmiles.antiaddictionsdk.api.AntiAddictionAPI;
import com.xmiles.sceneadsdk.adcore.core.SceneAdParams;
import com.xmiles.sceneadsdk.adcore.core.SceneAdSdk;
import com.xmiles.sceneadsdk.base.sdk.INetMode;
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
                // 是否调试模式，会打印详细日志
                .isDebug(false)
                // 接口环境模式，INetMode.OFFICIAL、INetMode.TEST
                // debug包用INetMode.TEST
                .netMode(INetMode.OFFICIAL)
                //产品id
                .prdid("310302")
                //版本名
                .appVersion(DeviceUtils.getVersionName(context))
                //版本号
                .appVersionCode(DeviceUtils.getVersionCode(context))
                //应用名
                .appName("梦幻杂货店")
                .canShowNotification(false)
                .needKeeplive(false) // 是否保活
                // 开启内置oaid获取
                .needInitOaid(true)
                // 如果自己没有代码申请READ_PHONE_STATE，则设置为false
                .needRequestIMEI(true)
                // 如果自己接入了神策安卓sdk，则设置为false
                .enableInnerTrack(true)
                .build();
        return params;
    }
}
