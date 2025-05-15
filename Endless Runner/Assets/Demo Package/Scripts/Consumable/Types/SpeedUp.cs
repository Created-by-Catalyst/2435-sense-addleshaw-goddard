using UnityEngine;
using System;
using System.Collections;
public class SpeedUp : Consumable
{
    public override string GetConsumableName()
    {
        return "SpeedUp";
    }

    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.SPEEDUP;
    }

    public override int GetPrice()
    {
        return 1500;
    }

	public override int GetPremiumCost()
	{
		return 5;
	}

	public override void Tick(CharacterInputController c)
    {
        base.Tick(c);

        c.SetSpeedUpExplicit(true);
    }

    public override IEnumerator Started(CharacterInputController c)
    {
        yield return base.Started(c);
        c.SetSpeedUp(duration);
    }

    public override void Ended(CharacterInputController c)
    {
        base.Ended(c);
        c.SetSpeedUpExplicit(false);
    }
}
