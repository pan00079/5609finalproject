using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataProcessor : MonoBehaviour
{
    public GameObject datasetCollection;
    public int sizeXAxis;
    public int sizeZAxis;

    float minLifeSatisfaction;
    int minPaidWorkTime;
    float minWorkToLeisureRatio;
    float maxLifeSatisfaction;
    int maxPaidWorkTime;
    float maxWorkToLeisureRatio;

    // Start is called before the first frame update
    void Start()
    {
        if (datasetCollection == null)
        {
            throw new System.Exception("GameObject variable (datasetCollection) is not set. Please set it, then run again.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCountriesPosition()
    {
        int numCountries = datasetCollection.transform.childCount;
        for (int i = 0; i < numCountries; i++)
        {
            GameObject go = datasetCollection.transform.GetChild(i).gameObject;
            CountryData data = go.GetComponent<CountryData>();

            float lifeSatisfaction = data.getLifeSatisfaction();
            int workHours = data.getTotalWorkHours();
            

            float lerpAmtX = (lifeSatisfaction - minLifeSatisfaction) / (maxLifeSatisfaction - minLifeSatisfaction);
            float lerpAmtZ = (float) (workHours - minPaidWorkTime) / (maxPaidWorkTime - minPaidWorkTime);

            float x = Mathf.Lerp(-sizeXAxis, sizeXAxis, lerpAmtX);
            float z = Mathf.Lerp(-sizeZAxis, sizeZAxis, lerpAmtZ);

            go.transform.position = new Vector3(x, 1f, z);
        }

    }

    public void setMinAndMaxValues(
        float minLifeSatisfaction, int minPaidWorkTime, float minWorkToLeisureRatio,
        float maxLifeSatisfaction, int maxPaidWorkTime, float maxWorkToLeisureRatio)
    {
        this.minLifeSatisfaction = minLifeSatisfaction;
        this.minPaidWorkTime = minPaidWorkTime;
        this.minWorkToLeisureRatio = minWorkToLeisureRatio;
        this.maxLifeSatisfaction = maxLifeSatisfaction;
        this.maxPaidWorkTime = maxPaidWorkTime;
        this.maxWorkToLeisureRatio = maxWorkToLeisureRatio;
    }
}
