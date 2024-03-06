using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BNG {
public class DestroyOnSceneSwitch : MonoBehaviour {
    public void OnDetached(Grabbable grabbableThatWasDetached) {
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(grabbableThatWasDetached.gameObject, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
}
}