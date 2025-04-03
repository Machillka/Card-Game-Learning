using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    public EnemyActionDataSO actionData;
    public EnemyAction currentAction;
    protected Player player;

    // protected override void Awake()
    // {
    //     base.Awake();
    //     player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    // }

    public virtual void OnPlayerTurnBegin()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        var actionIndex = Random.Range(0, actionData.actions.Count);
        currentAction = actionData.actions[actionIndex];
    }

    public virtual void OnEnemyTurnBegin()
    {
        switch (currentAction.effect.targetType)
        {
            case EffectTargetType.Self:
                Skill();
                break;
            case EffectTargetType.Target:
                Attack();
                break;
            case EffectTargetType.All:
                break;
        }
    }

    public virtual void Skill()
    {
        // animator.SetTrigger("skill");
        // currentAction.effect.Excute(this, this);
        StartCoroutine(ProcessDelayAction("skill"));
    }

    public virtual void Attack()
    {
        // animator.SetTrigger("attack");
        // currentAction.effect.Excute(this, player);
        StartCoroutine(ProcessDelayAction("attack"));
    }

    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        yield return new WaitUntil(
            () => animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f > 0.6f
                    && !animator.IsInTransition(0)
                    && animator.GetCurrentAnimatorStateInfo(0).IsName(actionName)
        );

        if (actionName == "attack")
            currentAction.effect.Excute(this, player);
        else
            currentAction.effect.Excute(this, this);
    }
}
