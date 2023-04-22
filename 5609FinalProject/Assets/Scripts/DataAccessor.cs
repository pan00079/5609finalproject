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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting...");
        if (jarPrefab == null || datasetCollection == null)
        {
            throw new System.Exception("GameObject variables (prefab or datasetCollection) are not set. Please set those, then run again.");
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
                dataComponent.setTimeVal("PaidWorkOrStudyTotal", paidWorkTotal);

                int unpaidWorkTotal = int.Parse(countryData[7]);
                dataComponent.setTimeVal("UnpaidWorkTotal", unpaidWorkTotal);

                int personalCareTotal = int.Parse(countryData[10]);
                dataComponent.setTimeVal("PersonalCareTotal", personalCareTotal);

                int sleeping = int.Parse(countryData[11]);
                dataComponent.setTimeVal("Sleeping", sleeping);

                int leisureTotal = int.Parse(countryData[14]);
                dataComponent.setTimeVal("LeisureTotal", leisureTotal);

                int otherTotal = int.Parse(countryData[18]);
                dataComponent.setTimeVal("OtherTotal", otherTotal);

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

        float[] minMaxFloat = GetMaxMinInCategoryFloat("LifeSatisfaction");
        float minLifeSatisfaction = minMaxFloat[0];
        float maxLifeSatisfaction = minMaxFloat[1];

        minMaxFloat = GetMaxMinInCategoryFloat("LifeExpectancy");
        float minLifeExpectancy = minMaxFloat[0];
        float maxLifeExpectancy = minMaxFloat[1];

        CountryData[] minMaxArray = listOfCountries.OrderBy(country => country.getWorkToLeisureRatio()).ToArray();
        float minWorkToLeisureRatio = minMaxArray[0].getWorkToLeisureRatio();
        float maxWorkToLeisureRatio = minMaxArray[minMaxArray.Length - 1].getWorkToLeisureRatio();

        int[] minMaxInt = GetMaxMinInCategory("PaidWorkOrStudyTotal");
        int minPaidWorkTime = minMaxInt[0];
        int maxPaidWorkTime = minMaxInt[1];

        minMaxInt = GetMaxMinInCategory("SupportNetwork");
        int minSupport = minMaxInt[0];
        int maxSupport = minMaxInt[1];

        minMaxInt = GetMaxMinInCategory("DisposableIncome");
        int minIncome = minMaxInt[0];
        int maxIncome = minMaxInt[1];

        minMaxInt = GetMaxMinInCategory("SelfReportedHealth");
        int minHealth = minMaxInt[0];
        int maxHealth = minMaxInt[1];


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

    public int[] GetMaxMinInCategory(string category)
    {
        CountryData[] minMaxArray = listOfCountries.OrderBy(country => country.getTimeVal(category)).ToArray();
        int[] maxMin = { (int)minMaxArray[0].getTimeVal(category), (int)minMaxArray[minMaxArray.Length - 1].getTimeVal(category) };
        //Debug.Log(maxMin[0] + " " + maxMin[1]);
        return maxMin;
    }

    public float[] GetMaxMinInCategoryFloat(string category)
    {
        float[] maxMin = new float[2];
        if (category.Equals("LifeSatisfaction"))
        {
            CountryData[] minMaxArray = listOfCountries.OrderBy(country => country.getLifeSatisfaction()).ToArray();
            maxMin[0] = minMaxArray[0].getLifeSatisfaction();
            maxMin[1] = minMaxArray[minMaxArray.Length - 1].getLifeSatisfaction();
        }
        else if (category.Equals("LifeExpectancy"))
        {
            CountryData[] minMaxArray = listOfCountries.OrderBy(country => country.getLifeExpectancy()).ToArray();
            maxMin[0] = minMaxArray[0].getLifeExpectancy();
            maxMin[1] = minMaxArray[minMaxArray.Length - 1].getLifeExpectancy();
        }

        return maxMin;
    }
}

