using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class StateAnimationSetDictionary : SerializableDictionary<CharacterState, DirectionalAnimationSet>
{
    public AnimationClip GetFacingClipFromState(CharacterState characterState, Vector2 facingDirection)
    {
        if (TryGetValue(characterState, out DirectionalAnimationSet animationSet))
        {
            //Found animation set in dictionary
            return animationSet.GetFacingClip(facingDirection);
        }
        else
        {
            Debug.LogError("Character state " + characterState.name + " is not found in StateAnimations Dictionary");
        }

        return null;
    }
}

