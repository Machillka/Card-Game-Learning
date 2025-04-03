using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    public AssetReference map;
    private Vector2Int currentRoomVector;

    [Header("广播")]
    public ObjectEventSO afterRoomLoadedEvent;
    public ObjectEventSO updateRoomEvent;

    private Room currentRoom;

    private void Start()
    {
        currentRoomVector = Vector2Int.one * -1;
    }

    /// <summary>
    /// 监听房间加载事件
    /// </summary>
    /// <param name="data"></param>
    public async void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            currentRoom = data as Room;
            var currentData = currentRoom.roomData;
            currentRoomVector = new Vector2Int(currentRoom.column, currentRoom.line);
            // Debug.Log("Loading room: " + currentData.roomType);
            currentScene = currentData.sceneToLoad;
        }

        await UnloadSceneTask();
        // 加载房间
        await LoadSceneTask();

        afterRoomLoadedEvent.RaiseEvent(currentRoom, this);
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

        if (currentRoomVector != Vector2Int.one * -1)
        {
            updateRoomEvent.RaiseEvent(currentRoomVector, this);
        }

        currentScene = map;

        await LoadSceneTask();
    }
}
