using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampelScript_B : MonoBehaviour
{

   // private int enemyDistance = 0;
    private int EnemyCount =  10;
    private string[] Enemies = new string[5];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            //EmemySearch();
            //EnemyDestruktion();
            //EnemyScan();
            EnemyRoster();
        }
    }
    void EnemyScan()
    {
        bool isAlive = false;
        do
        {
            print("scanning for Ememies");
        }
        while (isAlive == true);
    }
    /*void EmemySearch()

    {
        for (int i = 0; i < 5; i++)
            {
            enemyDistance = Random.Range(1, 10);

            if (enemyDistance >= 8) 
            {
                print("Der er en fjende et stykke fra dig ");
            }
            if (enemyDistance >= 4 && enemyDistance <= 7) 
            {
                print(" Der er en fjende i nærheden ");
            }
            if (enemyDistance < 4) 
            {
                print(" Der er en fjende meget tæt på dig ");
            }
        }
        */
      void EnemyDestruktion()
    {
        while(EnemyCount > 0)
        {
            print(" der er en fjinde i nærhedern! Lad os nakke den");
            EnemyCount--;
        }
    }   
    

    void EnemyRoster()
    {
        Enemies[0] = "Orc";
        Enemies[1] = "Dragon"; 
        Enemies[2] = "Dragon";
        Enemies[3] = "Orc";
        Enemies[4] = "Dragon";

        foreach(string enemy in Enemies)
        {
            print(enemy);
        }
    }
 }
