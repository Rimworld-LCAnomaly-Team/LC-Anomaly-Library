using LCAnomalyLibrary.Setting;
using RimWorld;
using System.ComponentModel;
using Verse;

namespace LCAnomalyLibrary.Util
{
    /// <summary>
    /// 音乐工具
    /// </summary>
    public static class MusicUtils
    {
        public static void PlayMusic_LC(int points)
        {
            //如果未启用警报音乐，就不播放音乐
            if (!Setting_LCAnomalyLibrary_Main.Settings.If_EnableLCWarningMusic)
                return;

            MusicManagerPlay musicManager = Find.MusicManagerPlay;

            if (points >= 80)
            {
                if(musicManager.CurrentSong != Defs.SongDefOf.ThirdWarning)
                {
                    Log.Warning("警报音乐：播放3级警报音乐");
                    musicManager.ForcePlaySong(Defs.SongDefOf.ThirdWarning, false);
                }
                return;
            }

            if(points >= 50)
            {
                if (musicManager.CurrentSong != Defs.SongDefOf.SecondWarning)
                {
                    //播放比自己高的警报后，不会向下自动递减警报音乐
                    if (musicManager.CurrentSong == Defs.SongDefOf.ThirdWarning)
                        return;

                    Log.Warning("警报音乐：播放2级警报音乐");
                    musicManager.ForcePlaySong(Defs.SongDefOf.SecondWarning, false);
                }

                return;
            }

            if (points >= 10)
            {
                if (musicManager.CurrentSong != Defs.SongDefOf.FirstWarning)
                {
                    //播放比自己高的警报后，不会向下自动递减警报音乐
                    if (musicManager.CurrentSong == Defs.SongDefOf.ThirdWarning || musicManager.CurrentSong == Defs.SongDefOf.SecondWarning)
                        return;

                    Log.Warning("警报音乐：播放1级警报音乐");
                    musicManager.ForcePlaySong(Defs.SongDefOf.FirstWarning, false);
                }

                return;
            }

            //警报点数小于10时，重置音乐
            if (musicManager.CurrentSong == Defs.SongDefOf.FirstWarning
                || musicManager.CurrentSong == Defs.SongDefOf.SecondWarning
                || musicManager.CurrentSong == Defs.SongDefOf.ThirdWarning)
            {
                Log.Warning("警报音乐：重置音乐");
                musicManager.StartNewSong();
            }

        }

        public static int LevelTag2Points(string tag)
        {
            switch (tag)
            {
                case "ZAYIN":
                    return 5;
                case "TETH":
                    return 20;
                case "HE":
                    return 40;
                case "WAW":
                    return 60;
                case "ALEPH":
                    return 75;
                default:
                    throw new InvalidEnumArgumentException($"未知的异想体等级：{tag}");
            }
        }
    }
}
