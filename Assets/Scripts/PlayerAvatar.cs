using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAvatar : MonoBehaviour
{
    public bool displayingTurn;
    public Color originalColor;
    public int owner;
    public TextMeshProUGUI activePlayerText;
    public Image initiativeMarker;
    public TextMeshProUGUI playerTypeText;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnAvatarClicked);
        }
    }

    private void OnAvatarClicked()
    {
        PlayerController.Instance.TogglePlayerType(owner);
    }

    public void UpdatePlayerTypeText(PlayerType playerType)
    {
        playerTypeText.text = playerType.ToString();
    }

    // Start is called before the first frame update

    public void DisplayTurn(bool active) {
        if (active) {
            displayingTurn = true;
            activePlayerText.gameObject.SetActive(true);
        }
        else {
            displayingTurn = false;
            activePlayerText.gameObject.SetActive(false);
            GetComponent<Image>().color = originalColor;
        }
    }

    public void DisplayInitiative(int initiative) {
        if (initiative == 0) {
            initiativeMarker.gameObject.SetActive(true);
        }
        else {
            initiativeMarker.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (displayingTurn) {
            GetComponent<Image>().color = new Color(originalColor.r,originalColor.g,originalColor.b, (float)Math.Sin(Time.time*4)/4+.75f);
        }
    }

    public void SetOriginalColor(Color pieceColor) {
        originalColor = pieceColor;
        GetComponent<Image>().color = originalColor;
    }
}
