using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace EfimEgorov
{
    public class LoadSounds : MonoBehaviour
    {
        public AudioSource audioSource;
        public string audioFileName = "userAudio.wav";
        public string folderName;
        private AudioClip clip;

        [SerializeField] private string streamingAssetsPath;
        
        void Awake()
        {
            streamingAssetsPath = Application.streamingAssetsPath;
        }
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(LoadAudioFromStreamingAssets());
        }
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                PlaySound();
            }
        }

        IEnumerator LoadAudioFromStreamingAssets()
        {
            string audioPath = Path.Combine(streamingAssetsPath, folderName, audioFileName);
            if (!File.Exists(audioPath))
            {
                Debug.Log("Audio has not found: " + audioPath);
                yield break;
            }

            string url = "file://" + audioPath;
            using(UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(audioPath);
                    Debug.Log("Loading error: " + www.error);
                }
                else
                {
                    Debug.Log("Audio loaded");
                    clip = DownloadHandlerAudioClip.GetContent(www);
                }
            }
        }

        void PlaySound()
        {
            if(audioSource == null)
            {
                Debug.Log("Audio source has not attached");
            }
            if(clip == null)
            {
                Debug.Log("sound is not attached");
            }
            audioSource.PlayOneShot(clip);
        }
    }
}