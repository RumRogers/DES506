using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{

    public static class JSONTipReader
    { 
        public static void LoadAllTips(string[] tips, TextAsset jsonFile)
        {
            tips = JsonUtility.FromJson<string[]>(jsonFile.text);

            //Debug.Log(tipArr[0]);
            Debug.Log(tips.Length);
        }

    }

}