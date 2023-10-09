using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TPSShoot;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameApp : MonoBehaviour
{
    public static Dictionary<string, GameObject> goDict = new Dictionary<string, GameObject>();
    public GameObject dialogPrefab;

    private string webVersion = RequestConfig.versionPath;
    private string localVersion = FileUtils.versionPath;

    private RequestUtils reqUtils;
    private string versionText;
    //private string version = FileUtils.versionPath;
    public void CheckUpdate()
    {
        reqUtils = RequestUtils.Instance;
        reqUtils.GetText(webVersion, GetVersion);
    }

    private void GetVersion(string versionText)
    {
        this.versionText = versionText;
        Debug.Log(versionText);
        if (string.IsNullOrEmpty(versionText))
        {
            Dialog("没连接到网络", ()=>Quit(), () => SceneManager.LoadScene(MyScenes.csScene));
            return;
        }
        if (versionText != FileUtils.GetTextByPath(localVersion))
        {
            
            //
            reqUtils.GetText(RequestConfig.GetVersionMessagePath(versionText), GetUpdataMeaage);
            //FileUtils.WriteTextByPath("version.txt", versionText);
        }
        else
        {
            //EnterGameScene();
            reqUtils.GetText(RequestConfig.GetVersionMessagePath(versionText), GetUpdataMeaage);
        }
    }
    public void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    /// <summary>
    /// 请求成功后获得的json(有更新的描述信息）
    /// </summary>
    /// <param name="updataMessage">json</param>
    private void GetUpdataMeaage(string updataMessage)
    {
        // 把text显示出来并进行更新（使用addressables更新）
        Debug.Log("版本号不同，需要更新....");
        var t = Addressables.InstantiateAsync(MyAddressablesStr.dialog, UiUtils.Instance.Canvas.transform);
        t.Completed += gameObject =>
        {
            Debug.Log(gameObject.Status);
            DialogUI dialogUI = gameObject.Result.GetComponent<DialogUI>();

            dialogUI.SetText("检测到版本更新，正在计算大小......");

            // 异步请求网络加载本次需要更新的大小
            AddressablesUtils.Instance.GetUpdateSize((size, updateHandle) =>
            {
                if (size == 0)
                {
                    FileUtils.WriteTextByPath("version.txt", versionText);
                    dialogUI.SetText("无需更新");
                    EnterGameScene();
                    return;
                }
                dialogUI.SetText("检测到版本更新，本次更新大小约为" + DataUtils.GetDataSize(size));
                // 当用户点击更新按钮后的操作
                dialogUI.onOkClick += () =>
                {
                    // 加载更新信息和进度条
                    Addressables.InstantiateAsync("UpdateInfoAndProgress", UiUtils.Instance.Canvas.transform).Completed += go =>
                    {
                        UpdateInfoAndProgressUI updateInfoAndProgressUI = go.Result.GetComponent<UpdateInfoAndProgressUI>();
                        updateInfoAndProgressUI.SetUpdateInfo(updataMessage);

                        // 开启下载任务。。。。
                        AddressablesUtils.Instance.StartDownLoadUpdate(updateHandle, (MyAddressablesInfoModel MAI) =>
                        {
                            //updateInfoAndProgressUI.SetUpdateSpeedText(10, )
                            updateInfoAndProgressUI.SetSlider(MAI.DownloadProgress);
                            updateInfoAndProgressUI.SetUpdateSpeedText(MAI.DownloadSpeed, MAI.DownloadedSize, size);

                            if (MAI.DownloadProgress == 1)
                            {
                                FileUtils.WriteTextByPath("version.txt", versionText); // 下载完成了;
                                EnterGameScene();
                            }
                        });
                    };
                };
                dialogUI.onNoClick += () => Application.Quit();
            });


        };
    }

    /// <summary>
    /// 从网上获得ab包之后的处理工作
    /// </summary>
    /// <param name="ab"></param>
    private void SetAssetBundle(string name, MyRequestInfoModel info)
    {
        //GameObject go = DownloadHandlerAssetBundle.GetContent(request).LoadAsset<GameObject>(name);
        //goDict.Add(name, go);

        //Instantiate(go);

        // 把这个ab包保存到本地
        ProgressUI.Instance.SetProgress(info.DownloadProgress);
        ProgressUI.Instance.SetProgressText(DataUtils.GetDataSize(info.DownloadSpeed), DataUtils.GetDataSize(info.Size));

    }

    private void Dialog(string text, Action onOkClick, Action onNoClick)
    {
        var t = Addressables.InstantiateAsync(MyAddressablesStr.dialog, UiUtils.Instance.Canvas.transform);
        t.Completed += gameObject =>
        {
            DialogUI dialogUI = gameObject.Result.GetComponent<DialogUI>();
            dialogUI.SetText(text);
            dialogUI.onOkClick = onOkClick;
            dialogUI.onNoClick = onNoClick;
        };
    }
    public GameObject GetGameObject(string name)
    {
        return goDict[name];
    }

    private void EnterGameScene()
    {
        Debug.Log("进入游戏");
        SceneManager.LoadScene(MyScenes.gameScene);
        
        
    }

}
