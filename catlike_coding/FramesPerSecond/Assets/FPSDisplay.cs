﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FPSCounter))]
public class FPSDisplay : MonoBehaviour
{
    static string[] stringsFrom00To99 = {
        "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
        "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
        "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
        "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
        "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
        "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
        "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
        "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
        "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
        "90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
    };

    public Text averageFpsLabel;
    public Text minFpsLabel;
    public Text maxFpsLabel;
    
    private FPSCounter fpsCounter;


    [System.Serializable]
    private struct FPSColor {
        public Color color;
        public int minimumFPS;
    }
    
    [SerializeField]
    private FPSColor[] coloring;
    
    private void Start()
    {
        fpsCounter = GetComponent<FPSCounter>();
    }
    void Update()
    {
        Display(averageFpsLabel, Mathf.Clamp(fpsCounter.AverageFPS, 0, 99));
        Display(minFpsLabel, Mathf.Clamp(fpsCounter.MinFPS, 0, 99));
        Display(maxFpsLabel, Mathf.Clamp(fpsCounter.MaxFPS, 0, 99));
    }
    
    private void Display(Text label, int value)
    {
        label.text = stringsFrom00To99[value];
        // Assumption: colors are in descending order
        for (int i = 0; i < coloring.Length; i++)
        {
            var c = coloring[i];
            if(value >= c.minimumFPS)
            {
                label.color = c.color;
                break;
            }
        }
    }
}
