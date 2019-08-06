using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    public string pieceName;
    public const int pieceID = 0;  
    public Sprite ICON;
    public const int maxHealth = 500;
    public int pieceCost = 1;
    public int health = maxHealth;
    public int mana;
    public int armor;
    public int magicalResistance;
    public int attackDamage;
    public int baseAttackSpeed;
    public int baseAttackRange;
    public string firstRace;
    public string secondaryRace;
    public string pieceClass;
}
