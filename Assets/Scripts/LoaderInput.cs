using UnityEngine;
using System;

public class LoaderInput : MonoBehaviour
{

    private static string[] gameConfig;    


    public static void LoadArchive()
    {
        TextAsset gameConfigReader = Resources.Load("gameConfig") as TextAsset;
        //new StreamReader(Application.dataPath + "/Resources/gameConfig.txt");
        string fullText = gameConfigReader.text;
        gameConfig = fullText.Split(new[] { "\n" },StringSplitOptions.None);
    }


    public static int GetSellValue(int propertiesIndex)
    {
        string line = gameConfig[propertiesIndex];
        return int.Parse(line.Split(' ')[0]);
    }

    public static int GetRentValue(int propertiesIndex)
    {
        string line = gameConfig[propertiesIndex];
        string[] valueString = line.Split(' ');
        return int.Parse(valueString[1]);
    }
}
