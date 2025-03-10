using UnityEngine;
using UnityEditor;
using System.IO;

namespace EfimEgorov
{
    public class CreateAssetsBundle
    {
        [MenuItem("Assets/Build AssetBundles")]

        static void BuildAllAssetBundles()
        {
            string assetsBundleDirectory = Path.Combine(Application.streamingAssetsPath, "AssetBundles");

            if(!Directory.Exists(assetsBundleDirectory))
            {
                Directory.CreateDirectory(assetsBundleDirectory);
            }

            BuildPipeline.BuildAssetBundles(assetsBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
}