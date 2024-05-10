using System.Collections;

public class Score2Multiplier : Consumable
{
    public override string GetConsumableName()
    {
        return "x2";
    }

    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.SCORE_MULTIPLAYER;
    }

    public override int GetPrice()
    {
        return 750;
    }

    public override int GetPremiumCost()
    {
        return 0;
    }

    public override IEnumerator Started(CharacterInputController c)
    {
        yield return base.Started(c);

        m_SinceStart = 0;

        TrackManager.m_Multiplier = 2;
    }

    public override void Ended(CharacterInputController c)
    {
        base.Ended(c);

        TrackManager.m_Multiplier = 1;
    }

    //protected int MultiplyModify(int multi)
    //{
    //    return multi * 2;
    //}
}
