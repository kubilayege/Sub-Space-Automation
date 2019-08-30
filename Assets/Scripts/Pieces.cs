using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    public string pieceName;
    public const int pieceID = 0;  
    public Sprite ICON;
    public int maxHealth = 500;
    public int pieceCost = 1;
    public int health;
    public int mana = 0;
    public int maxiumumMana = 100;
    public int armor;
    public int magicalResistance;
    public int attackDamage;
    public float baseAttackSpeed;
    public int baseAttackRange;
    public int star;
    public string firstRace;
    public string secondaryRace;
    public string pieceClass;

    private void Awake()
    {
        health = maxHealth;
    }

    public void ResetPiece()
    {
        health = maxHealth;
        mana = 0;
    }

    public void UpdateHealth(int value)
    {
        health += value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if(health <0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        Board board = transform.parent.parent.parent.GetComponent<Board>();
        if (board.enemyBoardList.Contains(this.gameObject))
        {
            board.enemyUnitCount -= 1;
        }

        this.transform.parent = this.transform.parent.parent.parent.GetChild(4).transform; // Graveyard
        transform.localPosition = Vector3.zero;
    }
}
