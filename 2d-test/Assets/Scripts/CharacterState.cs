using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterState", menuName="2d-test/CharacterState")]
public class CharacterState : ScriptableObject
{
    [field: SerializeField] public bool CanMove { get; set; } = true;
    [field: SerializeField] public bool CanExitWhilePlaying { get; set; } = true;
}
