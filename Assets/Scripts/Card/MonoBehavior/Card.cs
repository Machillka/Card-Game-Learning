using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Compoment")]                       // 获得自身组件 以便通过DataSO修改内容
    public SpriteRenderer cardSptire;
    public TextMeshPro costText;
    public TextMeshPro descrptionText;
    public TextMeshPro typeText;

    public CardDataSO cardData;
    public bool isAnimating = false;

    [Header("原始数据")]
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;

    public Player player;

    [Header("广播事件")]
    public ObjectEventSO discardCardEvent;

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
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void UpdatePositionRotation(Vector3 position, Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //TODO: 设置固定的高度
        if (isAnimating)
            return;
        transform.SetPositionAndRotation(
            originalPosition + Vector3.up,
            Quaternion.identity
        );

        GetComponent<SortingGroup>().sortingOrder = 25;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAnimating)
            return;
        ResetCardTransform();
    }

    public void ResetCardTransform()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    public void ExcuteCardEffects(CharacterBase from, CharacterBase target)
    {
        //TODO: 回收卡牌和cost减少
        discardCardEvent.RaiseEvent(this, this);
        foreach (var effect in cardData.effects)
        {
            effect.Excute(from, target);
        }
    }
}
