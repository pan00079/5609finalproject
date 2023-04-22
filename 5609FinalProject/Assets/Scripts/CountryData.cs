using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountryData : MonoBehaviour
{
    public enum FlowerType { FullyGrownOne, AlmostGrownTwo, FloweringThree, BuddingFour, VegetativeFive, SeedlingSix }

    // Data fields
    public FlowerType flowerType; // @ renamed from countryType
    float stemScalingFactor;
    GameObject enabledFlower;
    Outline outline;

    // Time Dataset
    public int id;
    string countryName;
    string surveyYears;

    // Color Gradients
    public Gradient gradientWater, gradientPetals, gradientCenter, gradientStem;

    // Time Categories
    // @ Refactored into a large dictionary. Accessing/modifying category values
    //   is now as simply as checking categories["sleeping"].
    Dictionary<string, int> categories = new Dictionary<string, int>
    {
        { "PaidWorkOrStudyTotal", 0 },
        { "PaidWorkJobs", 0 },
        { "TravelToFromWork", 0 },
        { "SchoolOrClasses", 0 },
        { "OtherPaidWork", 0 },              // otherPaidWork includes hw, job searching and other
        
        { "UnpaidWorkTotal", 0 },
        { "RoutineHousework", 0 },           // includes household travel
        { "CareForOthers", 0 },              // includes all care categories, volunteering
        { "OtherUnpaid", 0 },                // includes shopping but not child/adult care 
        
        { "PersonalCareTotal", 0 },
        { "Sleeping", 0 },
        { "EatingDrinking", 0 },
        { "HouseholdMedicalServices", 0 },
        
        { "LeisureTotal", 0 },
        { "AttendingEvents", 0 },
        { "VisitingFriends", 0 },
        { "TVOrRadio", 0 },
        { "OtherLeisure", 0 },
        
        { "OtherTotal", 0 },
        
        { "DisposableIncome", 0 },        // Better Life Index Categories
        { "EmploymentRate", 0 },          // center color     
        { "SupportNetwork", 0 },          // stem length
        { "SelfReportedHealth", 0 },      // flower color
    };

    // these are seperate from the dict bc they are floats
    float lifeExpectancy; // stem color
    float lifeSatisfaction; // flower type

    // Derived Data
    float workToLeisureRatio;

    void Start() {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineWidth = 3.5f;
        outline.enabled = false;
    }

    void Update() { }

    public void SetLightingIfSelected(bool selected)
    {
        outline.enabled = selected;
    }

    public void SetFlowerTypeBasedOnLifeSatisfaction()
    {
        var flowerTypeBins = new[] {
            Tuple.Create(0f, 5f, FlowerType.SeedlingSix),
            Tuple.Create(5f, 6f, FlowerType.VegetativeFive),
            Tuple.Create(6f, 6.5f, FlowerType.BuddingFour),
            Tuple.Create(6.5f, 7f, FlowerType.FloweringThree),
            Tuple.Create(7f, 7.5f, FlowerType.AlmostGrownTwo),
            Tuple.Create(7.5f, float.MaxValue, FlowerType.FullyGrownOne),
        };
        var bin = flowerTypeBins.FirstOrDefault(
        bin => lifeSatisfaction >= bin.Item1 && lifeSatisfaction < bin.Item2);
        if (bin != null) {
            flowerType = (FlowerType) bin.Item3; // Item3 is the third item in the tuple (the flowertype)
        }
    }

    public void SetLabel() {
        TextMeshProUGUI label = GetComponentInChildren<TextMeshProUGUI>();
        label.text = name;

    }

    // @ Refactored to not be if/switch since enums have an implicit int value.
    public void SetValuesBasedOnFlowerType() 
    {
        stemScalingFactor = 1.92857f + 0.188571f * (int) flowerType;
        enabledFlower = transform.GetChild((int) flowerType + 2).gameObject;
        enabledFlower.SetActive(true);
    }

    public void setStemLength(float minLifeExpectancy, float maxLifeExpectancy)
    {
        float lerpAmt = (float) (lifeExpectancy - minLifeExpectancy) / (maxLifeExpectancy - minLifeExpectancy);
        float length = Mathf.Lerp(1f, stemScalingFactor, lerpAmt);
        if (flowerType != FlowerType.SeedlingSix)
        {
            float thickness = enabledFlower.transform.GetChild(1).localScale.x;
            enabledFlower.transform.GetChild(1).localScale = new Vector3(thickness, length, thickness);
        }
            
        else
        {
            float thickness = enabledFlower.transform.localScale.x;
            enabledFlower.transform.localScale = new Vector3(thickness, length, thickness);
        }
    }

    public void setColorOfWater()
    {
        for (int i = 0; i < transform.childCount; i++) {
            float val = (float) (2.03f - workToLeisureRatio) / (2.03f - 0.63f);
            Color color = gradientWater.Evaluate(val);
            MeshRenderer meshRenderer = transform.GetChild(1).GetComponent<MeshRenderer>();
            color.a = 0.85f;
            meshRenderer.material.SetColor("_WaterColor", color);
        }
    }

    public void setColorOfFlowerCenter(int minIncome, int maxIncome)
    {
        float val = (float)(categories["DisposableIncome"] - minIncome) / (maxIncome - minIncome);
        Color color = gradientCenter.Evaluate(val);
        MeshRenderer meshRenderer;
        if (flowerType != FlowerType.SeedlingSix)
        {
            meshRenderer = enabledFlower.transform.GetChild(1).GetComponent<MeshRenderer>();
        }
        else
        {
            meshRenderer = enabledFlower.transform.GetComponent<MeshRenderer>();
        }
        meshRenderer.materials[1].SetColor("_Color", color);
    }

    public void setColorOfFlowerPetals(int minHealth, int maxHealth)
    {
        if (flowerType != FlowerType.SeedlingSix)
        {
            float val = (float)(categories["SelfReportedHealth"] - minHealth) / (maxHealth - minHealth);
            Color color = gradientPetals.Evaluate(val);
            MeshRenderer meshRenderer;
            meshRenderer = enabledFlower.transform.GetChild(0).GetComponent<MeshRenderer>();
            meshRenderer.material.SetColor("_Color", color);
        }
    }

    public void setColorOfFlowerStem(int minSupport, int maxSupport)
    {
        float val = (float)(categories["SupportNetwork"] - minSupport) / (maxSupport - minSupport);
        Color color = gradientStem.Evaluate(val);
        MeshRenderer meshRenderer;
        if (flowerType != FlowerType.SeedlingSix)
        {
            meshRenderer = enabledFlower.transform.GetChild(1).GetComponent<MeshRenderer>();
        }
        else
        {
            meshRenderer = enabledFlower.transform.GetComponent<MeshRenderer>();
        }
        meshRenderer.materials[0].SetColor("_Color", color);
    }

    public void setPreliminaryData(int id, string name, string surveyYears)
    {
        this.id = id;
        this.name = name;
        this.surveyYears = surveyYears;
    }

    // @ Doing this so we don't need 100 getters/setters for each category value.
    public void setTimeVal(string timeCategory, int value)
    {
        categories[timeCategory] = value;
    }
    public float getTimeVal(string timeCategory)
    {
        return categories[timeCategory];
    }

    public void setBetterLifeIndexData(
        int disposableIncome, int employmentRate, int supportNetwork,
        float expectancy, int selfReportedHealth, float satisfaction)
    {
        categories["DisposableIncome"] = disposableIncome;
        categories["EmploymentRate"] = employmentRate;
        categories["SupportNetwork"] = supportNetwork;
        lifeExpectancy = expectancy;
        categories["SelfReportedHealth"] = selfReportedHealth;
        lifeSatisfaction = satisfaction;
    }

    public void setWorkToLeisureRatio(float workToLeisureRatio)
    {
        this.workToLeisureRatio = workToLeisureRatio;
    }

    public float getWorkToLeisureRatio()
    {
        return workToLeisureRatio;
    }

    public float getLifeExpectancy()
    {
        return lifeExpectancy;
    }

    public float getLifeSatisfaction()
    {
        return lifeSatisfaction;
    }

    public void setLifeExpectancy(float f)
    {
        lifeExpectancy = f;
    }

    public void setLifeSatisfaction(float f) 
    {
        lifeSatisfaction = f;
    }
}
