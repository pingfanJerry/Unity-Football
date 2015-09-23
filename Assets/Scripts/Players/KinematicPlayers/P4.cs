using UnityEngine;
using System.Collections;

public class P4: Player 
{
    public override Vector3 HandleInput()
    {
        Vector3 input = new Vector3(0, 0, Input.GetAxisRaw("Vertical_P4"));
        input.Normalize();

        return input;
    }
}
