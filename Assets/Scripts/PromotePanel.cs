using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PromotePanel : MonoBehaviour
{
    public CanvasGroup promotePanel;

    private Pawn promotingPawn;

    public void Start()
    {
        promotePanel = GetComponent<CanvasGroup>();
    }

    public void EnablePromotePanel(MonoBehaviour caller)
    {
        if (caller != null)
        {
            promotingPawn = caller.GetComponent<Pawn>();
        }
        if (promotePanel != null)
        {
            Debug.Log("initiating promotion..");
            promotePanel.alpha = 1f;
            promotePanel.interactable = true;
            promotePanel.blocksRaycasts = true;
        }
    }

    public void DisablePromotePanel()
    {

        if (promotePanel != null)
        {
            promotePanel.alpha = 0f;  // Visibility to 0 percent
            promotePanel.interactable = false;  // disallow interactions
            promotePanel.blocksRaycasts = false;  // dont block raycasts

            promotingPawn = null;
        }
    }

    public void PromoteQueen()
    {
        if (promotingPawn != null)
        {
            promotingPawn.PromotePawn('q');
            DisablePromotePanel();
        }
    }

    public void PromoteRook()
    {
        if (promotingPawn != null)
        {
            promotingPawn.PromotePawn('r');
            DisablePromotePanel();
        }
    }
    public void PromoteBishop()
    {
        if (promotingPawn != null)
        {
            promotingPawn.PromotePawn('b');
            DisablePromotePanel();
        }
    }
    public void PromoteKnight()
    {
        if (promotingPawn != null)
        {
            promotingPawn.PromotePawn('k');
            DisablePromotePanel();
        }
    }
}
