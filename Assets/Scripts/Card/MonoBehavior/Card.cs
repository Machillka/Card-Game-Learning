using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Compoment")]                       // 获得自身组件 以便通过DataSO修改内容
    public SpriteRenderer cardSptire;
    public TextMeshPro costText;
    public TextMeshPro descrptionText;
    public TextMeshPro typeText;

    public CardDataSO cardData;

    private void Start()
    {
        Init(cardData);
    }

    public void Init(CardDataSO cardData)
    {
        this.cardData = cardData;
        cardSptire.sprite = cardData.cardImage;
        costText.text = cardData.cardCost.ToString();
        descrptionText.text = cardData.cardDescription;
        typeText.text = cardData.cardType.ToString();
        /*
        typeText.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Skill => "能力",
            _ => throw new System.NotImplementedException(),
        };
        */
    }
}
