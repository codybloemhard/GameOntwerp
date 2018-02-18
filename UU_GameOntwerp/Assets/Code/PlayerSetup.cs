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

    private void Start()
    {
        lastPhase = Phase.NONE;
        body = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        InitNet();
        
        if (isLocalPlayer)
        {
            lobbyCam = Camera.main;
            if (lobbyCam != null)
                lobbyCam.gameObject.SetActive(false);
        }
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
        Phase currentPhase = Center.instance.phase;
        if (currentPhase != lastPhase)
        {
            if (currentPhase == Phase.BUILDING)
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
        if(currentPhase == Phase.BUILDING)
            body.velocity = Vector3.zero;
    }
}