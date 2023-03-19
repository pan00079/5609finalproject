using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAccessor : MonoBehaviour
{
    public string csvDataPath;
    public string[] csvDataFile;
    public List<string[]> splitData;

    // Start is called before the first frame update
    void Start()
    {
        splitData = new List<string[]>();
        csvDataFile = File.ReadAllLines(csvDataPath);
        foreach(string line in csvDataFile) {
            string toSplit = line.Replace("\"", "");
            splitData.Add(toSplit.Split(','));
        }

        Debug.Log(csvDataFile[1]);
        Debug.Log(csvDataFile[2]);
        Debug.Log(csvDataFile[3]);
        Debug.Log(csvDataFile[4]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
