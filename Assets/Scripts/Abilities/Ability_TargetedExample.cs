using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_TargetedExample : MonoBehaviour
{
    GameObject FindTartget(Pieces[] Enemies, Pieces currentTarget)
    {

        GameObject abilityTarget = currentTarget.gameObject;
        foreach(Pieces enemy in Enemies)
        {
            if(currentTarget.health < enemy.health)
            {
                abilityTarget = enemy.gameObject;
            }
        }

        return abilityTarget;
    }
    public void UseAbility(Pieces[] Enemies, Pieces currentTarget)
    {
        GameObject target = FindTartget(Enemies, currentTarget);

        target.GetComponent<Pieces>().health += -200;
    }

    
}
