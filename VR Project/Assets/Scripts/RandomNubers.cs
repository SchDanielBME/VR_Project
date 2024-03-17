using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNubers : MonoBehaviour
{
    public event EventHandler<OrderEventArgs> OnGenerateOrder;

    [SerializeField] private int[] order = { 1, 2, 3 };

    public class OrderEventArgs : EventArgs
    {
        public int[] Order { get; private set; }

        public OrderEventArgs(int[] order)
        {
            Order = order;
        }
    }
     public void GenerrateOrder()
    {
        ShuffleArry(order);
        OnGenerateOrder?.Invoke(this, new OrderEventArgs(order));
    }

    private void ShuffleArry(int[] arrary)
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < arrary.Length; i++)
        {
            int randIndex = rand.Next(i, arrary.Length);
            int temp = arrary[i];
            arrary[i] = arrary[randIndex];
            arrary[randIndex] = temp;
        }

    }
}
