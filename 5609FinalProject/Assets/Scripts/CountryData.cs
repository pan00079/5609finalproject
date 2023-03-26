using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryData : MonoBehaviour
{
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
    int disposableIncome;
    int employmentRate;
    int supportNetwork;
    float lifeExpectancy;
    int selfReportedHealth;
    float lifeSatisfaction;

    // Derived Data
    float workToLeisureRatio;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Determines the color of the water based on 
    public void setColorOfWater()
    {

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
}
