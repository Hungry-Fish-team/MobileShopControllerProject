using UnityEditor;

public class CreateAssetBundels 
{
    [MenuItem("Assets/Build AssetBundles for Android")]
    public static void BuildAllAssetBundlesForAndroid()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    [MenuItem("Assets/Build AssetBundles for IOS")]
    public static void BuildAllAssetBundlesForIOS()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.iOS);
    }
}
