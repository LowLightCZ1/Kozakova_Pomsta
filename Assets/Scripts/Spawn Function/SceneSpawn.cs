using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawn : MonoBehaviour
{
    public static string TargetSpawn;

    public void LoadNweScene(string sceneName, string targetSpawnName)
    {
        TargetSpawn = targetSpawnName;
        SceneManager.LoadScene(sceneName);
    }


  

}
