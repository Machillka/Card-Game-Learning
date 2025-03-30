using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList;

    private void Awake()
    {
        InitializeCardDataList();
    }

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
}
