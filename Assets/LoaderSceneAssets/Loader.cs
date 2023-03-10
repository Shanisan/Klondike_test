using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{

    public Slider loadingBar;
    private Vector2 loadingBarSize;
    public Text loadingText;
    private readonly string assetBundleName = "remoteloading.remote";

    private AssetBundle assetBundle;
    string[] scenePaths;

    IEnumerator LoadRemoteLevel()
    {
        #region setup
        //The file is hosted on my personal Nextcloud so for testing you'll have to modify this URL
        string url = "http://192.168.50.214/index.php/s/BA4QFx9ZDKTa2cQ/download/remoteloading.remote";
        var request
            = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
        #endregion


        #region fetching remote level
        AsyncOperation op = request.SendWebRequest();
        while (!op.isDone)
        {
            loadingBar.value = Mathf.Clamp01(op.progress / 0.9f);
            yield return null;
        }
        assetBundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
        scenePaths = assetBundle.GetAllScenePaths();
        #endregion


        loadingBar.fillRect.GetComponent<Image>().color = Color.green;
        loadingText.text = "Loading Scene...";


        #region loading downloaded level
        op = SceneManager.LoadSceneAsync(scenePaths[0]);
        while (!op.isDone)
        {
            loadingBar.value = Mathf.Clamp01(op.progress / 0.9f);
            yield return null;
        }
        #endregion
    }

    private void CheckInternetThenLoad()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            loadingText.text = "No internet connection. " +
                "Please enable Wifi or Mobile Data and tap anywhere to retry.";
            loadingText.color = Color.red;
        }
        else
        {
            loadingBar.fillRect.GetComponent<Image>().color = Color.blue;
            loadingText.text = "Downloading Assets...";
            loadingText.color = Color.white;
            StartCoroutine(LoadRemoteLevel());
        }
    }

    public void Start()
    {
        if (assetBundle != null)
        {
            assetBundle.Unload(false);
        }

        CheckInternetThenLoad();
        
    }

    public void Update()
    {
        if(Input.touchCount > 0)
        {
            CheckInternetThenLoad();
        }
    }
}
