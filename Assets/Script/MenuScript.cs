using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using UnityEngine.UI;
public class MenuScript : NetworkBehaviour
{
    public GameObject menuPanel;
    public GameObject UiPanel;
    public GameObject Weapon;
    public InputField inputField;
    public InputField playerField;
    public GameObject upgradePanel;
    public Button dmg;
    public Button spd;
    public Text dmg_txt;
    public Text spd_txt;
    public Text Point_txt;
    public Text Money_txt;

    public Text dmg_val_txt;
    public Text dmg_prc_txt;
    public Text spd_val_txt;
    public Text spd_prc_txt;

    public int playerNumber = 1;
    public int Me = -1;
    public GameObject[] Player;
    public int Price;

    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        playerField.text = playerNumber.ToString();
        UiPanel.SetActive(false);
        Weapon.SetActive(false);
        upgradePanel.SetActive(false);
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
        Player = GameObject.FindGameObjectsWithTag("Player");
        Me = Player.Length - 1;
        menuPanel.SetActive(false);
        Weapon.SetActive(true);
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
        StartCoroutine(Selection());
    }

    IEnumerator Selection()
    {
        yield return new WaitForSeconds(1f);
        Player = GameObject.FindGameObjectsWithTag("Player");
        Me = Player.Length - 1;
        menuPanel.SetActive(false);
        Weapon.SetActive(true);
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

    public void Rifle()
    {
        Player[Me].GetComponent<Equip>().selected = 0;
        UiPanel.SetActive(true);
        Weapon.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pistol()
    {
        Player[Me].GetComponent<Equip>().selected = 1;
        UiPanel.SetActive(true);
        Weapon.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Sniper()
    {
        Player[Me].GetComponent<Equip>().selected = 2;
        UiPanel.SetActive(true);
        Weapon.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Damage()
    {
        Price = Mathf.CeilToInt((float)((Player[Me].GetComponentInChildren<GunDetail>().dmgmultiplier-0.95) * 10000));
        if (Player[Me].GetComponent<PlayerDetail>().Money.Value >= Price)
        {
            Player[Me].GetComponentInChildren<GunDetail>().dmgmultiplier = Player[Me].GetComponentInChildren<GunDetail>().dmgmultiplier + 0.05f;
            dmg_val_txt.text = (Player[Me].GetComponentInChildren<GunDetail>().damage * Player[Me].GetComponentInChildren<GunDetail>().dmgmultiplier).ToString("F2") + " Hp";
            dmg_prc_txt.text = (Mathf.CeilToInt((float)((Player[Me].GetComponentInChildren<GunDetail>().dmgmultiplier - 0.95) * 10000))).ToString();
            BuyServerRpc(Price);
        }
        if (Player[Me].GetComponentInChildren<GunDetail>().dmgmultiplier >= 2f)
        {
            dmg.interactable = false;
            dmg_txt.text = "Maxed";
        }
    }

    public void Speed()
    {
        Price = Mathf.CeilToInt(((float)(1.05 - Player[Me].GetComponentInChildren<GunDetail>().spdmultiplier) * 20000));
        if (Player[Me].GetComponent<PlayerDetail>().Money.Value >= Price)
        {
            Player[Me].GetComponentInChildren<GunDetail>().spdmultiplier = Player[Me].GetComponentInChildren<GunDetail>().spdmultiplier - 0.05f;
            spd_val_txt.text = ((float)(1 / (Player[Me].GetComponentInChildren<GunDetail>().fireRate * Player[Me].GetComponentInChildren<GunDetail>().spdmultiplier))).ToString("F2") + " rps";
            spd_prc_txt.text = (Mathf.CeilToInt(((float)(1.05 - Player[Me].GetComponentInChildren<GunDetail>().spdmultiplier) * 20000))).ToString();
            BuyServerRpc(Price);
        }
        if (Player[Me].GetComponentInChildren<GunDetail>().spdmultiplier <= 0.5f)
        {
            spd.interactable = false;
            spd_txt.text = "Maxed";
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void BuyServerRpc(int Price)
    {
        BuyClientRpc(Price);
    }

    [ClientRpc]
    public void BuyClientRpc(int Price)
    {
        Player = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in Player)
        {
            player.GetComponent<PlayerDetail>().Money.Value = player.GetComponent<PlayerDetail>().Money.Value - Price;
        }
    }

    public void Update()
    {
        if (Me != -1)
        {
            Point_txt.text = "Point: " + Player[Me].GetComponent<PlayerDetail>().Point.Value.ToString();
            Money_txt.text = "Money: " + Player[Me].GetComponent<PlayerDetail>().Money.Value.ToString() + " $";
            if (Player[Me].GetComponent<Equip>().selected != -1)
            {
                if (Input.GetKeyDown("e"))
                {
                    upgradePanel.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    dmg_val_txt.text = (Player[Me].GetComponentInChildren<GunDetail>().damage* Player[Me].GetComponentInChildren<GunDetail>().dmgmultiplier).ToString("F2") + " Hp";
                    dmg_prc_txt.text = (Mathf.CeilToInt((float)((Player[Me].GetComponentInChildren<GunDetail>().dmgmultiplier - 0.95) * 10000))).ToString();
                    spd_val_txt.text = ((float)(1 / (Player[Me].GetComponentInChildren<GunDetail>().fireRate* Player[Me].GetComponentInChildren<GunDetail>().spdmultiplier))).ToString("F2") + " rps";
                    spd_prc_txt.text = (Mathf.CeilToInt(((float)(1.05 - Player[Me].GetComponentInChildren<GunDetail>().spdmultiplier) * 20000))).ToString();
                }
                else if (Input.GetKeyUp("e"))
                {
                    upgradePanel.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
}