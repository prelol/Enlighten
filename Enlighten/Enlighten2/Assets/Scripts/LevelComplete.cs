using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] float nextLevelDelay = 0.5f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.CompareTag("Player"))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            //print("Current Scene Name: " + currentSceneName);
            //print("Current Level Number: " + currentSceneName.Remove(0, 5));
            int nextSceneNum = int.Parse(currentSceneName.Remove(0, 5));
            nextSceneNum++;
            string nextScene = "Level" + nextSceneNum;
            print(nextScene);
            if(Application.CanStreamedLevelBeLoaded(nextScene))
            {
                StartCoroutine(NextLevel(nextLevelDelay, nextScene));
                //gameObject.SetActive(false);
            } else
            {
                StartCoroutine(NextLevel(nextLevelDelay, "Menu"));
                //gameObject.SetActive(false);
            }
        }
    }



    IEnumerator NextLevel(float delay, string sceneToLoad)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
}
