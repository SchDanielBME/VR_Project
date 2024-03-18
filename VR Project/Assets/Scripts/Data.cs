using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Data : MonoBehaviour
{
    private int[] angels = { 10, 27, 43, 96, 108, 16, 59, 103, 122, 150 };
    private string[] direction = { "right", "right", "right", "right", "right", "left", "left", "left", "left", "left" };
    [SerializeField] private List<TableRow> officeTable = new List<TableRow>();
    [SerializeField] private List<TableRow> roomTable = new List<TableRow>();
    [SerializeField] private List<TableRow> emptyTable = new List<TableRow>();

    private int[] FirstAngles;
    private int[] SecondAngles;
    private int[] ThirdAngles;
    private int[] ScenesOreder;
    private int[][] AllTheAngles;
    [SerializeField] private int[] current = { 0, 0 };
    private string[] ScenesNames = { "Office", "Room", "Empty" };

    [SerializeField] private GameObject numberRandomizer;
    [SerializeField] private GameObject taskPanel;
    [SerializeField] private TextMeshProUGUI taskText;
    [SerializeField] private Button taskButton;
    [SerializeField] private GameObject stopPanel;
    [SerializeField] private Button stopButton;

    private bool stopButtonPressed = false;

    public event EventHandler<CurrentEventArgs> OnCurentScenes;
    public class CurrentEventArgs : EventArgs
    {
        public int CurrentSceneNum { get; }


        public CurrentEventArgs(int SceneNum)
        {
            CurrentSceneNum = SceneNum;
        }
    }


    [System.Serializable] 
    public class TableRow
    {
        public int Taskorder;
        public int instruction;
        public string direction;
        public int result;
        public int error;
        public int startTime;
        public int endTime;
        public int actualTime;

        // Constructor not needed for serialization; provided for manual instantiation if necessary
        public TableRow(int order, int instruction, string direction,
                        int result, int error, int startTime, int endTime, int actualTime)
        {
            this.Taskorder = order;
            this.instruction = instruction;
            this.direction = direction;
            this.result = result;
            this.error = error;
            this.startTime = startTime;
            this.endTime = endTime;
            this.actualTime = actualTime;
        }
    }

    void Start()
    {
        RandomNubers randomNubers = numberRandomizer.GetComponent<RandomNubers>();
        randomNubers.OnGenerateAngles += ComponentAnglesOrder;
        randomNubers.OnGenerateScenes += ComponentAScenesOrder;
        taskButton.onClick.AddListener(() => HideTaskMessage()); // להפוך לאיוונט
        stopButton.onClick.AddListener(() => { stopButtonPressed = true; }); // Modified to set the flag to true when pressed
    }

    private void ComponentAnglesOrder(object sender, RandomNubers.AngelsEventArgs e)
    {
        FirstAngles = e.FirstAngles;
        SecondAngles = e.SecondAngles;
        ThirdAngles = e.ThirdAngles;
        AllTheAngles = new int[][] { FirstAngles, SecondAngles, ThirdAngles };

        CreateTables();
    }


    private void ComponentAScenesOrder(object sender, RandomNubers.ScenesEventArgs s)
    {
        ScenesOreder = s.Order;
    }

    public void CreateTables()
    {
        for (int j = 0; j < ScenesOreder.Length; j++)
        {
            current[0] = ScenesOreder[j];
            OnCurentScenes?.Invoke(this, new CurrentEventArgs(ScenesOreder[j])); // לשלוח למחליט על הסצינות

            if (ScenesOreder[j] == 1)
            {
                PopulateTable(officeTable, "Office", AllTheAngles[j]);
            }
            if (ScenesOreder[j] == 2)
            {
                PopulateTable(roomTable, "Room", AllTheAngles[j]);
            }
            if (ScenesOreder[j] == 3)
            {
                PopulateTable(emptyTable, "Empty", AllTheAngles[j]);
            }

        }
    }


    private void UpdateCurrentAndShowMessage(int angelValue, string directionValue)
    {
        //taskPanel.SetActive(true);
        //taskText.text = "Please turn " + angelValue.ToString() + " degrees to the " + directionValue.ToString();
        //taskButton.onClick.AddListener(ShowStopPanel);  
        StartCoroutine(ShowMessageAndAwaitStop(angelValue, directionValue)); // Now starts a coroutine

    }

    IEnumerator ShowMessageAndAwaitStop(int angelValue, string directionValue)
    {
        taskPanel.SetActive(true);
        taskText.text = "Please turn " + angelValue.ToString() + " degrees to the " + directionValue;
        stopButtonPressed = false; // Reset the flag to false
        yield return new WaitUntil(() => stopButtonPressed); // Wait until the stopButton is pressed
        HideTaskMessage();
    }

    private void PopulateTable(List<TableRow> table, string sceneName, int[] array)
    {
        
        for (int i = 0; i < angels.Length; i++)

        {
            int order = array[i % array.Length];
            int angelValue = angels[i % angels.Length];
            string directionValue = direction[i % direction.Length];

            int CurrentAngel = angels[order];
            string Currentdirection = direction[order];

            UpdateCurrentAndShowMessage(CurrentAngel, Currentdirection);

            table.Add(new TableRow(
        order: order,
        instruction: angelValue,
        direction: directionValue,
        result: 0,
        error: 0,
        startTime: 0,
        endTime: 0,
        actualTime: 0 ));
                
        }
    }

    private void HideTaskMessage()
    {
        taskPanel.SetActive(false);
    }

    private void ShowStopPanel()
    {
        stopPanel.SetActive(true);
    }

}