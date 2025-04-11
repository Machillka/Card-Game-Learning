using UnityEngine;

// 本次使用单例管理 不使用 SO 进行事件广播
public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager _instance;
    public static CurrencyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CurrencyManager();
            }
            return _instance;
        }
    }

    private IntVariable _currency;
    public int Currency
    {
        get => _currency.currentValue;
        set => _currency.SetValue(value);
    }

    void InitCurrency()
    {
        Currency = 0;
    }

    void AddCurrency(int amount)
    {
        Currency += amount;
    }

    void ReduceCurrency(int amount)
    {
        Currency -= amount;
    }
}


//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
// }
