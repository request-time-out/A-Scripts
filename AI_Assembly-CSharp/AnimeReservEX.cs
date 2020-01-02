// Decompiled with JetBrains decompiler
// Type: AnimeReservEX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class AnimeReservEX : AnimeReserv
{
  private AnimationAssist assist;

  public AnimeReservEX(AnimationAssist _assist)
    : base(_assist.NowAnimation)
  {
    this.assist = _assist;
  }

  public void Update()
  {
    if (this.animeQueue.Count <= 0 || !this.assist.IsAnimeEnd())
      return;
    AnimeReserv.AnimeData animeData = this.animeQueue.Dequeue();
    this.assist.Play(animeData.Name, 0.0f, animeData.FadeSpeed, 0, (WrapMode) 0);
  }
}
