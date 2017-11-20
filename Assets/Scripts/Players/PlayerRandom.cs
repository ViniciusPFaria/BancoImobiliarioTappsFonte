using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRandom : PlayerBase
{
    public override void Play()
    {
        if (!isPlaying)
            return;

        Propertie currentPropertie = GameManager.INSTANCE.GetPropertie(currentPosition);
        if (currentPropertie.owner == null)
        {
            if (currentPropertie.sellValue <= currentGold)
            {
                int randDecision = UnityEngine.Random.Range(0, 2);
                if (randDecision == 1)
                {
                    currentGold -= currentPropertie.sellValue;
                    currentPropertie.owner = this;
                }
            }
        }
        else if (currentPropertie.owner != this)
        {
            if (currentPropertie.rentValue < currentGold)
            {
                currentPropertie.owner.ReciveRent(currentPropertie.rentValue);
                currentGold -= currentPropertie.rentValue;
            }
            else
            {
                isPlaying = false;
                base.Bankrupt(this);
            }
        }
    }
}
