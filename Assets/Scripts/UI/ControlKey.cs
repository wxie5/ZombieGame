using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlKey : MonoBehaviour, IPointerClickHandler
{
    private Image image;
    [SerializeField] private Text keyCodeText;
    [SerializeField] private Color enableColor;
    [SerializeField] private Color UnableColor;

    public string KeyName;
    private bool isShowing;

    public bool IsShowing 
    { 
        get => isShowing;
        set
        {
            isShowing = value;
            if(isShowing)
            {
                image.color = enableColor;
            }
            else
            {
                image.color = UnableColor;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        KeyCodeSettingPanel.Instance.ShowKey(this);
    }


    void Start()
    {
        image = this.GetComponent<Image>();
        UpdateKeyTextInUI();
    }

    public void UpdateKeyCodeText(string text)
    {
        keyCodeText.text = text;
    }
     
    public void UpdateKeyTextInUI()
    {
        switch (KeyName)
        {
            case "A_Up":
                keyCodeText.text = InputManager.Instance.get_PlayerA_Move_Up_Key().ToString();
                break;
            case "A_Down":
                keyCodeText.text = InputManager.Instance.get_PlayerA_Move_Down_Key().ToString();
                break;
            case "A_Left":
                keyCodeText.text = InputManager.Instance.get_PlayerA_Move_Left_Key().ToString();
                break;
            case "A_Right":
                keyCodeText.text = InputManager.Instance.get_PlayerA_Move_Right_Key().ToString();
                break;
            case "A_Shot":
                keyCodeText.text = InputManager.Instance.get_PlayerA_Attack_Key().ToString();
                break;
            case "A_Switch":
                keyCodeText.text = InputManager.Instance.get_PlayerA_Switch_Key().ToString();
                break;
            case "A_Reload":
                keyCodeText.text = InputManager.Instance.get_PlayerA_Reload_Key().ToString();
                break;
            case "A_Pick":
                keyCodeText.text = InputManager.Instance.get_PlayerA_Pick_Key().ToString();
                break;


            case "B_Up":
                keyCodeText.text = InputManager.Instance.get_PlayerB_Move_Up_Key().ToString();
                break;
            case "B_Down":
                keyCodeText.text = InputManager.Instance.get_PlayerB_Move_Down_Key().ToString();
                break;
            case "B_Left":
                keyCodeText.text = InputManager.Instance.get_PlayerB_Move_Left_Key().ToString();
                break;
            case "B_Right":
                keyCodeText.text = InputManager.Instance.get_PlayerB_Move_Right_Key().ToString();
                break;
            case "B_Shot":
                keyCodeText.text = InputManager.Instance.get_PlayerB_Attack_Key().ToString();
                break;
            case "B_Switch":
                keyCodeText.text = InputManager.Instance.get_PlayerB_Switch_Key().ToString();
                break;
            case "B_Reload":
                keyCodeText.text = InputManager.Instance.get_PlayerB_Reload_Key().ToString();
                break;
            case "B_Pick":
                keyCodeText.text = InputManager.Instance.get_PlayerB_Pick_Key().ToString();
                break;
        }
    }
}
