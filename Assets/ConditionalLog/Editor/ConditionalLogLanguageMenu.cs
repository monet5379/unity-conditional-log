using UnityEditor;

namespace ConditionalLog.Editor
{
    public static class ConditionalLogLanguageMenu
    {
        const string EnglishPath = "Conditional Log/Language/English";
        const string KoreanPath = "Conditional Log/Language/한국어";

        [MenuItem(EnglishPath, false, 100)]
        public static void SetEnglish()
        {
            ConditionalLogLocale.SetLanguage(ConditionalLogLang.English);
        }

        [MenuItem(EnglishPath, true)]
        public static bool SetEnglishValidate()
        {
            Menu.SetChecked(EnglishPath, ConditionalLogLocale.Current == ConditionalLogLang.English);
            return true;
        }

        [MenuItem(KoreanPath, false, 101)]
        public static void SetKorean()
        {
            ConditionalLogLocale.SetLanguage(ConditionalLogLang.Korean);
        }

        [MenuItem(KoreanPath, true)]
        public static bool SetKoreanValidate()
        {
            Menu.SetChecked(KoreanPath, ConditionalLogLocale.Current == ConditionalLogLang.Korean);
            return true;
        }
    }
}
