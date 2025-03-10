using System.IO;
using UnityEngine;

namespace EfimEgorov
{
    public class LoadAssetBundles : MonoBehaviour
    {
        [SerializeField] private string folderPath = "AssetBundles";
        [SerializeField] private string bundleName;
        [SerializeField] private string fileName = "Spikes";
        private string combinedPath;
        private AssetBundle bundle;

        void Start()
        {
            combinedPath = Path.Combine(Application.streamingAssetsPath, folderPath, bundleName);   
            LoadAssetBundle();
            LoadObstacle();
        }

        public void LoadObstacle()
        {
            if(bundle == null)
            {
                return;
            }

            GameObject obstacle = bundle.LoadAsset<GameObject>(fileName);
            if(obstacle != null)
            {
                Instantiate(obstacle);
            }
            else
            {
                Debug.Log(fileName + " has not found");
            }
        }

        public void LoadAssetBundle()
        {
            if(File.Exists(combinedPath))
            {
                bundle = AssetBundle.LoadFromFile(combinedPath);
                Debug.Log("Asset bundle loaded");
            }
            else
            {
                Debug.Log("File has not found: " + combinedPath);
            }
        }
    }
}