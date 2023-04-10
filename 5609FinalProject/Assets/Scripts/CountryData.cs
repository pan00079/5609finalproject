using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryData : MonoBehaviour
{
    public enum FlowerType
    {
        FullyGrownOne, AlmostGrownTwo, FloweringThree, 
        BuddingFour, VegetativeFive, SeedlingSix
    }

    public FlowerType countryType;
    float stemScalingFactor;
    GameObject enabledFlower;

    // Water Color Gradient
    public Gradient gradientWater;
    public Gradient gradientPetals;
    public Gradient gradientCenter;
    public Gradient gradientStem;

    // Time Dataset
    public int id;
    string countryName;
    string surveyYears;

    // Time Categories
    // Paid Work
    int paidWorkOrStudyCategoryTotal;   // main category
    int paidWorkJobs;
    int travelToFromWork;
    int schoolOrClasses;
    int otherPaidWork;                  // includes hw, job searching and other

    // Unpaid Work
    int unpaidWorkCategoryTotal;        // main category
    int routineHousework;               // includes household travel
    int careForOthers;                  // includes all care categories, volunteering
    int otherUnpaid;                    // includes shopping but not child/adult care 
                                        // (those are under careForOthers)

    // Personal Care
    int personalCareCategoryTotal;      // main category
    int sleeping;
    int eatingDrinking;
    int personalHouseholdMedicalServices;

    // Leisure
    int leisureCategoryTotal;           // main category
    int attendingEvents;                // includes sports
    int visitingFriends;
    int TVOrRadio;
    int otherLeisure;

    // Misc Category
    int otherCategoryTotal;             // main category

    // Better Life Index Categories
    int disposableIncome;   // center color
    int employmentRate;     // ??
    int supportNetwork;     // stem length
    float lifeExpectancy;   // stem color
    int selfReportedHealth; // flower color
    float lifeSatisfaction; // flower type

    // Derived Data
    float workToLeisureRatio;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void SetFlowerTypeBasedOnLifeSatisfaction()
    {
        if (lifeSatisfaction < 5)
        {
            countryType = FlowerType.SeedlingSix;
        }
        else if (lifeSatisfaction < 6)
        {
            countryType = FlowerType.VegetativeFive;
        }
        else if (lifeSatisfaction < 6.5)
        {
            countryType = FlowerType.BuddingFour;
        }
        else if (lifeSatisfaction < 7)
        {
            countryType = FlowerType.FloweringThree;
        }
        else if (lifeSatisfaction < 7.5)
        {
            countryType = FlowerType.AlmostGrownTwo;
        }
        else
        {
            countryType = FlowerType.FullyGrownOne;
        }
    }

    public void SetValuesBasedOnFlowerType()
    {
        Debug.Log("Setting Flower Type...");
        switch (countryType)
        {
            case FlowerType.FullyGrownOne:
                stemScalingFactor = 2f;
                enabledFlower = transform.GetChild(2).gameObject;
                enabledFlower.SetActive(true);
                break;
            case FlowerType.AlmostGrownTwo:
                stemScalingFactor = 2.02f;
                enabledFlower = transform.GetChild(3).gameObject;
                enabledFlower.SetActive(true);
                break;
            case FlowerType.FloweringThree:
                stemScalingFactor = 2.25f;
                enabledFlower = transform.GetChild(4).gameObject;
                enabledFlower.SetActive(true);
                break;
            case FlowerType.BuddingFour:
                stemScalingFactor = 2.39f;
                enabledFlower = transform.GetChild(5).gameObject;
                enabledFlower.SetActive(true);
                break;
            case FlowerType.VegetativeFive:
                stemScalingFactor = 2.65f;
                enabledFlower = transform.GetChild(6).gameObject;
                enabledFlower.SetActive(true);
                break;
            case FlowerType.SeedlingSix:
                stemScalingFactor = 3.2f;
                enabledFlower = transform.GetChild(7).gameObject;
                enabledFlower.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void setStemLength(float minLifeExpectancy, float maxLifeExpectancy)
    {
        float lerpAmt = (float) (lifeExpectancy - minLifeExpectancy) / (maxLifeExpectancy - minLifeExpectancy);
        float length = Mathf.Lerp(1f, stemScalingFactor, lerpAmt);
        if (countryType != FlowerType.SeedlingSix)
        {
            enabledFlower.transform.GetChild(1).localScale = new Vector3(1f, length, 1f);
        }
        else
        {
            enabledFlower.transform.localScale = new Vector3(1f, length, 1f);
        }
    }

    // Determines the color of the water based on 
    public void setColorOfWater()
    {
        Debug.Log("Setting Color of the Water...");
        for (int i = 0; i < transform.childCount; i++) {
            float val = (float) (2.03f - workToLeisureRatio) / (2.03f - 0.63f);
            Color color = gradientWater.Evaluate(val);
            MeshRenderer meshRenderer = transform.GetChild(1).GetComponent<MeshRenderer>();
            color.a = 0.80f;
            meshRenderer.material.SetColor("_Color", color);
        }
    }

    public void setColorOfFlowerCenter(int minIncome, int maxIncome)
    {
        Debug.Log("Setting Color of the Flower Center...");
        float val = (float)(disposableIncome - minIncome) / (maxIncome - minIncome);
        Color color = gradientCenter.Evaluate(val);
        MeshRenderer meshRenderer;

        if (countryType != FlowerType.SeedlingSix)
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
        if (countryType != FlowerType.SeedlingSix)
        {
            Debug.Log("Setting Color of the Flower Petals...");
            float val = (float)(selfReportedHealth - minHealth) / (maxHealth - minHealth);
            Color color = gradientPetals.Evaluate(val);
            MeshRenderer meshRenderer;
            meshRenderer = enabledFlower.transform.GetChild(0).GetComponent<MeshRenderer>();
            meshRenderer.material.SetColor("_Color", color);
        }
    }

    public void setColorOfFlowerStem(int minSupport, int maxSupport)
    {
        Debug.Log("Setting Color of the Flower Stem...");
        float val = (float)(supportNetwork - minSupport) / (maxSupport - minSupport);
        Color color = gradientStem.Evaluate(val);
        MeshRenderer meshRenderer;
        if (countryType != FlowerType.SeedlingSix)
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

    public void setPaidWorkTimes(int paidWorkOrStudyCategoryTotal)
    {
        this.paidWorkOrStudyCategoryTotal = paidWorkOrStudyCategoryTotal;
        // this.paidWorkJobs = paidWorkJobs;
        // this.travelToFromWork = travelToFromWork;
        // this.schoolOrClasses = schoolOrClasses;
        // this.otherPaidWork = otherPaidWork;
    }

    public void setUnpaidWorkTimes(int unpaidWorkCategoryTotal)
    {
        this.unpaidWorkCategoryTotal = unpaidWorkCategoryTotal;
        // this.routineHousework = routineHousework;
        // this.careForOthers = careForOthers;
        // this.otherUnpaid = otherUnpaid;
    }

    public void setPersonalCareTimes(int personalCareCategoryTotal)
    {
        this.personalCareCategoryTotal = personalCareCategoryTotal;
        // this.sleeping = sleeping;
        // this.eatingDrinking = eatingDrinking;
        // this.personalHouseholdMedicalServices = personalHouseholdMedicalServices;
    }

    public void setLeisureTimes(int leisureCategoryTotal)
    {
        this.leisureCategoryTotal = leisureCategoryTotal;
        // this.attendingEvents = attendingEvents;
        // this.visitingFriends = visitingFriends;
        // this.TVOrRadio = TVOrRadio;
        // this.otherLeisure = otherLeisure;
    }

    public void setOtherTime(int otherCategoryTotal)
    {
        this.otherCategoryTotal = otherCategoryTotal;
    }

    public void setBetterLifeIndexData(
        int disposableIncome, int employmentRate, int supportNetwork,
        float lifeExpectancy, int selfReportedHealth, float lifeSatisfaction)
    {
        this.disposableIncome = disposableIncome;
        this.employmentRate = employmentRate;
        this.supportNetwork = supportNetwork;
        this.lifeExpectancy = lifeExpectancy;
        this.selfReportedHealth = selfReportedHealth;
        this.lifeSatisfaction = lifeSatisfaction;
    }

    public void setWorkToLeisureRatio(float workToLeisureRatio)
    {
        this.workToLeisureRatio = workToLeisureRatio;
    }

    public float getLifeSatisfaction()
    {
        return lifeSatisfaction;
    }

    public int getTotalWorkHours()
    {
        return paidWorkOrStudyCategoryTotal;
    }

    public float getWorkToLeisureRatio()
    {
        return workToLeisureRatio;
    }

    public int getSupportNetwork()
    {
        return supportNetwork;
    }
    public int getDisposableIncome()
    {
        return disposableIncome;
    }

    public float getLifeExpectancy()
    {
        return lifeExpectancy;
    }

    public int getSelfReportedHealth()
    {
        return selfReportedHealth;
    }
}
