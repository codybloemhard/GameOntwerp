using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] localOff_bhv, externOff_bhv;
    [SerializeField]
    private GameObject[] localOff_obj, externOff_obj;
    [SerializeField]
    private Camera fpsCam;
    private Camera lobbyCam;
    private Phase lastPhase;
    //components on this object
    private Rigidbody body;
    private CapsuleCollider collider;
    [SerializeField]
    private Behaviour physicsControls, flyControls, blockSpawner, shooting;
    private string localName = "";
    private bool isSpectator;

    private void Start()
    {
        lastPhase = Phase.NONE;
        body = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        InitNet();
        
        if (isLocalPlayer)
        {
            CmdRegistrate();
            string name = Center.instance.GetLocalName();
            CmdSetName(name);
            lobbyCam = Camera.main;
            if (lobbyCam != null)
                lobbyCam.gameObject.SetActive(false);
        }
    }

    [Command]
    private void CmdRegistrate()
    {
        string treasure = "";
        if (transform.position.x > 0)
            treasure = "B";
        else treasure = "A";
        int nr = Center.instance.GetNewPlayer(treasure);
        if (isClient)
            RpcSetplayerNrOnLocal(nr);
        else if(isLocalPlayer)
            GetComponent<Shooting>().SetNr(nr);
        if (nr > 1) isSpectator = true;
        else isSpectator = false;
        RpcSetSpectator(isSpectator);//dont need to check for isClient: host is first player and never spectator!
    }

    [ClientRpc]
    private void RpcSetSpectator(bool spec)
    {
        isSpectator = spec;
        GetComponent<FlyMovement>().SetSpectator(spec);
    }

    [ClientRpc]
    private void RpcSetplayerNrOnLocal(int nr)
    {
        if (!isLocalPlayer) return;
        GetComponent<Shooting>().SetNr(nr);
    }
    
    [Command]
    private void CmdSetName(string name)
    {
        Center.instance.SetName(name);
    }

    //enable and disable the correct behaviours and objects based on if were the local player
    private void InitNet()
    {
        if (isLocalPlayer)
        {
            for (int i = 0; i < localOff_bhv.Length; i++)
                localOff_bhv[i].enabled = false;
            for (int i = 0; i < localOff_obj.Length; i++)
                localOff_obj[i].SetActive(false);
        }
        else
        {
            for (int i = 0; i < externOff_bhv.Length; i++)
                externOff_bhv[i].enabled = false;
            for (int i = 0; i < externOff_obj.Length; i++)
                externOff_obj[i].SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (lobbyCam != null)
            lobbyCam.gameObject.SetActive(true);
    }

    private void Update()
    {
        Phase currentPhase = Center.instance.GetPhase();
        if (currentPhase != lastPhase)
        {
            if (isSpectator || currentPhase == Phase.PREGAME || currentPhase == Phase.POSTGAME || currentPhase == Phase.POSTROUND)
            {
                body.useGravity = false;
                collider.enabled = false;
                physicsControls.enabled = false;
                flyControls.enabled = true;
                blockSpawner.enabled = false;
                shooting.enabled = false;
                (flyControls as FlyMovement).mouseLook.Init(transform, fpsCam.transform);
            }
            else if (currentPhase == Phase.BUILDING)
            {
                body.useGravity = false;
                collider.enabled = false;
                physicsControls.enabled = false;
                flyControls.enabled = true;
                blockSpawner.enabled = true;
				shooting.enabled = false;
                (flyControls as FlyMovement).mouseLook.Init(transform, fpsCam.transform);
            }
            else if (currentPhase == Phase.PLAYING)
            {
                body.useGravity = true;
                collider.enabled = true;
                physicsControls.enabled = true;
                flyControls.enabled = false;
                (blockSpawner as BlockSpawner).End();
                blockSpawner.enabled = false;
				shooting.enabled = true;
                (physicsControls as RigidbodyFirstPersonController).mouseLook.Init(transform, fpsCam.transform);
            }
            InitNet();
        }

        if(isSpectator || currentPhase == Phase.BUILDING || currentPhase == Phase.PREGAME || currentPhase == Phase.POSTGAME 
            || currentPhase == Phase.POSTROUND)
            body.velocity = Vector3.zero;
    }
}