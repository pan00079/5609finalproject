using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DataProcessor : MonoBehaviour
{
    public enum AxisName
    {
        PaidWorkOrStudyTotal,
        UnpaidWorkTotal,
        PersonalCareTotal,
        LeisureTotal,
        OtherTotal,
        Sleeping,
        DisposableIncome,
        EmploymentRate,
        SupportNetwork,
        SelfReportedHealth,
        LifeExpectancy,
        LifeSatisfaction
    }

    public DataAccessor dataAccessor;
    public GameObject datasetCollection;
    public int sizeXAxis;
    public int sizeZAxis;
    public AxisName XAxis;
    public AxisName YAxis;
    public TextMeshProUGUI XAxisLabel;
    public TextMeshProUGUI YAxisLabel;

    AxisName currentXAxis;
    AxisName currentYAxis;

    // Start is called before the first frame update
    void Start()
    {
        if (datasetCollection == null)
        {
            throw new System.Exception("GameObject variable (datasetCollection) is not set. Please set it, then run again.");
        }
        currentXAxis = XAxis;
        currentYAxis = YAxis;

        updateCountryPositionAndLabel(true, XAxis);
        updateCountryPositionAndLabel(false, YAxis);
    }

    // Update is called once per frame
    void Update()
    {
        if (XAxis != currentXAxis)
        {
            updateCountryPositionAndLabel(true, XAxis);
        }
        if (YAxis != currentYAxis)
        {
            updateCountryPositionAndLabel(false, YAxis);
        }
    }

    public void updateCountryPositionAndLabel(bool updateX, AxisName category)
    {
        float min;
        float max;

        if (category == AxisName.LifeExpectancy)
        {
            float[] arr = dataAccessor.GetMaxMinInCategoryFloat("LifeExpectancy");
            min = arr[0];
            max = arr[1];
        }
        else if (category == AxisName.LifeSatisfaction)
        {
            float[] arr = dataAccessor.GetMaxMinInCategoryFloat("LifeSatisfaction");
            min = arr[0];
            max = arr[1];
        }
        else
        {
            int[] arr = dataAccessor.GetMaxMinInCategory(category.ToString());
            min = arr[0];
            max = arr[1];
        }

        int numCountries = datasetCollection.transform.childCount;
        for (int i = 0; i < numCountries; i++)
        {
            GameObject go = datasetCollection.transform.GetChild(i).gameObject;
            CountryData data = go.GetComponent<CountryData>();

            float val;

            if (category == AxisName.LifeExpectancy)
            {
                val = data.getLifeExpectancy();
            }
            else if (category == AxisName.LifeSatisfaction)
            {
                val = data.getLifeSatisfaction();
            }
            else
            {
                val = data.getTimeVal(category.ToString());
            }

            float lerpAmt = (val - min) / (max - min);
            float x;
            float z;

            if (updateX)
            {  
                x = Mathf.Lerp(-sizeXAxis, sizeXAxis, lerpAmt);
                z = go.transform.localPosition.z;
                currentXAxis = XAxis;
            }
            else
            {
                x = go.transform.localPosition.x;
                z = Mathf.Lerp(-sizeXAxis, sizeXAxis, lerpAmt);
                currentYAxis = YAxis;
            }

            go.transform.localPosition = new Vector3(x, 1.05f, z);
            go.transform.localRotation = Quaternion.Euler(new Vector3(0f, Random.Range(-20.0f, 20.0f), 0f));
        }

        StringBuilder sb = new StringBuilder("<------------------------------------------------------->\n");
        
        if (updateX)
        {
            sb.AppendFormat("     {0}\t\t\t\t\t\t\t\t{1}\n\t\t", min, max);
            sb.Append(category.ToString());
            XAxisLabel.SetText(sb.ToString());
        }
        else
        {
            sb.AppendFormat("     {0}\t\t\t\t\t\t\t\t{1}\n\t\t", max, min);
            sb.Append(category.ToString());
            YAxisLabel.SetText(sb.ToString());
        }


        
    }
}
