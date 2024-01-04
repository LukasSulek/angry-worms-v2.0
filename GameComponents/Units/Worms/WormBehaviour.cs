using System;
using UnityEngine;
using System.Collections;

public class WormBehaviour : UnitBehaviour
{
    private int remainingTaps = 1;
    private float startTime = 1f;
    private float remainingTime = 1f;
    private const float deltaTime = 0.01f;
    
    public int ScoreValue { get; private set; }
    public int Index {get; set;}

    private Coroutine timeTillWormHidesCoroutine = null;

    public event Action<int> OnVegetableEaten;
    public event Action<int> OnWormDamaged;
    public event Action<WormBehaviour> OnWormKilled;
    private event Action<WormBehaviour> OnWormHidden;
    
    private HealthCounter healthCounter = null;
    private TimeToKillWormTimer wormKillTimer = null;

    public override void Init()
    {
        OnWormKilled += ResetWorm;
        OnWormHidden += ResetWorm;


    }

    public override void ChangeDataT<T>(T data)
    {
        base.ChangeDataT(data);

        if (data is WormData wormData)
        {
            remainingTaps = wormData.TapsToKill;
            startTime = wormData.TimeToKill;
            remainingTime = wormData.TimeToKill;
            ScoreValue = wormData.ScoreValue;

            if (remainingTaps > 1) CreateHealthCounter(wormData);
            CreateWormKillTimer(wormData);
            timeTillWormHidesCoroutine = StartCoroutine(TimeTillWormHides());

        }
    }
    private void CreateHealthCounter(WormData wormData)
    {
        healthCounter = Instantiate(wormData.HealthCounter, transform.position, Quaternion.identity, transform);
        healthCounter.Init(remainingTaps);
        OnWormDamaged += healthCounter.SetHealthText;
    }

    private void CreateWormKillTimer(WormData wormData)
    {
        if (wormKillTimer == null) wormKillTimer = Instantiate(wormData.WormKillTimer, transform.position, Quaternion.identity, transform);
        wormKillTimer.Init(remainingTime, wormData.TimeToKill);
    }

    private IEnumerator TimeTillWormHides()
    {
        while (remainingTime > 0)
        {
            remainingTime -= deltaTime;
            wormKillTimer.SetValue(remainingTime, startTime);

            yield return new WaitForSeconds(deltaTime);
        }

        OnWormHidden?.Invoke(this);
    }

    protected override void OnInteraction()
    {
        base.OnInteraction();
        HandleWormTapped();
    }
    private void HandleWormTapped()
    {
        remainingTaps--;
        OnWormDamaged?.Invoke(remainingTaps);
        if(remainingTaps <= 0)
        {
            OnWormKilled?.Invoke(this);
            return;
        }

        //Play Death Sound                         
    }

    private void ResetWorm(WormBehaviour worm)
    {
        worm.gameObject.SetActive(false);

        DestroyHealthCounter();
        ResetTimerCoroutine();
    }

    public void EatVegetable(int index)
    {
        OnVegetableEaten?.Invoke(index);
    }

    private void DestroyHealthCounter()
    {
        if (healthCounter != null) Destroy(healthCounter.gameObject);
        healthCounter = null;
    }
    public void ResetTimerCoroutine()
    {
        if (timeTillWormHidesCoroutine != null) StopCoroutine(timeTillWormHidesCoroutine);
        timeTillWormHidesCoroutine = null;
    }
}
