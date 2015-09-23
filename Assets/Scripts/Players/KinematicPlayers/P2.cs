using UnityEngine;
using System.Collections;

public class P2 : Player 
{
    public override Vector3 HandleInput()
    {
        Vector3 input = new Vector3(0, 0, Input.GetAxisRaw("Vertical_P2"));
        input.Normalize();

        return input;
    }
}
