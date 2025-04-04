using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    public AssetReference map;
    public AssetReference menu;
    public AssetReference intro;

    private Vector2Int currentRoomVector;

    public FadePanel fadePanel;

    [Header("广播")]
    public ObjectEventSO afterRoomLoadedEvent;
    public ObjectEventSO updateRoomEvent;

    private Room currentRoom;

    private void Awake()
    {
        currentRoomVector = Vector2Int.one * -1;
        // LoadMenu();
        LoadIntro();
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
            fadePanel.FadeOut(0.2f);
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    private async Awaitable UnloadSceneTask()
    {
        fadePanel.FadeIn(0.4f);
        await Awaitable.WaitForSecondsAsync(0.45f);
        await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
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

    public async void LoadMenu()
    {

        // 表示第一次加载
        if(currentScene != null)
            await UnloadSceneTask();

        currentScene = menu;

        await LoadSceneTask();
    }

    public async void LoadIntro()
    {

        // 表示第一次加载
        if(currentScene != null)
            await UnloadSceneTask();

        currentScene = intro;

        await LoadSceneTask();
    }
}
