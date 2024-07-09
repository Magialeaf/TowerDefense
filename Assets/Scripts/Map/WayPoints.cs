using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WayPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> wayPointList;
    public static WayPoints Instance { get; private set; }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Init();
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        Transform[] temp = GetComponentsInChildren<Transform>();
        wayPointList = new List<Transform>(temp);
        wayPointList.RemoveAt(0);
    }

    public int GetLength() => wayPointList.Count;
    public Vector3 GetWayPoint(int index) => wayPointList[index].position;
}
