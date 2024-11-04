using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Animator fadeInAnimator;

    private IEnumerator Start()
    {
        Application.targetFrameRate = 60;

        yield return new WaitForSeconds(1.3f);

        fadeInAnimator.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (!settingsPanel.activeInHierarchy) OpenSettingsPanel();
            else CloseSettingsPanel();
        }
    }

    [SerializeField]
    GameObject settingsPanel;

    public void OpenSettingsPanel()
    {
        Cursor.lockState = CursorLockMode.None;
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        settingsPanel.SetActive(false);
    }
}
