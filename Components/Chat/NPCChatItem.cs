using TMPro;
using UnityEngine;
namespace Starxr.SDK.AI.Components
{
    public class NPCChatItem : MonoBehaviour
    {
        #region Public Fields 
        public TextMeshProUGUI AnswerText;

        #endregion

        #region Public Methods
        public void SetItem(string answerInfo)
        {
            if (AnswerText != null)
            {
                AnswerText.text = "";
                AnswerText.text = answerInfo;
            }
        }
        #endregion
    }
}
