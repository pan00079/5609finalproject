using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAccessor : MonoBehaviour
{
    public string csvDataPath;
    public List<GameObject> listOfCountries;
    public GameObject jarPrefab;

    // serves as a parent for all individual country data
    public GameObject datasetCollection;
    public GameObject dataProcessor;

    // Start is called before the first frame update
    void Start()
    {
        if (jarPrefab == null || datasetCollection == null || dataProcessor == null)
        {
            throw new System.Exception("GameObject variables (prefab, datasetCollection, or processor) are not set. Please set those, then run again.");
        }

        string[] csvDataFile = File.ReadAllLines(csvDataPath);
        parseCountryData(csvDataFile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Takes in country CSV data (Time Use + Better Life Index) and
    // instantiates a jar, populating it with its respective country data.
    void parseCountryData(string[] csvDataFile)
    {
        List<float> lifeSatisfactionList = new List<float>();
        List<int> paidWorktimeList = new List<int>();
        List<float> workToLeisureRatioList = new List<float>();

        foreach (string line in csvDataFile)
        {
            int countryId;
            string[] countryData = line.Split(',');

            if (int.TryParse(countryData[0], out countryId))
            {
                GameObject countryObject = Instantiate(jarPrefab, datasetCollection.transform);
                CountryData dataComponent = countryObject.GetComponent<CountryData>();
                // = int.Parse(countryData[0]);
                string countryName = countryData[1];
                string surveyYear = countryData[2];
                dataComponent.setPreliminaryData(countryId, countryName, surveyYear);
                countryObject.name = countryName;

                int paidWorkTotal = int.Parse(countryData[3]);
                // other parsing here
                dataComponent.setPaidWorkTimes(paidWorkTotal);

                int unpaidWorkTotal = int.Parse(countryData[7]);
                // other parsing here
                dataComponent.setUnpaidWorkTimes(unpaidWorkTotal);

                int personalCareTotal = int.Parse(countryData[10]);
                // other parsing here
                dataComponent.setPersonalCareTimes(personalCareTotal);

                int leisureTotal = int.Parse(countryData[14]);
                // other parsing here
                dataComponent.setLeisureTimes(leisureTotal);

                int otherTotal = int.Parse(countryData[18]);
                dataComponent.setOtherTime(otherTotal);

                int disposableIncome = int.Parse(countryData[21]);
                int employmentRate = int.Parse(countryData[22]);
                int supportNetwork = int.Parse(countryData[23]);
                float lifeExpectancy = float.Parse(countryData[24]);
                int selfReportedHealth = int.Parse(countryData[25]);
                float lifeSatisfaction = float.Parse(countryData[26]);
                dataComponent.setBetterLifeIndexData(
                    disposableIncome, employmentRate, supportNetwork,
                    lifeExpectancy, selfReportedHealth, lifeSatisfaction);

                float workToLeisureRatio = (float)paidWorkTotal / leisureTotal;
                dataComponent.setWorkToLeisureRatio(workToLeisureRatio);

                // add to list so that we know min and stuff
                lifeSatisfactionList.Add(lifeSatisfaction);
                paidWorktimeList.Add(paidWorkTotal);
                workToLeisureRatioList.Add(workToLeisureRatio);
            }
        }

        float minLifeSatisfaction = lifeSatisfactionList.Min();
        int minPaidWorkTime = paidWorktimeList.Min();
        float minWorkToLeisureRatio = workToLeisureRatioList.Min();
        float maxLifeSatisfaction = lifeSatisfactionList.Max();
        int maxPaidWorkTime = paidWorktimeList.Max();
        float maxWorkToLeisureRatio = workToLeisureRatioList.Max();

        DataProcessor processorComponent = dataProcessor.GetComponent<DataProcessor>();
        processorComponent.setMinAndMaxValues(minLifeSatisfaction, minPaidWorkTime, minWorkToLeisureRatio,
            maxLifeSatisfaction, maxPaidWorkTime, maxWorkToLeisureRatio);
        processorComponent.setCountriesPosition();
    }

}

