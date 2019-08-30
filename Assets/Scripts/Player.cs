using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int experience=0;
    public int requiredExp=1;
    public int level=0;
    public int health = 100;
    public Dictionary<int, Tuple<int,int>> levelExperienceTable = new Dictionary<int, Tuple<int, int>>(); 

    void Awake()
    {
        DefineLevels();
    }

    public void IncreaseExp()
    {
        experience += 1;
        CalculatePlayerLevel();
    }

    public void TakeDamage(int value)
    {
        health -= value;
    }

    private void CalculatePlayerLevel()
    {
        Tuple<int, int> outTupple;
        if (levelExperienceTable.ContainsKey(experience))
        {
            levelExperienceTable.TryGetValue(experience, out outTupple);
            level =  outTupple.Item1;
            requiredExp = outTupple.Item2;
        }
    }

    private void DefineLevels()
    {
        levelExperienceTable.Add(1, new Tuple<int, int>(1, 1));
        levelExperienceTable.Add(2, new Tuple<int, int>(2, 1));
        levelExperienceTable.Add(3, new Tuple<int, int>(3, 2));
        levelExperienceTable.Add(5, new Tuple<int, int>(4, 4));
        levelExperienceTable.Add(9, new Tuple<int, int>(5, 8));
        levelExperienceTable.Add(13, new Tuple<int, int>(6, 16));
        levelExperienceTable.Add(21, new Tuple<int, int>(7, 24));
        levelExperienceTable.Add(37, new Tuple<int, int>(8, 32));
        levelExperienceTable.Add(61, new Tuple<int, int>(9, 40));
        levelExperienceTable.Add(93, new Tuple<int, int>(10, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
