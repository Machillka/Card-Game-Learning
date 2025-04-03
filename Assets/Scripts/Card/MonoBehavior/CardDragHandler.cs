using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject arrowPrefab;
    private GameObject currentArrow;

    private Card currentCard;
    private bool canMove;
    private bool canExecute;

    private CharacterBase targetCharacter;

    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvaliable)
            return;

        switch (currentCard.cardData.cardType)
        {
            case CardType.Attack:
                canMove = false;
                currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                break;
            case CardType.Defense:
            case CardType.Abilities:
                canMove = true;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvaliable)
            return;

        if (canMove)
        {
            currentCard.isAnimating = true;
            Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            currentCard.transform.position = worldPos;

            canExecute = worldPos.y > 1f;
        }
        else
        {
            if(eventData.pointerEnter == null)
                return;
            if (eventData.pointerEnter.CompareTag("Enemy"))
            {
                canExecute = true;
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }
            canExecute = false;
            targetCharacter = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvaliable)
            return;

        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }
        if (canExecute)
        {
            currentCard.ExcuteCardEffects(currentCard.player, targetCharacter);
            canExecute = false;
        }
        else
        {
            currentCard.ResetCardTransform();
            currentCard.isAnimating = false;
        }
    }
}