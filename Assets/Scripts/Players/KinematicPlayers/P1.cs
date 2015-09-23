using UnityEngine;
using System.Collections;

public class P1 : Player
{
    public override Vector3 HandleInput()
    {
        Vector3 input = new Vector3(0, 0, Input.GetAxisRaw("Vertical_P1"));
        input.Normalize();

        return input;
    }
}
