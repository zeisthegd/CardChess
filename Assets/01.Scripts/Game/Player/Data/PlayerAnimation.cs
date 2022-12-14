using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Data/Animation", fileName = "New Player Animation")]
public class PlayerAnimation : ScriptableObject
{
    [SerializeField] AnimationClip idle;
    [SerializeField] AnimationClip hurt;
    [SerializeField] AnimationClip defeated;
    [SerializeField] AnimationClip normalAttack;
    [SerializeField] AnimationClip specialAttack;
    [SerializeField] AnimationClip castEffect;

    public AnimationClip Idle { get => idle; set => idle = value; }
    public AnimationClip Hurt { get => hurt; set => hurt = value; }
    public AnimationClip Defeated { get => defeated; set => defeated = value; }
    public AnimationClip NormalAttack { get => normalAttack; set => normalAttack = value; }
    public AnimationClip SpecialAttack { get => specialAttack; set => specialAttack = value; }
    public AnimationClip CastEffect { get => castEffect; set => castEffect = value; }
}
