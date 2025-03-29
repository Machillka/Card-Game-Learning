using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public Room roomPrefab;
    public LineRenderer lineRenderer;
    [Header("Config")]
    public MapConfigSO mapConfig;
    public MapLayoutSO mapLayout;

    public float border;

    private float screenWidth;
    private float screenHeight;
    private float columnWidth;
    private Vector3 generatePosition;

    private List<Room> rooms = new List<Room>();
    private List<LineRenderer> lines = new List<LineRenderer>();

    public List<RoomDataSO> roomDataList = new List<RoomDataSO>();
    // 通过房间类型返回数据
    private Dictionary<RoomType, RoomDataSO> roomDataDict = new Dictionary<RoomType, RoomDataSO>();

    private void Awake()
    {
        // 得到世界坐标的长和宽 得到正值
        screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        screenHeight = Camera.main.orthographicSize * 2;

        // +1 为了留出一个空位
        columnWidth = screenWidth / (mapConfig.roomBlueprints.Count + 1);
        Debug.Log("screenWidth: " + screenWidth + " screenHeight: " + screenHeight + " columnWidth: " + columnWidth);

        foreach (var roomData in roomDataList)
        {
            roomDataDict.Add(roomData.roomType, roomData);
        }
    }

    /// <summary>
    /// 每次激活的时候都判断是否已经有存储过的房间，如果有则加载，没有则生成
    /// 如果要进行游戏进度保存 可以直接序列化 mapLayoutSO 进行保存或加载
    /// </summary>
    private void OnEnable()
    {
        if (mapLayout.mapRoomDataList.Count > 0)
        {
            LoadMap();
        }
        else
        {
            CreateMap();
        }
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
                // 生成房间
                var room = Instantiate(roomPrefab, generatePosition, Quaternion.identity, transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprints[column].roomType);
                // 因为是初始化第一个 map, 所以都是Locked状态
                room.SetupRoom(column, i - 1, GetRoomData(newType));
                rooms.Add(room);

                currentColumnRooms.Add(room);
            }
            // 如果不是第一列 则连接上一列
            if (previousColumeRooms.Count > 0)
            {
                CreateConnection(previousColumeRooms, currentColumnRooms);
            }
            previousColumeRooms = currentColumnRooms;
        }
        SaveMap();
    }

    [ContextMenu("ReGenerateRooms")]
    public void ReGenerateRooms()
    {
        foreach(var room in rooms)
        {
            Destroy(room.gameObject);
        }

        foreach(var line in lines)
        {
            Destroy(line.gameObject);
        }

        rooms.Clear();
        lines.Clear();

        CreateMap();
    }

    /// <summary>
    /// 连接上一列的房间和当前列的房间
    /// </summary>
    /// <param name="previousColumeRooms">上一列房间</param>
    /// <param name="currentColumnRooms">下一列房间</param>
    private void CreateConnection(List<Room> previousColumeRooms, List<Room> currentColumnRooms)
    {
        HashSet<Room> connectedColumeRooms = new HashSet<Room>();

        foreach (var room in previousColumeRooms)
        {
            var targetRoom = ConnectToRamdomRoom(room, currentColumnRooms);
            connectedColumeRooms.Add(targetRoom);
        }

        // 若当前房间没有与上一列房间的连接，于是随机从上一列房间中选取一个进行连接
        foreach (var room in currentColumnRooms)
        {
            if (!connectedColumeRooms.Contains(room))
            {
                ConnectToRamdomRoom(room, previousColumeRooms);
            }
        }
    }

    /// <summary>
    /// 实现一个房间和下一个房间（从列表中选取）的连线
    /// </summary>
    /// <param name="room">当前要连线的房间</param>
    /// <param name="currentColumnRooms">从中选取一个要连线的房间</param>
    /// <returns></returns>
    private Room ConnectToRamdomRoom(Room room, List<Room> currentColumnRooms)
    {
        Room targetRoom;

        targetRoom = currentColumnRooms[Random.Range(0, currentColumnRooms.Count)];

        // 创建连线
        var line = Instantiate(lineRenderer, Vector3.zero, Quaternion.identity, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        lines.Add(line);

        return targetRoom;
    }

    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }

    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(',');

        string roomTypeOption = options[Random.Range(0, options.Length)];

        return (RoomType)System.Enum.Parse(typeof(RoomType), roomTypeOption);
    }

    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new List<MapRoomData>();
        mapLayout.linePositionList = new List<LinePosition>();

        for (int i = 0; i < rooms.Count; i++)
        {
            var room = new MapRoomData()
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                column = rooms[i].column,
                line = rooms[i].line,
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState
            };

            mapLayout.mapRoomDataList.Add(room);
        }

        for (int i = 0; i < lines.Count; i++)
        {
            var line = new LinePosition()
            {
                startPos = new SerializeVector3(lines[i].GetPosition(0)),
                endPos = new SerializeVector3(lines[i].GetPosition(1))
            };

            mapLayout.linePositionList.Add(line);
        }
    }

    private void LoadMap()
    {
        // 读取房间
        foreach (var room in mapLayout.mapRoomDataList)
        {
            var newPos = new Vector3(room.posX, room.posY, 0);
            var newRoom = Instantiate(roomPrefab, newPos, Quaternion.identity, transform);
            newRoom.SetupRoom(room.column, room.line, room.roomData, room.roomState);
            rooms.Add(newRoom);
        }

        foreach (var line in mapLayout.linePositionList)
        {
            var newLine = Instantiate(lineRenderer, Vector3.zero, Quaternion.identity, transform);
            newLine.SetPosition(0, line.startPos.ToVector3());
            newLine.SetPosition(1, line.endPos.ToVector3());
            lines.Add(newLine);
        }
    }
}