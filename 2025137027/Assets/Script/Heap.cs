using System;
using System.Collections.Generic;


public class BimaryMaxHeap
{
    List<int> elements;

    private List<int> arr = new List<int>();

    public void Add(int data)
    {
        arr.Add(data);
        int i = arr.Count - 1;

        while (i >= 0)
        {
            int parent = (i - 1) / 2;

            if (arr[i] >= arr[parent])
            {
                /*int tmp = arr[i];
                arr[i] = arr[parent];
                arr[parent] = tmp;
                */

                if (arr[i].CompareTo(arr[parent]) > 0)
                {
                    (arr[i], arr[parent]) = (arr[parent], arr[i]);
                }

                i = parent;
            }
            else
            {
                break;
            }

        }
    }

    public int Remove()
    {
        if (arr.Count == 0)
        {
            throw new ApplicationException();
        }

        int data = arr[0];

        arr[0] = arr.Count - 1;

        arr.RemoveAt(arr.Count - 1);

        int i = 0;

        int last = arr.Count - 1;
        while (i < last)
        {
            int child = 2 * i + 1;

            if (child < last && arr[child] >= arr[child + 1])
            {
                child++;
            }

            if (child > last || arr[i] >= arr[child])
            {
                break;
            }

            if (arr[i].CompareTo(arr[child]) > 0)
            {
                (arr[i], arr[child]) = (arr[child], arr[i]);
            }

            i = child;

        }
        


        return data;

    }

}