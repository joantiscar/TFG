using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlockGrab : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        args.interactorObject.transform.gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.ActionBasedController>().enableInputActions = false;
   

        base.OnSelectEntered(args);
    }
}