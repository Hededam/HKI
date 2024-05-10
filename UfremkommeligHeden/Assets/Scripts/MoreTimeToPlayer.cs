using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreTimeToPlayer : MonoBehaviour
{

      public void GivMigTid()
      {
      GameObject Gamestuff = GameObject.FindWithTag("Gamestuff");

                if (Gamestuff != null)
                {
                    // Tjek om objektet har PlayerXp scriptet tilknyttet
                    PlayerXp playerXp = Gamestuff.GetComponent<PlayerXp>();

                    if (playerXp != null)
                    {
                    playerXp.PlayTimeLeft += 300; // Tilføj 30 sekunder til spilletiden
                    }
                }
      }
    }

