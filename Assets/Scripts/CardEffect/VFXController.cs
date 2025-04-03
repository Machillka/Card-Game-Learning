using UnityEngine;

public class VFXController : MonoBehaviour
{
    public float existingTime;
    public GameObject buff, debuff;

    //TODO: 使用携程解决
    private float timeCounter;

    private void Update()
    {
        if (buff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= existingTime)
            {
                timeCounter = 0f;
                buff.SetActive(false);
            }
        }

        if (debuff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= existingTime)
            {
                timeCounter = 0f;
                debuff.SetActive(false);
            }
        }
    }
}
