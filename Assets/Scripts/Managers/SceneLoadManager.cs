using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    public AssetReference map;

    /// <summary>
    /// 监听房间加载事件
    /// </summary>
    /// <param name="data"></param>
    public async void OnLoadRoomEvent(object data)
    {
        Debug.Log("OnLoadRoomEvent");
        if (data is RoomDataSO currentData)
        {
            Debug.Log("Loading room: " + currentData.roomType);
            currentScene = currentData.sceneToLoad;
        }

        await UnloadSceneTask();
        // 加载房间
        await LoadSceneTask();
    }

    private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;
        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    private async Awaitable UnloadSceneTask()
    {
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    public async void LoadMap()
    {
        await UnloadSceneTask();

        currentScene = map;

        await LoadSceneTask();
    }
}
