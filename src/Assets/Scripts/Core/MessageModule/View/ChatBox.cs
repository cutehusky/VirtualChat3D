﻿using LogicUI.FancyTextRendering;
using Nobi.UiRoundedCorners;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MessageModule.View
{
    public class ChatBox: MonoBehaviour
    {
        public TMP_Text role;
        public MarkdownRenderer text;
        public TMP_Text time;
        public Image background;
        public ImageWithIndependentRoundedCorners roundedCorners;
    }
}