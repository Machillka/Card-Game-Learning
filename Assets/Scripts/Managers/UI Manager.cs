using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("面板")]
    public GameObject gameplayPanel;
    public GameObject gamewinPanel;
    public GameObject gameOverPanel;
    public GameObject pickCardPanel;
    public GameObject restRoomPanel;

    public void OnLoadRoomEvent(object obj)
    {
        Room currentRoom = obj as Room;

        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                gameplayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.RestRoom:
                restRoomPanel.SetActive(true);
                break;
        }
    }

    public void HiddAllPanels()
    {
        gameplayPanel.SetActive(false);
        gamewinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restRoomPanel.SetActive(false);
    }

    public void OnGameWinEvent()
    {
        HiddAllPanels();
        gamewinPanel.SetActive(true);
    }

    public void OnGameOverEvent()
    {
        HiddAllPanels();
        gameOverPanel.SetActive(true);
    }

    public void OnPickCardEvent()
    {
        // HiddAllPanels();
        pickCardPanel.SetActive(true);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardPanel.SetActive(false);
    }
}
