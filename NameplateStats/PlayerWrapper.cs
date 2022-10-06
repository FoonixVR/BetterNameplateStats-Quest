using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;

namespace NameplateStats
{
    static class PlayerWrapper
    {
        public static float GetFrames(this Player player) => (player._playerNet.prop_Byte_0 != 0) ? Mathf.Floor(1000f / (float)player._playerNet.prop_Byte_0) : -1f;

        public static short GetPing(this Player player) => player._playerNet.field_Private_Int16_0;

        public static string GetFramesColord(this Player player)
        {
            float frames = player.GetFrames();
            if (frames > 60f)
            {
                return "<color=green>" + frames.ToString() + "</color>";
            }
            if (frames > 25f)
            {
                return "<color=yellow>" + frames.ToString() + "</color>";
            }
            return "<color=red>" + frames.ToString() + "</color>";
        }

        public static string GetFramesTextColord(this Player player)
        {
            float frames = player.GetFrames();
            if (frames > 60f)
            {
                return "<color=green>FPS</color>";
            }
            if (frames > 25f)
            {
                return "<color=yellow>FPS</color>";
            }
            return "<color=red>FPS</color>";
        }

        public static string GetPingTextColord(this Player player)
        {
            short ping = player.GetPing();
            if (ping > 150)
            {
                return "<color=red>Ping</color>";
            }
            if (ping > 75)
            {
                return "<color=yellow>Ping</color>";
            }
            return "<color=green>Ping</color>";
        }

        public static string GetPingColord(this Player player)
        {
            short ping = player.GetPing();
            if (ping > 150)
            {
                return "<color=red>" + ping.ToString() + "</color>";
            }
            if (ping > 75)
            {
                return "<color=yellow>" + ping.ToString() + "</color>";
            }
            return "<color=green>" + ping.ToString() + "</color>";
        }
    }
}
