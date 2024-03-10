using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BNG {
    public class DestroyOnSceneSwitch : MonoBehaviour {
        public void OnDetached(Grabbable grabbableThatWasDetached) {
            // Hent den gemte scene fra det andet script (erstat med den faktiske måde)
            string targetSceneName = SceneLoaderHede.lastLoadedScene; // Erstat "OtherScript" med navnet på det script, hvor værdien er gemt

            // Find den ønskede scene
            Scene targetScene = SceneManager.GetSceneByName(targetSceneName);

            if (targetScene.IsValid()) {
                // Flyt GameObject til den ønskede scene
                SceneManager.MoveGameObjectToScene(grabbableThatWasDetached.gameObject, targetScene);
            } else {
                Debug.LogError($"Scenen med navnet '{targetSceneName}' blev ikke fundet.");
            }
        }
    }
}
