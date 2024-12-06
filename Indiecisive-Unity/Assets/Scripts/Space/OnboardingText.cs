using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;
using Unity.VisualScripting;

public class OnboardingText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMeshPro;

    [SerializeField]
    protected float timeBetweenChars;

    [SerializeField]
    protected float timeBetweenWords;

    public string[] stringArray;

    public bool invokeBool = true;

    public int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        EndCheck();
    }

    public void EndCheck()
    {
        if (i <= stringArray.Length - 1)
        {
            textMeshPro.text = stringArray[i];
            StartCoroutine(TextVisible());
        }
    }

    private IEnumerator TextVisible()
    {
        textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = textMeshPro.textInfo.characterCount;
        int count = 0;

        while(true/*count < totalVisibleCharacters*/)
        {
            int visibleCount = count % (totalVisibleCharacters + 1);
            textMeshPro.maxVisibleCharacters = visibleCount;

            if(visibleCount >= totalVisibleCharacters && invokeBool == true)
            {
                i++;
                Invoke("EndCheck", timeBetweenWords);
                break;
            }

            count++;

            yield return new WaitForSeconds(timeBetweenChars);
        }
    }
}
