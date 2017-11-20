using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCautious : PlayerBase
{
    public override void Play()
    {
        if (!isPlaying)
            return;

        Propertie currentPropertie = GameManager.INSTANCE.GetPropertie(currentPosition);
        if (currentPropertie.owner == null)
        {
            if (currentPropertie.sellValue <= currentGold && currentGold - currentPropertie.sellValue > 80)
            {
                currentGold -= currentPropertie.sellValue;
                currentPropertie.owner = this;
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
