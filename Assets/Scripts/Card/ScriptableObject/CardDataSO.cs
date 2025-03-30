using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardDataSO", order = 0)]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int cardCost;
    public CardType cardType;
    [TextArea]
    public string cardDescription;

    //TODO: 根据类型添加更多属性
}