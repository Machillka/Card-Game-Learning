using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList;

    [Header("Card Library")]
    public CardLibrarySO newGameCardLibrary;        // 新游戏的卡牌库
    public CardLibrarySO currnetCardLibrary;        // 当前玩家的卡牌库

    private void Awake()
    {
        InitializeCardDataList();

        // 每次新游戏都会初始化新的卡牌库
        currnetCardLibrary.cardLibraryList.Clear();
        foreach (var card in newGameCardLibrary.cardLibraryList)
        {
            currnetCardLibrary.cardLibraryList.Add(card);
        }
    }
    #region 获取卡牌库
    private void InitializeCardDataList()
    {
        // Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += handle =>
        // {
        //     cardDataList = new List<CardDataSO>(handle.Result);
        // };
        // 异步加载所有的CardDataSO
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoadComplete;
    }

    private void OnCardDataLoadComplete(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("CardData Load Failed");
        }
    }
    #endregion

    public GameObject GetCardObject()
    {
        return poolTool.GetObjectFromPool();
    }

    public void DiscardCardObject(GameObject cardObj)
    {
        poolTool.ReleaseObjectToPool(cardObj);
    }
}
