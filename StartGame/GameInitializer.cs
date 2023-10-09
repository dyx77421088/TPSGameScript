using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private string updateAddress;

    private void Start()
    {
        Addressables.InitializeAsync().Completed += OnInitializationComplete;
    }

    private void OnInitializationComplete(AsyncOperationHandle<IResourceLocator> obj)
    {
        UpdateAddressableAssets();
    }

    private void UpdateAddressableAssets()
    {
        Addressables.DownloadDependenciesAsync(updateAddress).Completed += OnDownloadComplete;
    }

    private void OnDownloadComplete(AsyncOperationHandle handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.LoadScene("GameScene"); // 加载游戏场景
        }
        else
        {
            // 处理下载失败的情况
        }
    }
}