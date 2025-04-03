using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    public CharacterBase currentCharacter;

    [Header("UI Elements")]
    public Transform healthbarTransform;
    private UIDocument healthBarDocument;
    private ProgressBar healthBar;
    private VisualElement defenseRoot;
    private Label defenseAmountLabel;
    private VisualElement buffRoot;
    private Label buffRound;

    [Header("Buff Sprite")]
    public Sprite buffSprite;
    public Sprite debuffSprite;

    private Enemy enemy;
    private VisualElement intentRoot;
    private Label intentAmount;

    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
        enemy = GetComponent<Enemy>();
        InitializeHealthBar();
    }

    //TODO: 使用事件监听实现 Update
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
        healthBar.RemoveFromClassList("highHealth");
        healthBar.RemoveFromClassList("mediumHealth");
        healthBar.RemoveFromClassList("lowHealth");

        var percentage = (float)currentCharacter.CurrentHP / (float)currentCharacter.MaxHP;

        if (percentage < 0.3f)
        {
            healthBar.AddToClassList("lowHealth");
        }
        else if (percentage < 0.6f)
        {
            healthBar.AddToClassList("mediumHealth");
        }
        else
        {
            healthBar.AddToClassList("highHealth");
        }

        defenseRoot.style.display = currentCharacter.defense.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        defenseAmountLabel.text = currentCharacter.defense.currentValue.ToString();

        buffRoot.style.display = currentCharacter.buffRound.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        buffRoot.style.backgroundImage = currentCharacter.baseStrength > 1 ? new StyleBackground(buffSprite) : new StyleBackground(debuffSprite);
        buffRound.text = currentCharacter.buffRound.currentValue.ToString();
    }

    [ContextMenu("Set UI Position")]
    private void InitializeHealthBar()
    {
        healthBarDocument = GetComponent<UIDocument>();
        healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");

        defenseRoot = healthBar.Q<VisualElement>("Defense");
        defenseAmountLabel = defenseRoot.Q<Label>("DefenseAmount");

        buffRoot = healthBar.Q<VisualElement>("Buff");
        buffRound = buffRoot.Q<Label>("BuffRound");

        healthBar.highValue = currentCharacter.MaxHP;
        MoveToWorldPosition(healthBar, healthbarTransform.position, Vector2.zero);

        defenseRoot.style.display = DisplayStyle.None;
        buffRoot.style.display = DisplayStyle.None;

        intentRoot = healthBar.Q<VisualElement>("Intent");
        intentAmount = intentRoot.Q<Label>("IntentAmount");

        intentRoot.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// 玩家回合开始时调用
    /// </summary>
    public void SetIntentElement()
    {
        intentRoot.style.display = DisplayStyle.Flex;
        intentRoot.style.backgroundImage = new StyleBackground(enemy.currentAction.sprite);

        // 判断是否攻击
        var value = enemy.currentAction.effect.value;

        if (enemy.currentAction.effect.GetType() == typeof(DamageEffect))
        {
            value = (int)math.round(enemy.currentAction.effect.value * enemy.baseStrength);
        }

        intentAmount.text = value.ToString();
    }

    public void HideIntentElement()
    {
        intentRoot.style.display = DisplayStyle.None;
    }
}
