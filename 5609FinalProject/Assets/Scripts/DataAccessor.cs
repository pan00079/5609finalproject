using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAccessor : MonoBehaviour
{
    public string csvDataPath;
    public List<CountryData> listOfCountries;
    public GameObject jarPrefab;

    // serves as a parent for all individual country data
    public GameObject datasetCollection;
    public GameObject dataProcessor;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting...");
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

                listOfCountries.Add(dataComponent);

                string countryName = countryData[1];
                string surveyYear = countryData[2];
                dataComponent.setPreliminaryData(countryId, countryName, surveyYear);
                countryObject.name = countryName;

                int paidWorkTotal = int.Parse(countryData[3]);
                dataComponent.setTimeVal("PaidWorkOrStudyCategoryTotal", paidWorkTotal);

                int unpaidWorkTotal = int.Parse(countryData[7]);
                dataComponent.setTimeVal("UnpaidWorkCategoryTotal", unpaidWorkTotal);

                int personalCareTotal = int.Parse(countryData[10]);
                dataComponent.setTimeVal("PersonalCareCategoryTotal", personalCareTotal);

                int leisureTotal = int.Parse(countryData[14]);
                dataComponent.setTimeVal("LeisureCategoryTotal", leisureTotal);

                int otherTotal = int.Parse(countryData[18]);
                dataComponent.setTimeVal("OtherCategoryTotal", otherTotal);

                int disposableIncome = int.Parse(countryData[21]);
                int employmentRate = int.Parse(countryData[22]);
                int supportNetwork = int.Parse(countryData[23]);
                float lifeExpectancy = float.Parse(countryData[24]);
                int selfReportedHealth = int.Parse(countryData[25]);
                float lifeSatisfaction = float.Parse(countryData[26]);
                dataComponent.setBetterLifeIndexData(
                    disposableIncome, employmentRate, supportNetwork,
                    lifeExpectancy, selfReportedHealth, lifeSatisfaction
                );

                float workToLeisureRatio = (float) paidWorkTotal / leisureTotal;
                dataComponent.setWorkToLeisureRatio (workToLeisureRatio);
            }
        }

        CountryData[] minMaxArray = listOfCountries.OrderBy(country => country.getLifeSatisfaction()).ToArray();
        float minLifeSatisfaction = minMaxArray[0].getLifeSatisfaction();
        float maxLifeSatisfaction = minMaxArray[minMaxArray.Length - 1].getLifeSatisfaction();

        minMaxArray = listOfCountries.OrderBy(country => country.getTimeVal("PaidWorkOrStudyCategoryTotal")).ToArray();
        int minPaidWorkTime = (int) minMaxArray[0].getTimeVal("PaidWorkOrStudyCategoryTotal");
        int maxPaidWorkTime = (int) minMaxArray[minMaxArray.Length - 1].getTimeVal("PaidWorkOrStudyCategoryTotal");

        minMaxArray = listOfCountries.OrderBy(country => country.getWorkToLeisureRatio()).ToArray();
        float minWorkToLeisureRatio = minMaxArray[0].getWorkToLeisureRatio();
        float maxWorkToLeisureRatio = minMaxArray[minMaxArray.Length - 1].getWorkToLeisureRatio();

        minMaxArray = listOfCountries.OrderBy(country => country.getTimeVal("SupportNetwork")).ToArray();
        int minSupport = (int) minMaxArray[0].getTimeVal("SupportNetwork");
        int maxSupport = (int) minMaxArray[minMaxArray.Length - 1].getTimeVal("SupportNetwork");

        minMaxArray = listOfCountries.OrderBy(country => country.getTimeVal("DisposableIncome")).ToArray();
        int minIncome = (int) minMaxArray[1].getTimeVal("DisposableIncome");
        int maxIncome = (int) minMaxArray[minMaxArray.Length - 1].getTimeVal("DisposableIncome");

        minMaxArray = listOfCountries.OrderBy(country => country.getTimeVal("SelfReportedHealth")).ToArray();
        int minHealth = (int) minMaxArray[0].getTimeVal("SelfReportedHealth");
        int maxHealth = (int) minMaxArray[minMaxArray.Length - 1].getTimeVal("SelfReportedHealth");

        minMaxArray = listOfCountries.OrderBy(country => country.getLifeExpectancy()).ToArray();
        float minLifeExpectancy = (int) minMaxArray[0].getLifeExpectancy();
        float maxLifeExpectancy = (int) minMaxArray[minMaxArray.Length - 1].getLifeExpectancy();

        DataProcessor processorComponent = dataProcessor.GetComponent<DataProcessor>();
        processorComponent.setMinAndMaxValues(minLifeSatisfaction, minPaidWorkTime, minWorkToLeisureRatio,
            maxLifeSatisfaction, maxPaidWorkTime, maxWorkToLeisureRatio);
        processorComponent.setCountriesPosition();

        foreach (CountryData country in listOfCountries)
        {
            country.SetFlowerTypeBasedOnLifeSatisfaction();
            country.SetValuesBasedOnFlowerType();
            country.setColorOfWater();
            country.setStemLength(minLifeExpectancy, maxLifeExpectancy);
            country.setColorOfFlowerCenter(minIncome, maxIncome);
            country.setColorOfFlowerPetals(minHealth, maxHealth);
            country.setColorOfFlowerStem(minSupport, maxSupport);
            country.SetLabel();
        }
    }
}

