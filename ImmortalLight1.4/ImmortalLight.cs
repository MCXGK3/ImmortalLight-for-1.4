using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace ImmortalLight
{
    public class ImmortalLight1_4 : Mod, ITogglableMod,Modding.ILogger
    {
        internal static ImmortalLight1_4 Instance;

        public Setting setting = new();

        //public override List<ValueTuple<string, string>> GetPreloadNames()
        //{
        //    return new List<ValueTuple<string, string>>
        //    {
        //        new ValueTuple<string, string>("White_Palace_18", "White Palace Fly")
        //    };
        //}

        //public ImmortalLight1._4() : base("ImmortalLight1._4")
        //{
        //    Instance = this;
        //}
        public ImmortalLight1_4() : base("ImmortalLight")
        {
            Instance = this;
        }
        
        public override string GetVersion()
        {
            return "0.0.1.1";
        }

        public bool ToggleButtonInsideMenu => true;
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        Log("Initializing");

        Instance = this;

        ModHooks.Instance.LanguageGetHook += ChangeLanguage;
        On.PlayMakerFSM.OnEnable += Modify;
            Log("Initialized");
    }

        private string ChangeLanguage(string key, string sheetTitle)
        {
            switch (key)
            {
                case "NAME_FINAL_BOSS": return OtherLanguage("永恒之光", "ImmortalLight");
                case "ABSOLUTE_RADIANCE_SUPER": return OtherLanguage("直面", "Confront");
                case "ABSOLUTE_RADIANCE_MAIN": return OtherLanguage("永恒之光", "ImmortalLight");
                case "GODSEEKER_RADIANCE_STATUE": return OtherLanguage("不会······消失······", "won't......disappear......");
                case "GG_S_RADIANCE": return OtherLanguage("永远不变，也永远变化；永远熄灭，也永远在燃烧", "Changes never and changes forever; burns up and burns out");
                default: return Language.Language.orig_Get(key,sheetTitle);
            }
        }

        private void Modify(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            try
            {
                if (self.FsmName == "Control" && self.gameObject.name == "Absolute Radiance")
                {
                    Log("Radiance");
                    Log(BossSequenceController.IsInSequence);
                    if (setting.inPantheon || !BossSequenceController.IsInSequence)
                    {
                        Log("Immortal");
                        self.gameObject.AddComponent<Immortal>();
                        Log("End");
                    }
                }
            }
            catch (Exception e) { Log(e); }
            orig(self);
        }


        private string OtherLanguage(string chinese, string english)
        {
            if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
            {
                return chinese;
            }
            else return english;
        }
        public void Unload()
        {
            ModHooks.Instance.LanguageGetHook += ChangeLanguage;
            On.PlayMakerFSM.OnEnable += Modify;
        }
    }
}