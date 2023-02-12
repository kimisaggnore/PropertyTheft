using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwapCounter : MonoBehaviour
{
    TextMeshProUGUI mText;
    private int swapCount = 0;
    public int parSwaps = 0;
    // Start is called before the first frame update
    void Start()
    {
        mText = GetComponent<TextMeshProUGUI>();
        FindObjectOfType<PlayerSwapProperty>().SuccessfulSwap += UpdateText;
        mText.text = $"Swaps\n0/{parSwaps}";
    }

    private void UpdateText(PlayerSwapProperty psp)
    {
        swapCount += 1;
        mText.text = $"Swaps\n{swapCount}/{parSwaps}";
    }
}
