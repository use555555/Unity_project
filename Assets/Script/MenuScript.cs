using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine.UI;
public class MenuScript : MonoBehaviour
{
    public GameObject menuPanel;
    public InputField inputField;
    public InputField playerField;
    public int playerNumber = 1;
    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        playerField.text = playerNumber.ToString();
    }
    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkManager.ConnectionApprovedDelegate callback)
    {
        bool approve = false;
        string password = System.Text.Encoding.ASCII.GetString(connectionData);
        if (password == "mygame")
        {
            approve = true;
        }
        callback(true, null, approve, new Vector3(0, 10, 0), Quaternion.identity);
    }
    public void Host()
    {
        GameController.maxPlayer = playerNumber;
        NetworkManager.Singleton.StartHost();
        menuPanel.SetActive(false);
    }
    public void Join()
    {
        if (inputField.text.Length <= 0)
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = "127.0.0.1";
        }
        else
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = inputField.text;
        }
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("mygame");
        NetworkManager.Singleton.StartClient();
        menuPanel.SetActive(false);
    }

    public void Plus()
    {
        if (playerNumber +1 <= 4)
        {
            playerNumber = playerNumber + 1;
        }    
        playerField.text = playerNumber.ToString();
    }
    public void Minus()
    {
        if (playerNumber - 1 >= 1)
        {
            playerNumber = playerNumber - 1;
        }
        playerField.text = playerNumber.ToString();
    }
}