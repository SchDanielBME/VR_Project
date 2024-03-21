using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeadLocation : MonoBehaviour
{
    [SerializeField] private GameObject data;
    Vector3 Vector3= Vector3.zero;

    void Start()
    {
        Data dataInfo = data.GetComponent<Data>();
        dataInfo.OnTaskButtonClicked += TakeStartPose;
        dataInfo.OnStopButtonClicked += TakeEndPose;

    }

    private void TakeStartPose(object sender, EventArgs e)
    {
        // לרכוש מיקום
        // להתחיל זמן 
        // לשלוח את האירוע חזרה לדאטה לאשר שנשמר המיקופ
    }

    private void TakeEndPose(object sender, EventArgs e)
    {
        // לרכוש מיקום
        // להתחיל זמן 
        // לעשות אירוע ששולח מיקום + זמן של סטרט ואנד
        // לשלוח את האירוע לאובג'קט חדש שעושה אנליזה 
    }
    void Update()
    {
        
    }
}
