using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;
    public static GameManager INSTANCE
    {
        get
        {
            return _Instance;//lazzy singleton
        }
    }

    private PlayerBase[] playerArray = new PlayerBase[4];
    private Propertie[] propertiesArray = new Propertie[20];

    private int contLosers = 0;
    private int contTimeout = 0;
    private int totalRounds = 0;

    [SerializeField] List<GameObject> sliderList;
    [SerializeField] Text timeOutText;
    [SerializeField] Text mediaTurnosText;

    // Use this for initialization
    void Start()
    {
        _Instance = this;
    }

    public void StartSimulation()
    {
        LoaderInput.LoadArchive();

        totalRounds = 0;
        contTimeout = 0;

        for (int cont = 0; cont < propertiesArray.Length; cont++)
        {
            propertiesArray[cont] = new Propertie()
            {
                rentValue = LoaderInput.GetRentValue(cont),
                sellValue = LoaderInput.GetSellValue(cont)
            };
        }

        playerArray[0] = new PlayerCautious();
        playerArray[1] = new PlayerDemanding();
        playerArray[2] = new PlayerImpulsive();
        playerArray[3] = new PlayerRandom();

        for (int matchCont = 0; matchCont < 300; matchCont++)
        {
            ResetVariables();
            DefineStartOrder();
            RunSimulation();

        }

        for (int cont = 0; cont < playerArray.Length; cont++)
        {
            sliderList[cont].GetComponentInChildren<Slider>().value = playerArray[cont].GetWin();
            sliderList[cont].GetComponentInChildren<Text>().text = playerArray[cont].GetType().ToString()
                + " win: " + playerArray[cont].GetWin()
            + " porcentagem %: " + (playerArray[cont].GetWin() * 100 / 300);
        }

        timeOutText.text = "Timeout: " + contTimeout;
        mediaTurnosText.text = "Media de turnos: " + totalRounds / 300;
    }

    private void ResetVariables()
    {
        contLosers = 0;
        foreach (PlayerBase player in playerArray)
        {
            player.Reset();
        }
    }
    private void DefineStartOrder()
    {
        PlayerBase[] orderedPlayerArray = new PlayerBase[4];
        List<PlayerBase> playerList = playerArray.OfType<PlayerBase>().ToList();


        for (int cont = 0; cont < orderedPlayerArray.Length; cont++)
        {
            int rand = Random.Range(0, playerList.Count);
            orderedPlayerArray[cont] = playerList[rand];
            playerList.Remove(playerList[rand]);
        }


        playerArray = orderedPlayerArray;
    }

    private void RunSimulation()
    {

        bool hasWiner = false;
        int contRound = 0;
        while (!hasWiner)
        {
            for (int contPlayer = 0; contPlayer < playerArray.Length; contPlayer++)
            {
                playerArray[contPlayer].Move();
                if (contLosers >= 3 && playerArray[contPlayer].StillInTheGame())
                {
                    totalRounds += contRound;
                    playerArray[contPlayer].Win();
                    hasWiner = true;
                    break;
                }
            }
            contRound++;
            if (contRound >= 1000)
            {
                contTimeout++;
                totalRounds += contRound;
                PlayerBase[] winArray = playerArray.OrderBy(player => player.GetCurrentGold()).ToArray();
                winArray[3].Win();
                hasWiner = true;
            }
        }
    }

    public Propertie GetPropertie(int currentPosition)
    {
        return propertiesArray[currentPosition];
    }

    public void ClearPlayerProperties(PlayerBase loserPlayer)
    {
        contLosers++;
        foreach (Propertie propertie in propertiesArray)
        {
            if (propertie.owner == loserPlayer)
            {
                propertie.owner = null;
            }
        }
    }
}


