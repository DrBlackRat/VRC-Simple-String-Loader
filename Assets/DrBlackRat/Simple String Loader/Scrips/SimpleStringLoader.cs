using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace DrBlackRat
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SimpleStringLoader : UdonSharpBehaviour
    {
        [Header("Download Link")]
        public VRCUrl url;

        [Header("Settings")]
        [Tooltip("Load String when you enter the World")]
        [SerializeField] private bool loadOnStart = true;

        [Space(10)]
        [Tooltip("Automaically reload String after a certain ammount of time (Load On Start should be enabled for this)")]
        [SerializeField] bool autoReload = false;
        [Tooltip("Time in minutes after which the String should be redownloaded")]
        [SerializeField] [Range(1, 60)] int autoReloadTime = 10;

        [Header("Text Components")]
        [Tooltip("Text component the string should be applied to, if left empty it tires to use the one it's attached to")]
        [SerializeField] private Text text;
        [Tooltip("Text Mesh Pro component the string should be applied to, if left empty it tires to use the one it's attached to")]
        [SerializeField] private TextMeshPro textMeshPro;
        [Tooltip("Text Mesh Pro UI component the string should be applied to, if left empty it tires to use the one it's attached to")]
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        [Header("Loadig & Error String")]
        [Tooltip("Use the Loading String while it waits for the String to Load")]
        [SerializeField] private bool useLoadingString = true;
        [Tooltip("Skips the Loading String when reloading the String (e.g. Auto Reload or Manually Loading it again)")]
        [SerializeField] private bool skipLoadingStringOnReload = false;
        [Tooltip("String used while the String is Loading")] [TextAreaAttribute]
        [SerializeField] private string loadingString;
        [Space(10)]
        [Tooltip("Use the Error String when the String couldn't be Loaded")]
        [SerializeField] private bool useErrorString = true;
        [Tooltip("String used when the String couldn't be Loaded")] [TextAreaAttribute]
        [SerializeField] private string errorString;

        // Internals
        [HideInInspector]
        public string loadedString;
        private bool loading = false;

        void Start()
        {
            // Get Components
            if (text == null) text = GetComponent<Text>();
            if (textMeshPro == null) textMeshPro = GetComponent<TextMeshPro>();
            if (textMeshProUGUI == null) textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            // Start Loading
            if (loadOnStart) _LoadString();

        }
        public void _LoadString()
        {
            // Check if it's already loading
            if (loading)
            {
                Debug.LogWarning($"[Simple String Loader] String from [{url}] is currently being downloaded, wait for it to be done before trying again!");
                return;
            }
            // Load String
            if (url == null || string.IsNullOrEmpty(url.Get()))
            {
                Debug.LogError("[Simple String Loader] URL is empty!");
                return;
            }
            loading = true;
            Debug.Log($"[Simple String Loader] Laoading String from [{url}]");
            VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);
        }
        public void _ApplyString(string useString)
        {
            if (text != null) text.text = loadedString;
            if (textMeshPro != null) textMeshPro.text = loadedString;
            if (textMeshProUGUI != null) textMeshProUGUI.text = loadedString;
        }
        public override void OnStringLoadSuccess(IVRCStringDownload result)
        {
            loading = false;
            Debug.Log($"[Simple String Loader] String from [{url}] Loaded Successfully!");
            // Apply String
            _ApplyString(result.Result);
            // Auto Reload
            if (autoReload)
            {
                SendCustomEventDelayedSeconds("_LoadString", autoReloadTime * 60);
                Debug.Log($"[Simple String Loader] Next Auto Reload for [{url}] in {autoReloadTime} minute(s)");
            }
        }

        public override void OnStringLoadError(IVRCStringDownload result)
        {
            loading = false;
            Debug.LogError($"[Simple String Loader] Could not Load String [{url}] because: {result.Error}");
            // Auto Reload
            if (autoReload)
            {
                SendCustomEventDelayedSeconds("_LoadString", autoReloadTime * 60);
                Debug.Log($"[Simple String Loader] Next Auto Reload for [{url}] in {autoReloadTime} minute(s)");
            }
        }
    }
}

