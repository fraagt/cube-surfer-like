using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Roads : MonoBehaviour
{
    private List<RoadSection> sections;

    public RoadSection CurrentRoad
    {
        get => sections[currentRoadIndex];
    }
    private int currentRoadIndex;

    private void Awake()
    {
        sections =
            transform.GetComponentsInChildren<RoadSection>().ToList();
        currentRoadIndex = 0;
    }

    public bool SetNextSection()
    {
        if (currentRoadIndex == (sections.Count - 1))
        {
            return false;
        }

        currentRoadIndex++;

        return true;
    }
}
