using UnityEngine;

public class BridgeScript : MonoBehaviour
{
    public GameObject bridge = null;
    public bool isInRange;
    private GameObject player = null;
    private InfoMessage infoMessage;
    private GameManagerScript gameManagerScript;
    public ParticleSystem particleSystem;
    private void OnEnable()
    {
        infoMessage = GameObject.FindGameObjectWithTag("InfoCanvas").GetComponent<InfoMessage>();
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isInRange = true;
            player = other.transform.gameObject;
            if (!player.GetComponent<Interactor>().CanFixBridge)
            {
                infoMessage.DisplayMessage("You need a hammer", 1.5F);
            }
            else
            {
                infoMessage.DisplayMessage("E to build the bridge", 1.5F, true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isInRange = false;
            player = null;
        }
    }

    private void Update()
    {
        if (isInRange && player != null)
        {
            if (Input.GetKeyDown(KeyCode.E)|| Input.GetKeyDown(KeyCode.F))
            {
                if (player.GetComponent<Interactor>().CanFixBridge)
                {
                    player.GetComponent<TileMaker>().BridgeIsFixed = true;
                    gameObject.SetActive(false);
                    if (infoMessage.CurrentMessage != "The bridge is fixed!")
                        infoMessage.DisplayMessage("The bridge is fixed!", 1.5F, true);
                    Interactor.BridgeFixed = true;

                    gameManagerScript.AddItemToInventory(InventoryItem.Hammer, -1);

                    if (particleSystem != null)
                        particleSystem.Stop();
                }
                else
                {
                    if (infoMessage.CurrentMessage != "You need a hammer")
                        infoMessage.DisplayMessage("You need a hammer", 1.5F);
                }
            }
        }
    }
}

