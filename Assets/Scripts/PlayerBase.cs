using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase
{

    protected int currentPosition = 0;
    protected int currentGold = 300;
    protected bool isPlaying = true;
    private int winCont =0;
    public void Move()
    {
        int randDice = Random.Range(1, 7);
        currentPosition += randDice;
        if (currentPosition >= 20)
        {
            RunReward();
        }

        Play();
    }

    private void RunReward()
    {
        currentGold += 100;
        currentPosition -= 20;
    }

    protected void Bankrupt(PlayerBase loserPlayer)//lose the game :) 
    {
        GameManager.INSTANCE.ClearPlayerProperties(loserPlayer);
    }

    public abstract void Play();

    public void ReciveRent(int rentValue)
    {
        currentGold += rentValue;
    }

    public int GetCurrentGold()
    {
        return currentGold;
    }

    public bool StillInTheGame()
    {
        return isPlaying;
    }

    public void Reset()
    {
        currentPosition = 0;
        currentGold = 300;
        isPlaying = true;
    }

    public void Win()
    {
        winCont++;
        GameManager.INSTANCE.ClearPlayerProperties(this);
    }

    public int GetWin()
    {
        return winCont;
    }
}
