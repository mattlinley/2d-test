using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DirectionalAnimationSet", menuName = "2d-test/DirectionalAnimationSet")]
public class DirectionalAnimationSet : ScriptableObject
{
    [field: SerializeField] public AnimationClip Up { get; private set; }
    [field: SerializeField] public AnimationClip Down { get; private set; }
    [field: SerializeField] public AnimationClip Left { get; private set; }
    [field: SerializeField] public AnimationClip Right { get; private set; }

    /// <summary>
    /// Return the animation for the closest up/down/left/right direction of the character
    /// </summary>
    /// <param name="facingDirection">move direction from player controller</param>
    /// <returns>The animation clip for the required direction</returns>
    public AnimationClip GetFacingClip(Vector2 facingDirection)
    {
        //get closest direction to the input
        Vector2 closestDirection = GetClosestDirection(facingDirection);

        //return animation clip based on direction
        if (closestDirection == Vector2.left)
        {
            return Left;
        }
        else if (closestDirection == Vector2.right)
        {
            return Right;
        }
        else if (closestDirection == Vector2.up)
        {
            return Up;
        }
        else if (closestDirection == Vector2.down)
        {
            return Down;
        }
        else
        {
            Debug.LogError("Direction not expected " + closestDirection);
            return null;
        }
    }


    /// <summary>
    /// Compare the distance of the normalized input direction to the four cardinal directions.
    /// The smallest distance will be the direction our character should be facing.
    /// </summary>
    /// <param name="inputDirection">move direction from player controller</param>
    /// <returns>the closest up/down/left/right as a vector2</returns>
    public Vector2 GetClosestDirection(Vector2 inputDirection)
    {
        //ensure no magnitude
        Vector2 normalizedDirection = inputDirection.normalized;

        Vector2 closestDirection = Vector2.zero;
        float closestDistance = 0f;
        bool firstSet = false;

        Vector2[] directionsToCheck = new Vector2[4] { Vector2.down, Vector2.up, Vector2.left, Vector2.right };

        for (int i = 0; i< directionsToCheck.Length; i++)
        {
            if (!firstSet)
            {
                closestDirection = directionsToCheck[i];
                closestDistance = Vector2.Distance(inputDirection, directionsToCheck[i]);
                firstSet = true;
            }
            else
            {
                //Compare to the current closest direction and distance
                float nextDistance = Vector2.Distance(inputDirection, directionsToCheck[i]);

                if (nextDistance < closestDistance)
                {
                    //Update new closest distance
                    closestDistance = nextDistance;
                    closestDirection = directionsToCheck[i];
                }
            }
        }

        return closestDirection;
    }
}
