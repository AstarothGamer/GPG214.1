using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace EfimEgorov
{
    public class LoadTextures : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public string spriteFileName = "userTexture.png";
        public string folderName = "Textures";

        [SerializeField] private string streamingAssetsPath;

        void Awake()
        {
            streamingAssetsPath = Application.streamingAssetsPath;
        }
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutine(LoadSpriteFromStreamingAssets());
        }

        IEnumerator LoadSpriteFromStreamingAssets()
        {
            string spritePath = Path.Combine(streamingAssetsPath, folderName, spriteFileName);
            if (!File.Exists(spritePath))
            {
                Debug.LogWarning("File has not founded: " + spritePath);
                yield break;
            }

            string url = "file://" + spritePath;
            using(UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Loading error: " + www.error);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    
                    Rect rect = new Rect(0, 0, texture.width, texture.height);
                    Vector2 pivot = new Vector2(0.5f, 0.5f);
                    Sprite sprite = Sprite.Create(texture, rect, pivot);
                    spriteRenderer.sprite = sprite;
                    Debug.Log("Sprite loaded");
                }
            }
        }
    }
}