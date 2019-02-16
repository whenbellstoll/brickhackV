using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Digit : MonoBehaviour {

    [SerializeField] private Sprite[] numbers;
    private SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = numbers[0];
    }

    public void SetColor(Color col)
    {
        rend.color = col;
    }

    public void SetSprite(int num)
    {
        if(num >= 0 && num < 10)
        {
            rend.sprite = numbers[num];
        }
    }


}
