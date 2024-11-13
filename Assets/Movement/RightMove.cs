using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    bool isPressed = false;
    public GameObject Player;
    public float Force;
    public Animator animator;

    public OtitsDialogue otitsdialogueManager; 
    public CatDialogue catdialogueManager;
    public MamaDialogue mamadialogueManager;
    public DialogueManager tambaydialogueManager;
    public DialogueManager3 loladialogueManager;
    public ChismisDialogue chismisdialogueManager;
    public TeddyDialogue teddydialogueManager;
    public JuperDialogue juperdialogueManager;
    public EpenDialogue ependialogueManager;
    public KidnapperDialogue kidnapperdialogueManager;
    public MamaDialogue2 mama2dialogueManager;
    public NellyDialogue nellydialogueManager;
    public IceDialogue IcedialogueManager;
    public KidnapperDialogue1 kidnapper1dialogueManager;
    public IanDialogue IandialogueManager;
    public mamaDial mama3dialogueManager;
    public AsniDialogue asnidialogueManager;
    public WarsakDial warsakdialogueManager;
    public mom2Dial mom2dialogueManager;







    private void Start()
    {
        otitsdialogueManager = OtitsDialogue.GetInstance();
        catdialogueManager = CatDialogue.GetInstance();
        mamadialogueManager = MamaDialogue.GetInstance();
        tambaydialogueManager = DialogueManager.GetInstance();
        loladialogueManager = DialogueManager3.GetInstance();
        chismisdialogueManager = ChismisDialogue.GetInstance();
        teddydialogueManager = TeddyDialogue.GetInstance();
        juperdialogueManager = JuperDialogue.GetInstance();
        ependialogueManager = EpenDialogue.GetInstance();
        kidnapperdialogueManager = KidnapperDialogue.GetInstance();
        mama2dialogueManager = MamaDialogue2.GetInstance();
        nellydialogueManager = NellyDialogue.GetInstance();
        IcedialogueManager = IceDialogue.GetInstance();
        kidnapper1dialogueManager = KidnapperDialogue1.GetInstance();
        IandialogueManager = IanDialogue.GetInstance();
        mama3dialogueManager = mamaDial.GetInstance();
        asnidialogueManager = AsniDialogue.GetInstance();
        warsakdialogueManager = WarsakDial.GetInstance();
        mom2dialogueManager = mom2Dial.GetInstance();


        if (Player == null)
        {
            Debug.LogError("Player is not assigned in the inspector");
        }

        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in the inspector");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if ((otitsdialogueManager != null && otitsdialogueManager.dialogueIsPlaying) ||
           (catdialogueManager != null && catdialogueManager.dialogueIsPlaying) ||
           (mamadialogueManager != null && mamadialogueManager.dialogueIsPlaying) ||
           (tambaydialogueManager != null && tambaydialogueManager.dialogueIsPlaying) ||
           (loladialogueManager != null && loladialogueManager.dialogueIsPlaying) ||
           (chismisdialogueManager != null && chismisdialogueManager.dialogueIsPlaying) ||
           (teddydialogueManager != null && teddydialogueManager.dialogueIsPlaying) ||
           (juperdialogueManager != null && juperdialogueManager.dialogueIsPlaying) ||
           (ependialogueManager != null && ependialogueManager.dialogueIsPlaying) ||
           (kidnapperdialogueManager != null && kidnapperdialogueManager.dialogueIsPlaying) ||
           (mama2dialogueManager != null && mama2dialogueManager.dialogueIsPlaying) ||
           (nellydialogueManager != null && nellydialogueManager.dialogueIsPlaying) ||
           (IcedialogueManager != null && IcedialogueManager.dialogueIsPlaying) ||
           (kidnapper1dialogueManager != null && kidnapper1dialogueManager.dialogueIsPlaying) ||
           (IandialogueManager != null && IandialogueManager.dialogueIsPlaying) ||
           (mama3dialogueManager != null && mama3dialogueManager.dialogueIsPlaying) ||
           (asnidialogueManager != null && asnidialogueManager.dialogueIsPlaying) ||
           (warsakdialogueManager != null && warsakdialogueManager.dialogueIsPlaying) ||
           (mom2dialogueManager != null && mom2dialogueManager.dialogueIsPlaying))
        {
            if (animator != null)
            {
                animator.SetBool("isRunning", false);
            }
            return;
        }

        if (isPressed && Player != null)
        {
            Player.transform.Translate(Force * Time.deltaTime, 0, 0);
            if (animator != null)
            {
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isRunning", false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
