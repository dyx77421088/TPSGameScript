using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CheckUpdate : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(CheckUpdateForCatLog());
    }
    List<object> keys;
    IEnumerator CheckUpdateForCatLog()
    {
        var init = Addressables.InitializeAsync(false); // ��ʼ��
        yield return init;

        var checkForCatalogUpdate =  Addressables.CheckForCatalogUpdates(false); // ������
        yield return checkForCatalogUpdate;

        if (checkForCatalogUpdate.Status == AsyncOperationStatus.Succeeded)
        {
            List<string> results = checkForCatalogUpdate.Result;
            if (results != null && results.Count > 0)
            {
                Debug.Log("�����ɣ��ȴ�����");
                var updateHandle = Addressables.UpdateCatalogs(results, false); // ���±��ص�catlog
                yield return updateHandle;
                List<IResourceLocator> resources = updateHandle.Result;

                foreach (var item in resources)
                {
                    keys = new List<object>();
                    keys.AddRange(item.Keys);
                    
                    var size = Addressables.GetDownloadSizeAsync(keys);
                    Debug.Log("��С��" + size.Result);

                    if (size.Result > 0)
                    {
                        var download = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union, false);
                        Debug.Log("����");
                        while (!download.IsDone)
                        {
                            Debug.Log("������++++>" + download.PercentComplete);
                            yield return null;
                        }
                        yield return download;
                        if (download.Status == AsyncOperationStatus.Succeeded)
                        {
                            Debug.Log("��������ˣ���");
                        }
                        Addressables.Release(download);
                    }

                }

                Addressables.Release(updateHandle);
            }
            else
            {
                Debug.Log("���ܸ���");
            }
            
        }
        else
        {
            Debug.Log("���ʧ��");
        }

        Addressables.Release(checkForCatalogUpdate);
        Addressables.Release(init);
    }
}
