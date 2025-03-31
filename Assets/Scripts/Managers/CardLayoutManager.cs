using System;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    // public bool isHorizontal = false;
    public float maxWidth = 7f;
    public float cardSpacing = 2f;
    public Vector3 centerPosition;

    private List<Vector3> cardPositionList = new List<Vector3>();
    private List<Quaternion> cardRotationList = new List<Quaternion>();

    private void CalulateCardPosition(int numberOfCards, bool isHorizontal = false)
    {
        cardPositionList.Clear();
        cardRotationList.Clear();

        if (isHorizontal)
        {
            CalulateHorizontalCardPosition(numberOfCards);
        }
        else
        {
            CalulateSectorCardPosition();
        }
    }

    public CardTransform GetCardTransform(int index, int numberOfCards, bool isHorizontal = false)
    {
        CalulateCardPosition(numberOfCards, isHorizontal);

        if (index < 0 || index >= cardPositionList.Count)
        {
            Debug.LogError("GetCardTransform index out of range");
            return null;
        }

        return new CardTransform(cardPositionList[index], cardRotationList[index]);
    }

    private void CalulateSectorCardPosition()
    {

    }

    private void CalulateHorizontalCardPosition(int numberOfCards)
    {
        float currentWidth = cardSpacing * (numberOfCards - 1);
        float totalWidtrh = MathF.Min(maxWidth, currentWidth);

        // 计算每张牌之间的间隙
        float currentSpacing = totalWidtrh > 0 ? totalWidtrh / (numberOfCards - 1) : 0;

        for (int i = 0; i < numberOfCards; i++)
        {
            float x = -totalWidtrh / 2 + i * currentSpacing;
            var pos = new Vector3(x, centerPosition.y, 0);
            var rotation = Quaternion.identity;

            cardPositionList.Add(pos);
            cardRotationList.Add(rotation);
        }
    }
}
