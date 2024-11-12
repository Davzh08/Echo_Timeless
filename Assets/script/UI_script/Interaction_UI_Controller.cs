using UnityEngine;

public class ClockInteractionUI : MonoBehaviour
{
    public GameObject interactionUI; // UI 提示的 GameObject
    private bool isPlayerNear = false;

    void Start()
    {
        interactionUI.SetActive(false); // 初始隐藏 UI
    }

    void Update()
    {
        // 检测玩家是否按下互动键，并且在触发区内
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E)) // 假设E键是互动键
        {
            // 执行互动逻辑，比如切换场景或播放动画
        }
    }

    // 玩家进入触发区时显示 UI
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 确保碰撞体是玩家
        {
            isPlayerNear = true;
            interactionUI.SetActive(true);
        }
    }

    // 玩家离开触发区时隐藏 UI
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactionUI.SetActive(false);
        }
    }
}