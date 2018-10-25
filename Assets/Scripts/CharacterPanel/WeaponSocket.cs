using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class WeaponSocket : GearSocket
{
    private float currentY;

    [SerializeField]
    private SpriteRenderer parentRenderer;

    public override void SetXAndY(float x, float y)
    {
        base.SetXAndY(x, y);

        if (currentY != y)
        {
            if (y == 1)
            {
                //Back
                spriteRenderer.sortingOrder = parentRenderer.sortingOrder - 1;
            }
            else
            {
                //Front
                spriteRenderer.sortingOrder = parentRenderer.sortingOrder + 5;
            }
        }
    }
}