using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public Room roomPrefab;
    public LineRenderer lineRenderer;

    public MapConfigSO mapConfig;

    public float border;

    private float screenWidth;
    private float screenHeight;
    private float columnWidth;
    private Vector3 generatePosition;

    private List<Room> rooms = new List<Room>();

    private void Awake()
    {
        // 得到世界坐标的长和宽 得到正值
        screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        screenHeight = Camera.main.orthographicSize * 2;

        // +1 为了留出一个空位
        columnWidth = screenWidth / (mapConfig.roomBlueprints.Count + 1);
        Debug.Log("screenWidth: " + screenWidth + " screenHeight: " + screenHeight + " columnWidth: " + columnWidth);
    }

    private void Start()
    {
        CreateMap();
    }

    public void CreateMap()
    {
        List<Room> previousColumeRooms = new List<Room>();

        // Create the map
        for (int column = 0; column < mapConfig.roomBlueprints.Count; column++)
        {
            var bluepring = mapConfig.roomBlueprints[column];
            var amount = Random.Range(bluepring.min, bluepring.max);
            var roomType = bluepring.roomType;

            // 设置生成初始位置, 每一列的第一个, x: 从屏幕左侧开始位移; y: 屏幕上顶点
            generatePosition = new Vector3(-screenWidth / 2 + border + columnWidth * column, screenHeight / 2);
            var roomGapY = screenHeight / (amount + 1);

            // // 固定最后一列的 x 位置
            // if (column == mapConfig.roomBlueprints.Count - 1)
            // {
            //     generatePosition.x = screenWidth / 2 - border;
            // }

            List<Room> currentColumnRooms = new List<Room>();

            for (int i = 1; i <= amount; i++)
            {
                // 每次生成, 向下位移一个offset
                generatePosition.y = screenHeight / 2 - roomGapY * i;
                var room = Instantiate(roomPrefab, generatePosition, Quaternion.identity, transform);

                rooms.Add(room);
                currentColumnRooms.Add(room);
            }
            // 如果不是第一列 则连接上一列
            if (previousColumeRooms.Count > 0)
            {
                for (int i = 0; i < previousColumeRooms.Count; i++)
                {
                    var previousRoom = previousColumeRooms[i];
                    var currentRoom = currentColumnRooms[i];

                    var line = Instantiate(lineRenderer, Vector3.zero, Quaternion.identity, transform);
                    line.SetPosition(0, previousRoom.transform.position);
                    line.SetPosition(1, currentRoom.transform.position);
                }
            }
            previousColumeRooms = currentColumnRooms;
        }
    }

    [ContextMenu("ReGenerateRooms")]
    public void ReGenerateRooms()
    {
        foreach(var room in rooms)
        {
            Destroy(room.gameObject);
        }

        rooms.Clear();

        CreateMap();
    }
}
