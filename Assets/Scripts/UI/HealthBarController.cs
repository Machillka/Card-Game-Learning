using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    public CharacterBase currentCharacter;

    [Header("Elements")]
    public Transform healthbarTransform;
    private UIDocument healthBarDocument;
    private ProgressBar healthBar;

    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
        InitializeHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    private void MoveToWorldPosition(VisualElement element, Vector3 worldPosition, Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, size, Camera.main);
        element.transform.position = rect.position;
    }

    public void UpdateHealthBar()
    {
        if (healthBar == null)
            return;
        if (currentCharacter.isDead)
        {
            healthBar.style.display = DisplayStyle.None;
            return;
        }
        healthBar.title = $"{currentCharacter.CurrentHP}/{currentCharacter.MaxHP}";
        healthBar.value = currentCharacter.CurrentHP;
    }

    [ContextMenu("Set UI Position")]
    private void InitializeHealthBar()
    {
        healthBarDocument = GetComponent<UIDocument>();
        healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");
        healthBar.highValue = currentCharacter.MaxHP;
        MoveToWorldPosition(healthBar, healthbarTransform.position, Vector2.zero);
    }

}
