using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class StartScreen : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private UnityEngine.InputSystem.PlayerInput playerInput;
    [SerializeField] private CinemachineInputAxisController cinemachineController;

    private void Start()
    {
       ShowScreen();
    }

    public void ShowScreen()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
        cinemachineController.enabled = false;
        
        playerInput.enabled = false;
        startScreen.SetActive(true);
    }

    public void HideScreen()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        cinemachineController.enabled = true;
        
        playerInput.enabled = true;
        startScreen.SetActive(false);
    }
}
