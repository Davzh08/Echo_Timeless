using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockInteraction1 : MonoBehaviour
{
    private bool isPlayerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered the range.");  // 调试信息
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player left the range.");  // 调试信息
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Clock clicked, loading scene...");  // 调试信息
            LoadScene();
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("ClockShop");
    }
}