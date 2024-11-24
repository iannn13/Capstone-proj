using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScene : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private GameObject Credits;  

    private void Start()
    {
        
        Button buttonComponent = Credits.GetComponent<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(OnDoorButtonClicked);
        }
        else
        {
            Debug.LogError("HowToPlay GameObject does not have a Button component!");
        }
    }

    private void OnDoorButtonClicked()
    {
        Debug.Log("Door button clicked");
        StartCoroutine(LoadSceneWithoutFade());
    }

    private IEnumerator LoadSceneWithoutFade()
    {
       
        yield return new WaitForSeconds(0.5f);

        
        SceneManager.LoadSceneAsync(44); 
    }
}
