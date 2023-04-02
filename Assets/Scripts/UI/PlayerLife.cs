using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PlayerLife : MonoBehaviour
    {
        [SerializeField] private List<GameObject> lifeImgList;

        private int currentLife;
        
        public Action OnLifeEnded { get; set; }
        
        public void ActivateLife()
        {
            currentLife = lifeImgList.Count;
            foreach (var lifeImg in lifeImgList)
                lifeImg.SetActive(true);
        }

        public void MinusLife()
        {
            currentLife--;
            if (currentLife < 0)
            {
                OnLifeEnded?.Invoke();
                return;
            }
            
            lifeImgList[currentLife].SetActive(false);
        }

        public bool IsLifeEnded()
        {
            return currentLife < 0;
        }
    }
}