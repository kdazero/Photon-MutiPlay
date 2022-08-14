using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCount : MonoBehaviour
{
    public Text UI_Count;
    [SerializeField]private int PlayerInGame = 0;
   
    void Update()
    {
        UI_Count.text = "" + GameObject.FindGameObjectsWithTag("player").Length;
    }
}
