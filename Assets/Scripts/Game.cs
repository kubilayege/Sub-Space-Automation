using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject matchPrefab;

    public void PlayGame()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        GameObject match = GameObject.Instantiate(matchPrefab, Vector3.zero, Quaternion.identity, this.transform);
    }

}
