using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{

    public Slider loadingBar;
    public Text loadingText;
    private readonly string assetBundleName = "remoteloading.remote";

    private AssetBundle assetBundle;
    string[] scenePaths;

    IEnumerator LoadRemoteAssetBundle()
    {
        #region setup
        string url = "file:///" + Application.dataPath + "/AssetBundles/" + assetBundleName;
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
        loadingBar.fillRect.GetComponent<Image>().color = Color.red;
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

    public void Start()
    {
        if (assetBundle != null)
        {
            assetBundle.Unload(false);
        }
        loadingBar.fillRect.GetComponent<Image>().color = Color.blue;
        loadingText.text = "Downloading Assets...";
        StartCoroutine(LoadRemoteAssetBundle());

    }
}
