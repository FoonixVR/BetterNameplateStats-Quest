using MelonLoader;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using VRC;
using System.Collections.Generic;
using MelonLoader.TinyJSON;
using System.Collections;

[assembly: MelonInfo(typeof(NameplateStats.Main), "BetterNameplateStats", "0.0.1", "Foonix")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace NameplateStats
{
    public class Main : MelonMod
    {
        public static List<NameplateStructure> userTags = new List<NameplateStructure>();
        private int noUpdateCount;
        private byte frames;
        private byte ping;
        
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("NameplateStats initializing..");
        }

        public IEnumerator OnUIManagerInit()
        {
            MelonLogger.Msg("Waiting For Ui");
            while (GameObject.Find("UserInterface") == null) yield return null;
            while (GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)") == null) yield return null;
            MelonLogger.Msg("Ui loaded");
            
            var field0 = NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_ObjectPublicHa1UnT1Unique_1_Player_0;
            field0.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<Player>(OnPlayerJoin));
        }

        private TextMeshProUGUI AddTag(string CustomTag, Color color, Player player)
        {
            PlayerNameplate nameplate2 = player.prop_VRCPlayer_0.field_Public_PlayerNameplate_0;
            Transform transform = UnityEngine.Object.Instantiate<Transform>(nameplate2.gameObject.transform.Find("Contents/Quick Stats"), nameplate2.gameObject.transform.Find("Contents"));
            transform.parent = nameplate2.gameObject.transform.Find("Contents");
            transform.gameObject.SetActive(true);
            TextMeshProUGUI component = transform.Find("Trust Text").GetComponent<TextMeshProUGUI>();
            component.color = color;
            transform.Find("Trust Icon").gameObject.SetActive(false);
            transform.Find("Performance Icon").gameObject.SetActive(false);
            transform.Find("Performance Text").gameObject.SetActive(false);
            transform.Find("Friend Anchor Stats").gameObject.SetActive(false);
            transform.name = "Shadow Info Tag";
            transform.gameObject.transform.localPosition = new Vector3(0f, 180f, 0f);
            transform.GetComponent<ImageThreeSlice>().color = Color.white;
            component.text = CustomTag;
            return component;
        }

        public void OnPlayerJoin(Player player)
        {
            NameplateStructure nameplate = new NameplateStructure();
            nameplate.component = AddTag("", Color.white, player);
            nameplate.ID = player.field_Private_APIUser_0.id;
            userTags.Add(nameplate);
        }

        public override void OnUpdate()
        {
            System.Threading.Thread.Sleep(3000);
            
            PlayerManager.prop_PlayerManager_0.field_Private_List_1_Player_0.ToArray().ToList<Player>().ForEach(player =>
            {
                if (userTags.Count == 0) return;
                for (var i = 0; i < userTags.Count; i++)
                {
                    if (frames == player._playerNet.field_Private_Byte_0 && ping == player._playerNet.field_Private_Byte_1)
                    {
                        noUpdateCount++;
                    }
                    else
                    {
                        noUpdateCount = 0;
                    }

                    frames = player._playerNet.field_Private_Byte_0;
                    ping = player._playerNet.field_Private_Byte_1;


                    if (player.field_Private_VRCPlayerApi_0.isMaster)
                    {
                        userTags[i].component.text += "[<color=#FFB300>H</color>] ";
                    }

                    string textStatus = "<color=green>Stable</color>";
                    if (noUpdateCount > 30)
                        textStatus = "<color=yellow>Lagging</color>";
                    if (noUpdateCount > 150)
                        textStatus = "<color=red>Crashed</color>";
                    userTags[i].component.text = $"[{textStatus}] | {PlayerWrapper.GetPingTextColord(player)}: {PlayerWrapper.GetPingColord(player)}| {PlayerWrapper.GetFramesTextColord(player)}: {PlayerWrapper.GetFramesColord(player)}";
                }
            });
        }
    }
}