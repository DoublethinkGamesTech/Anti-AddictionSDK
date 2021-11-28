package com.doublethink.utils;

import com.xmiles.sceneadsdk.base.sdk.INetMode;

public class DataParams {
    //Whether to debug mode or not, detailed log will be printed
    public static boolean isDebug = false;
    //Interface environment mode，Formal environment = INetMode.OFFICIAL，Debug environment = INetMode.TEST
    public static int iNetMode = INetMode.OFFICIAL;
    // protect id
    public static String productId = "999999";
    // game name
    public static String gameName = "应用名";

}
