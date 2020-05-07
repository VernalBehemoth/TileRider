using System.Collections;
using UnityEngine;

public class InventoryItemProducer : MonoBehaviour
{
    public InventoryItem itemToProduce = InventoryItem.Thirst_Berry;
    public bool isInRange;
    private GameObject player = null;
    private InfoMessage infoMessage;
    private bool sendInfo = true;
    private readonly float messageCoolOff = 10F;
    public int maxCuantity = 5;
    private int takenCuantity = 0;
    public ParticleSystem particleSystem;

    private void OnEnable()
    {
        infoMessage = GameObject.FindGameObjectWithTag("InfoCanvas").GetComponent<InfoMessage>();
        StartCoroutine(DetectPlayer());
        if (particleSystem == null)
        {
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }
    }

    private IEnumerator MessageCoolOff()
    {
        yield return new WaitForSeconds(messageCoolOff);
        sendInfo = true;
    }

    private IEnumerator DetectPlayer()
    {
        for (; ; )
        {
            // Cast a sphere wrapping character controller 10 meters forward
            // to see if it is about to hit anything.
            int layerMask = LayerMask.GetMask("Player");
            Vector3 lowerPos = transform.position;
            lowerPos.y -= 2F;

            if (Physics.SphereCast(lowerPos, 0.75F, transform.up, out RaycastHit hit, 2F, layerMask, QueryTriggerInteraction.Ignore))
            {
                isInRange = true;
                player = hit.transform.gameObject;
                if (sendInfo)
                {
                    if (infoMessage.CurrentMessage != "Pick up with E")
                    {
                        infoMessage.DisplayMessage("Pick up with E", 1F);
                        sendInfo = false;
                        StartCoroutine(MessageCoolOff());
                    }
                }
            }
            else
            {
                isInRange = false;
                player = null;
            }

            yield return new WaitForSeconds(0.5F);
        }

    }

    // Update is called once per frame
    private void Update()
    {
        if (player != null)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
            {
                if (takenCuantity < maxCuantity)
                {
                    takenCuantity++;
                    player.GetComponent<Interactor>().AddToInventory(itemToProduce, 1);
                    if (GetComponent<AudioSource>() != null && !GetComponent<AudioSource>().isPlaying)
                        GetComponent<AudioSource>().Play();

                    if (itemToProduce == InventoryItem.Gold)
                        Interactor.chestsLocated += 1;

                }
                else
                {
                    if (sendInfo)
                    {
                        infoMessage.DisplayMessage("Nothing here anymore!", 1.5F, false);

                        StartCoroutine(MessageCoolOff());
                    }
                }
                if (particleSystem != null && particleSystem.isEmitting &&  takenCuantity >= maxCuantity)
                {
                    particleSystem.Stop();
                }
            }
            
        }
    }
}