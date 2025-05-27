    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class CharUpdater : MonoBehaviour
    {
        public Sprite[] newSprite;
        public Image charImage;
        public void ChangeSourceImage(int charIndex)
        {
            charImage.sprite = newSprite[charIndex];
        }

    }
