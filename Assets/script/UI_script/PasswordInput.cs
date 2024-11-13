using UnityEngine;
using UnityEngine.UI; // 如果使用的是Text
// using TMPro; // 如果使用的是TextMeshPro

public class PasswordInput : MonoBehaviour
{
    public Text passwordDisplay; // 或者使用 TextMeshProUGUI passwordDisplay;
    private string currentPassword = ""; // 存储当前输入的密码
    private const int maxDigits = 4; // 密码的最大长度

    void Start()
    {
        UpdatePasswordDisplay(); // 初始化显示
    }

    void Update()
    {
        // 检查玩家是否按了数字键
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()) && currentPassword.Length < maxDigits)
            {
                currentPassword += i.ToString(); // 添加数字到密码
                UpdatePasswordDisplay(); // 更新显示

                // 检查密码是否已满4位
                if (currentPassword.Length == maxDigits)
                {
                    CheckPassword(); // 在密码输入完成后验证密码
                }
            }
        }

        // 允许玩家按退格键来删除最后一个数字
        if (Input.GetKeyDown(KeyCode.Backspace) && currentPassword.Length > 0)
        {
            currentPassword = currentPassword.Substring(0, currentPassword.Length - 1);
            UpdatePasswordDisplay();
        }
    }

    // 更新UI显示密码
    private void UpdatePasswordDisplay()
    {
        string displayText = "";

        for (int i = 0; i < maxDigits; i++)
        {
            if (i < currentPassword.Length)
            {
                displayText += currentPassword[i] + " ";
            }
            else
            {
                displayText += "_ ";
            }
        }

        passwordDisplay.text = displayText.Trim();
    }

    // 密码输入完成后检查密码
    private void CheckPassword()
    {
        if (currentPassword == "1234") // 替换为正确的密码
        {
            Debug.Log("Password Correct!");
            // 这里可以触发打开门、显示成功UI等操作
        }
        else
        {
            Debug.Log("Password Incorrect!");
            ResetPassword(); // 如果密码错误，重置密码输入
        }
    }

    // 重置密码输入
    private void ResetPassword()
    {
        currentPassword = "";
        UpdatePasswordDisplay();
    }
}