using System;
using System.Collections.Generic;
using UnityEngine;

namespace Starxr.SDK.AI.Core
{
    //[CreateAssetMenu(fileName = "NewNPCData", menuName = "ScriptableObjects/NPCData", order = 1)]
    public class GPTSettings : ScriptableObject
    {
        private const string AssetPath = "GPT/GPTSetting";
        public static GPTSettings Instance
        {
            get
            {
                if (instance is null)
                {
                    instance = Resources.Load<GPTSettings>(AssetPath);
                }
                return instance;
            }
        }
        private static GPTSettings instance = null;

        [Header("API Keys")]
        public string GPTAPIKey;
        public string GoogleCloudAPIKey;

        [Header("Server URLs")]
        //public string DefaultServerURL;
        public string TTSAPIUrl;
        public string STTAPIUrl;
        public string DefaultServerURL => defaultServerURL;

        [SerializeField]
        private string defaultServerURL = string.Empty;

        [Header("NPC and Language Settings")]
        public List<NPCIDEntry> npcIDList = new List<NPCIDEntry>();
        public List<LanguageEntry> languageList = new List<LanguageEntry>();

        public Dictionary<AICategory, string> GetNPCIDDictonary()
        {
            Dictionary<AICategory, string> npcIDDict = new();
            foreach (var entry in npcIDList)
            {
                npcIDDict[entry.category] = entry.npcID;
            }
            return npcIDDict;
        }

        public Dictionary<LANGUAGE, string> GetLanguageDictonary()
        {
            Dictionary<LANGUAGE, string> languageDict = new();
            foreach (var entry in languageList)
            {
                languageDict[entry.language] = entry.languageCode;
            }
            return languageDict;
        }
    }

    [System.Serializable]
    public class NPCIDEntry
    {
        public AICategory category;
        public string npcID;
    }

    [System.Serializable]
    public class LanguageEntry
    {
        public LANGUAGE language;
        public string languageCode;
    }

    public enum AICategory
    {
        Math,
        Art,
        Economy,
        ShowHost,
        Korean,
        English
    }

    public enum LANGUAGE
    {
        ko, //ko-KR
        en, //en-US
    }

    public enum GENDER
    {
        MALE,
        FEMALE,
    }
}
