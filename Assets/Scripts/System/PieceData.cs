using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName = "New Piece Data", menuName = "Piece")]
public class PieceData : ScriptableObject
{
    public Sprite base_sprite;
    public Sprite[] layers_sprite;
    public AudioClip capture_Clip;
    public AudioClip move_Clip;
    public AudioClip die_Clip;
    public GameObject die_ParticleEffect;

}
