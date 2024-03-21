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

    private int currentAngleIndex = 0;
    private int currentSceneIndex = 0;
    private bool waitingForStartPosition = true;
    private bool waitingForEndPosition = true;

    public event EventHandler <CurrentEventArgs> OnCurentScenes;
    public class CurrentEventArgs : EventArgs
    {
        public int CurrentSceneNum { get; }


        public CurrentEventArgs(int SceneNum)
        {
            CurrentSceneNum = SceneNum;
        }
    }

    public event EventHandler<AngleEventArgs> OnCurentAngle;
    public class AngleEventArgs : EventArgs
    {
        public int Angle { get; }


        public AngleEventArgs(int angleIn360)
        {
            Angle = angleIn360;
        }
    }

    public event EventHandler OnTaskButtonClicked;
    public event EventHandler OnStopButtonClicked;

    [System.Serializable] 
    public class TableRow
    {
        public int Taskorder;
        public int instruction;
        public string direction;
        public int angle360;
        public int startPosition;
        public int endPosition;
        public int result;
        public int error;
        public int startTime;
        public int endTime;
        public int actualTime;

        public TableRow(int order, int instruction, string direction, int angle360, int startPosition, int endPosition,
                        int result, int error, int startTime, int endTime, int actualTime)
        {
            this.Taskorder = order;
            this.instruction = instruction;
            this.direction = direction;
            this.angle360 = angle360;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
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
        stopPanel.SetActive(false);
        taskPanel.SetActive(false);
        taskButton.onClick.AddListener(TaskButtonClicked);
        stopButton.onClick.AddListener(StopButtonClicked);

        waitingForStartPosition = true;
    }

    private void ComponentAnglesOrder(object sender, RandomNubers.AngelsEventArgs e)
    {
        FirstAngles = e.FirstAngles;
        SecondAngles = e.SecondAngles;
        ThirdAngles = e.ThirdAngles;
        AllTheAngles = new int[][] { FirstAngles, SecondAngles, ThirdAngles };
    }


    private void ComponentAScenesOrder(object sender, RandomNubers.ScenesEventArgs s)
    {
        ScenesOreder = s.Order;
        CreateTables();

    }

    public void CreateTables()
    {
        current[0] = ScenesOreder[currentSceneIndex];
        OnCurentScenes?.Invoke(this, new CurrentEventArgs(ScenesOreder[currentSceneIndex]));

        if (ScenesOreder[currentSceneIndex] == 1)
        {
            PopulateTable(officeTable, "Office", AllTheAngles[currentSceneIndex]);
        }
        if (ScenesOreder[currentSceneIndex] == 2)
        {
            PopulateTable(roomTable, "Room", AllTheAngles[currentSceneIndex]);
        }
        if (ScenesOreder[currentSceneIndex] == 3)
        {
            PopulateTable(emptyTable, "Empty", AllTheAngles[currentSceneIndex]);
        }
    }

    void BeginNextProcess()
    {
        if (currentAngleIndex >= angels.Length)
        {
            if (currentSceneIndex >= ScenesOreder.Length)
            {
                Debug.Log("Thank you! We finished");
                return;
            }

            else
            {
               Debug.Log("Lets Start The Next Level");
               currentAngleIndex = 0;
               currentSceneIndex++;
               CreateTables();
                return;
            }
        }

        waitingForStartPosition = true;
    } 

    private void UpdateCurrentAndShowMessage(int angelValue, string directionValue)
    {
        taskPanel.SetActive(true);
        taskText.text = "Please turn " + angelValue.ToString() + " degrees to the " + directionValue.ToString();
    }

    private void PopulateTable(List<TableRow> table, string sceneName, int[] anglesOrder)
    {
        current[1] = anglesOrder[currentAngleIndex];

        int CurrentAngel = angels[current[1]];
        string Currentdirection = direction[current[1]];
        int angle360 = CurrentAngel;

        if ( direction[current[1]] == "left")
        {
            angle360 = angle360 + 180;
        }

        OnTaskButtonClicked?.Invoke(this, new AngleEventArgs(angle360));
        UpdateCurrentAndShowMessage(CurrentAngel, Currentdirection);

        table.Add(new TableRow(
    order: current[1],
    instruction: CurrentAngel,
    direction: Currentdirection,
    angle360: angle360,
    startPosition: 0,
    endPosition: 0,
    result: 0,
    error: 0,
    startTime: 0,
    endTime: 0,
    actualTime: 0));
    }


    private void TaskButtonClicked()
    {
        if (waitingForStartPosition)
        {
            OnTaskButtonClicked?.Invoke(this, EventArgs.Empty);
            taskPanel.SetActive(false);
            // לקבל אישור מיקום מהראש 
            waitingForStartPosition = false;
            stopPanel.SetActive(true);
            waitingForEndPosition = true;
        }
    }

    private void StopButtonClicked()
    {
        if (waitingForEndPosition)
        {
            OnStopButtonClicked?.Invoke(this, EventArgs.Empty);
            stopPanel.SetActive(false);
            // לקבל אישור מיקום מהראש
            // לקבל את כל הנתונים- צריך לבחור אפה להשלים
            waitingForEndPosition = false;
            // רק אחרי עדכון כל הטבלה
            currentAngleIndex++;
            BeginNextProcess(); 
        }
    }

}