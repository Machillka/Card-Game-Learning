using System;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    // public bool isHorizontal = false;
    public float maxHorizontalWidth = 7f;
    public float maxSectorWidth = 10f;
    public float cardSpacing = 2f;
    public float horizontalCenterY = -4.5f;
    public float sectorCenterY = -21.5f;

    [Header("弧度排列参数")]
    public float angleBetweenCards = 7f;
    public float radius = 17f;

    private Vector3 centerPosition;
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
            CalulateSectorCardPosition(numberOfCards);
        }
    }

    public CardTransform GetCardTransform(int index, int numberOfCards, bool isHorizontal = false)
    {
        CalulateCardPosition(numberOfCards, isHorizontal);

        if (index < 0 || index >= cardPositionList.Count)
        {
            return null;
        }

        return new CardTransform(cardPositionList[index], cardRotationList[index]);
    }

    private void CalulateSectorCardPosition(int numberOfCards)
    {
        //TODO: 限制扇形弧度范围（同横向排列一样）
        // 向下移动中心点
        centerPosition = Vector3.up * sectorCenterY;

        // 计算总的角度变化
        // float cardAngle = (numberOfCards - 1) * angleBetweenCards / 2;
        // float totalAlgle = Mathf.Min(maxSectorWidth / 2, cardAngle);

        // for (int i = 0; i < numberOfCards; i++)
        // {
        //     var deltaAngle = cardAngle -  i * angleBetweenCards;
        //     var pos = SectorCardPositon(deltaAngle);
        //     var rotation = Quaternion.Euler(0, 0, deltaAngle);

        //     cardPositionList.Add(pos);
        //     cardRotationList.Add(rotation);
        // }
        // 总展开角度计算
        float cardAngle = (numberOfCards - 1) * angleBetweenCards;
        // 限制角度
        float totalAlgle = Mathf.Min(maxSectorWidth, cardAngle);
        float deltaSpaceAngle = totalAlgle > 0 ? totalAlgle / (numberOfCards - 1) : 0;

        for (int i = 0; i < numberOfCards; i++)
        {
            var deltaAngle = totalAlgle / 2 -  i * deltaSpaceAngle;
            var pos = SectorCardPositon(deltaAngle);
            var rotation = Quaternion.Euler(0, 0, deltaAngle);

            cardPositionList.Add(pos);
            cardRotationList.Add(rotation);
        }
    }

    private void CalulateHorizontalCardPosition(int numberOfCards)
    {
        centerPosition = Vector3.up * horizontalCenterY;

        float currentWidth = cardSpacing * (numberOfCards - 1);
        float totalWidth = Mathf.Min(maxHorizontalWidth, currentWidth);

        // 计算每张牌之间的间隙
        float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;

        for (int i = 0; i < numberOfCards; i++)
        {
            float x = -totalWidth / 2 + i * currentSpacing;
            var pos = new Vector3(x, centerPosition.y, 0);
            var rotation = Quaternion.identity;

            cardPositionList.Add(pos);
            cardRotationList.Add(rotation);
        }
    }

    private Vector3 SectorCardPositon(float angle)
    {
        return new Vector3(
            centerPosition.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPosition.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0
        );
    }
}
