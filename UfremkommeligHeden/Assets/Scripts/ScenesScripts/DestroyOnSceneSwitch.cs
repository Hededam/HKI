using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BNG {
    public class DestroyOnSceneSwitch : MonoBehaviour {
        public void OnDetached(Grabbable grabbableThatWasDetached) {
            // Hent den gemte scene fra det andet script (erstat med den faktiske m�de)
            string targetSceneName = SceneLoaderHede.lastLoadedScene; // Erstat "OtherScript" med navnet p� det script, hvor v�rdien er gemt

            // Find den �nskede scene
            Scene targetScene = SceneManager.GetSceneByName(targetSceneName);

            if (targetScene.IsValid()) {
                // Flyt GameObject til den �nskede scene
                SceneManager.MoveGameObjectToScene(grabbableThatWasDetached.gameObject, targetScene);
            } else {
                Debug.LogError($"Scenen med navnet '{targetSceneName}' blev ikke fundet.");
            }
        }
    }
}
