using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Angles;
using static RandomNubers;

public class Angles : MonoBehaviour
{
    public event EventHandler<OrderEventArgs> OnGenerateAngles;
    [SerializeField] private int[] angels = { 10, 27, 43, 96, 108, 16, 59, 103, 122, 150};
    [SerializeField] private string[] direction = { "right", "right", "right", "right", "right", "left", "left", "left", "left", "left" };

    public class OrderEventArgs : EventArgs
    {
        public int[] Angles { get; }
        public string[] Directions { get; }

        public OrderEventArgs(int[] angels, string[] direction)
        {
            Angles = angels;
            Directions = direction;
        }
    }
    public void GenerrateOrder()
    {
        ShuffleArray(angels, direction);
        OnGenerateAngles?.Invoke(this, new OrderEventArgs(angels, direction));
 
    }

    private void ShuffleArray(int[] arrary1, string[] arrary2)
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < arrary1.Length; i++)
        {
            int randIndex = rand.Next(i, arrary1.Length);

            int tempArray1 = arrary1[i];
            string tempArray2 = arrary2[i];

            arrary1[i] = arrary1[randIndex];
            arrary2[i] = arrary2[randIndex];

            arrary1[randIndex] = tempArray1;
            arrary2[randIndex] = tempArray2;
        }

    }
}

