using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeadLocation : MonoBehaviour
{
    [SerializeField] private GameObject data;
    public event EventHandler OnSaveStartLoction;
    public event EventHandler<PoseEventArgs> OnPoseCaptured;
    public event EventHandler OnSaveEndLoction;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float startTime;
    private float endTime;
    private float realAngleInXZPlane;
    private float angleError;
    [SerializeField] private int angle;

    public class PoseEventArgs : EventArgs
    {
        public Vector3 StartPosition { get; private set; }
        public Vector3 EndPosition { get; private set; }
        public float ElapsedTime { get; private set; }
        public float ErrorTime { get; private set; }


        public PoseEventArgs(Vector3 startPosition, Vector3 endPosition, float elapsedTime,float angleError)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            ElapsedTime = elapsedTime;
            ErrorTime = angleError;
        }
    }
    void Start()
    {
        Data dataInfo = data.GetComponent<Data>();
        if (dataInfo != null)
        {
            dataInfo.OnTaskButtonClicked += TakeStartPose;
            dataInfo.OnStopButtonClicked += TakeEndPose;
            dataInfo.OnCurentAngle += GetCurentAngle; ;
        }
    }

    private void TakeStartPose(object sender, EventArgs e)
    {
        startPosition = Camera.main.transform.position;
        startTime = Time.time;
        OnSaveStartLoction?.Invoke(this, EventArgs.Empty);
    }

    private void TakeEndPose(object sender, EventArgs e)
    {
        endPosition = Camera.main.transform.position;
        endTime = Time.time;
        float elapsedTime = endTime - startTime;
        OnSaveEndLoction?.Invoke(this, EventArgs.Empty);

        Vector3 startDirection = new Vector3(startPosition.x, 0, startPosition.z);
        Vector3 endDirection = new Vector3(endPosition.x, 0, endPosition.z);
        Vector3 movementDirection = (endDirection - startDirection).normalized; // Direction of movement in XZ plane
        Vector3 referenceDirection = new Vector3(0, 0, 1);
        realAngleInXZPlane = Vector3.SignedAngle(referenceDirection, movementDirection, Vector3.up);

        if (realAngleInXZPlane < 0)
        {
            realAngleInXZPlane += 360;
        }
        angleError = Mathf.Abs(angle - realAngleInXZPlane);

        OnPoseCaptured?.Invoke(this, new PoseEventArgs(startPosition, endPosition, elapsedTime, angleError));
    }

    private void GetCurentAngle(object sender, Data.AngleEventArgs en)
    {
        angle = en.Angle;

    }
}
