using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectScene : MonoBehaviour
{
    [SerializeField] private GameObject data;
    [SerializeField] private GameObject plane;
    [SerializeField] private GameObject room;
    [SerializeField] private GameObject props;
    private int scence;

    void Start()
    {
        Data currentScene = data.GetComponent<Data>();
        currentScene.OnCurentScenes += ComponentCurentScenes;
        room.SetActive(false);
        props.SetActive(false);
    }

    private void ComponentCurentScenes(object sender, Data.CurrentEventArgs e)
    {
        scence = e.CurrentSceneNum;
       
        if (scence == 1) {
            room.SetActive(true);
            props.SetActive(true);
            plane.SetActive(false);
        }

        if (scence == 2)
        {
            room.SetActive(true);
            props.SetActive(false);
            plane.SetActive(false);
        }

        else
        {
            room.SetActive(false);
            props.SetActive(false);
            plane.SetActive(true);
        }

    }
}
