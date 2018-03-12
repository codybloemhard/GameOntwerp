using UnityEngine;
using UnityEngine.Networking;

#if ENABLE_UNET
[RequireComponent(typeof(NetworkManager))]

public class CustomLanControls : MonoBehaviour
{
    public static CustomLanControls instance;
    [SerializeField]
	private NetworkManager manager;

    private void Awake()
	{
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
		manager = GetComponent<NetworkManager>();
	}

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q))
            QuitConnection();
    }
    
    public void QuitConnection()
    {
        if (NetworkServer.active || NetworkClient.active)
        {
            manager.StopHost();
            Center.instance.Reset();
        }
    }
    
    public void StartHost()
    {
        if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            manager.StartHost();
    }

    public void StartClient()
    {
        if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            manager.StartClient();
    }

    public string NetworkAddress()
    {
        return manager.networkAddress;
    }

    public void SetNetworkAddress(string newaddress)
    {
        manager.networkAddress = newaddress;
    }

    public bool ConnectionExists()
    {
        return NetworkClient.active || NetworkServer.active;
    }
}
#endif //ENABLE_UNET