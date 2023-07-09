using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiContainer : MonoBehaviour
{
    public static EmojiContainer Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    [SerializeField] private Sprite happyEmoji;
    [SerializeField] private Sprite neutralEmoji;
    [SerializeField] private Sprite sadEmoji;
    [SerializeField] private Sprite angryEmoji;

    public Sprite HappyEmoji => happyEmoji;

    public Sprite NeutralEmoji => neutralEmoji;

    public Sprite SadEmoji => sadEmoji;

    public Sprite AngryEmoji => angryEmoji;
}
