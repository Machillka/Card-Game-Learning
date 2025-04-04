using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitLoad : MonoBehaviour
{
    public AssetReference presistentScene;

    private void Awake()
    {
        Addressables.LoadSceneAsync(presistentScene);
    }
}
