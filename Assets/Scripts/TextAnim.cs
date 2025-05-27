using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SheviaFireX
{
    public class TextAnim : MonoBehaviour
    {
        //public AnimatorHandler animatorHandler;
        public TextMeshProUGUI _textMeshPro;

        public string stringArray;

        [SerializeField] float timeBtwnChars;
        //[SerializeField] float timeBtwnWords;

        int i = 0;

        void OnEnable()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
            stringArray = _textMeshPro.text;
            EndCheck();


        }

        public void EndCheck()
        {
            if (i <= stringArray.Length - 1)
            {
                //animatorHandler.TriggerGoTalk();
                _textMeshPro.text = stringArray;
                StartCoroutine(TextVisible());
            }


        }

        public IEnumerator TextVisible()
        {

            _textMeshPro.ForceMeshUpdate();
            int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
            int counter = 0;
            while (true)
            {
                int visibleCount = counter % (totalVisibleCharacters + 1);
                _textMeshPro.maxVisibleCharacters = visibleCount;

                if (visibleCount >= totalVisibleCharacters)
                {
                    i += 1;
                    if (i > stringArray.Length - 1) // cek apakah sudah sampai ke teks terakhir
                    {
                        yield break; // hentikan coroutine
                    }
                    else
                    {
                        //Invoke("EndCheck", 0);
                        //Debug.Log("end");    
                        break;
                    }
                }

                counter += 1;
                yield return new WaitForSeconds(timeBtwnChars);
            }

            //animatorHandler.TriggerGoIdle();

        }
    }
}