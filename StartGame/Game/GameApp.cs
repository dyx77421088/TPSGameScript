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
            Dialog("û���ӵ�����", ()=>Quit(), () => SceneManager.LoadScene(MyScenes.csScene));
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
    /// ����ɹ����õ�json(�и��µ�������Ϣ��
    /// </summary>
    /// <param name="updataMessage">json</param>
    private void GetUpdataMeaage(string updataMessage)
    {
        // ��text��ʾ���������и��£�ʹ��addressables���£�
        Debug.Log("�汾�Ų�ͬ����Ҫ����....");
        var t = Addressables.InstantiateAsync(MyAddressablesStr.dialog, UiUtils.Instance.Canvas.transform);
        t.Completed += gameObject =>
        {
            Debug.Log(gameObject.Status);
            DialogUI dialogUI = gameObject.Result.GetComponent<DialogUI>();

            dialogUI.SetText("��⵽�汾���£����ڼ����С......");

            // �첽����������ر�����Ҫ���µĴ�С
            AddressablesUtils.Instance.GetUpdateSize((size, updateHandle) =>
            {
                if (size == 0)
                {
                    FileUtils.WriteTextByPath("version.txt", versionText);
                    dialogUI.SetText("�������");
                    EnterGameScene();
                    return;
                }
                dialogUI.SetText("��⵽�汾���£����θ��´�СԼΪ" + DataUtils.GetDataSize(size));
                // ���û�������°�ť��Ĳ���
                dialogUI.onOkClick += () =>
                {
                    // ���ظ�����Ϣ�ͽ�����
                    Addressables.InstantiateAsync("UpdateInfoAndProgress", UiUtils.Instance.Canvas.transform).Completed += go =>
                    {
                        UpdateInfoAndProgressUI updateInfoAndProgressUI = go.Result.GetComponent<UpdateInfoAndProgressUI>();
                        updateInfoAndProgressUI.SetUpdateInfo(updataMessage);

                        // �����������񡣡�����
                        AddressablesUtils.Instance.StartDownLoadUpdate(updateHandle, (MyAddressablesInfoModel MAI) =>
                        {
                            //updateInfoAndProgressUI.SetUpdateSpeedText(10, )
                            updateInfoAndProgressUI.SetSlider(MAI.DownloadProgress);
                            updateInfoAndProgressUI.SetUpdateSpeedText(MAI.DownloadSpeed, MAI.DownloadedSize, size);

                            if (MAI.DownloadProgress == 1)
                            {
                                FileUtils.WriteTextByPath("version.txt", versionText); // ���������;
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
    /// �����ϻ��ab��֮��Ĵ�����
    /// </summary>
    /// <param name="ab"></param>
    private void SetAssetBundle(string name, MyRequestInfoModel info)
    {
        //GameObject go = DownloadHandlerAssetBundle.GetContent(request).LoadAsset<GameObject>(name);
        //goDict.Add(name, go);

        //Instantiate(go);

        // �����ab�����浽����
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
        Debug.Log("������Ϸ");
        SceneManager.LoadScene(MyScenes.gameScene);
        
        
    }

}
