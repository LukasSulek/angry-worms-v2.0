using System;

public class VegetableBehaviour : UnitBehaviour
{
    private event Action onVegetableDestroy;
    public override void Init()
    {
        onVegetableDestroy += HandleVegetableDestroy;
    }

    private void OnDestroy()
    {
        onVegetableDestroy -= HandleVegetableDestroy;
    }

    protected override void OnInteraction()
    {
        onVegetableDestroy?.Invoke();
    }

    public void HandleVegetableDestroy()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ChangeGameState(GameState.GameOver);
    }
}
