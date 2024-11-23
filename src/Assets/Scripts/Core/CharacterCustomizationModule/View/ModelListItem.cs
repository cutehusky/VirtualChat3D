using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.CharacterCustomizationModule.View
{
    public class ModelListItem: MonoBehaviour
    {
        public TMP_Text modelId;
        public Button preview;
        public Button selectAsChatBot;
        public Button selectAsCharacter;
        void ScrollCellIndex(int idx)
        {
            
            gameObject.name = name;
        }
    }
}