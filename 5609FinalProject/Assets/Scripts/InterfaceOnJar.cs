using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceOnJar: MonoBehaviour
{
    GameObject selectedJar;
    Dictionary<string, int> totals = new Dictionary<string, int>();
    // Should be read in instead, but they're just here for now for ease of use
    string countryName;
    int MAX_TOTAL = 752;
    int MIN_TOTAL = 6;

    void Start()
    {
        selectedJar = null;
        Canvas canvas = GameObject.Find("Canvas2D").GetComponent<Canvas>();
        canvas.enabled = false;
        Debug.Log("Starting Interface");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.gameObject.tag == "FlowerJar")
                {
                    selectedJar = hit.transform.gameObject;
                    CountryData countryData = selectedJar.GetComponent<CountryData>();
                    if (countryName == countryData.getName()) {
                        updateVisibility();
                        return;
                    }
                    fillRelevantInformation(countryData);
                    setBars(countryData.getName());
                    updateVisibility(true);
                } else {
                    updateVisibility(false);
                }
            }
        }
    }

    void updateVisibility(bool enable) {
        Canvas canvas = GameObject.Find("Canvas2D").GetComponent<Canvas>();
        canvas.enabled = enable;
    }

    void updateVisibility() {
        Canvas canvas = GameObject.Find("Canvas2D").GetComponent<Canvas>();
        canvas.enabled = !canvas.enabled;
    }

    // updates based on 
    void setBars(string name) {
        GameObject canvasGameObject = GameObject.Find("Canvas2D");
        Transform canvasTransform = canvasGameObject.GetComponent<Transform>().Find("Panel");
        foreach (KeyValuePair<string, int> pair in totals)
        {
            int barLength = getBarLength(pair.Value);
            Transform valTransform = canvasTransform.Find(pair.Key);

            // get the numerical value
            Text valText = valTransform.Find("Number").GetComponent<Text>();
            valText.text = pair.Value.ToString();
    
            // set the bar length
            Debug.Log(barLength);
            RectTransform bar = valTransform.GetComponent<RectTransform>();
            bar.sizeDelta = new Vector2(barLength, bar.sizeDelta.y);
        }
        // set country name
        Text countryName = canvasTransform.Find("Country").GetComponent<Text>();
        this.countryName = countryName.text;
        countryName.text = name;
    }

    int getBarLength(int val) {
        int MAX_BAR_LENGTH = 200;
        float percent = (float) ((float) val / (float) (MAX_TOTAL - MIN_TOTAL));
        return (int) (percent * MAX_BAR_LENGTH);
    }

    // retrieves important bar information
    void fillRelevantInformation(CountryData countryData) {
        totals["Paid"] = countryData.getTimeVal("PaidWorkOrStudyTotal");
        totals["Unpaid"] = countryData.getTimeVal("UnpaidWorkTotal");
        totals["Care"] = countryData.getTimeVal("PersonalCareTotal");
        totals["Leisure"] = countryData.getTimeVal("LeisureTotal");
        totals["Other"] = countryData.getTimeVal("OtherTotal");
        Debug.Log("totals data is filled");
    }

    // for debugging purposes
    void printRelevantInformation() {
        foreach (KeyValuePair<string, int> pair in totals) {
            Debug.Log(pair.Key + ": " + pair.Value);
        }
    }
}
