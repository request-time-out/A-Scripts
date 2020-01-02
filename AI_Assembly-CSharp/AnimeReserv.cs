// Decompiled with JetBrains decompiler
// Type: AnimeReserv
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class AnimeReserv
{
  protected Queue<AnimeReserv.AnimeData> animeQueue;
  protected Animation animation;

  public AnimeReserv(Animation _animation)
  {
    this.animation = _animation;
    this.animeQueue = new Queue<AnimeReserv.AnimeData>();
  }

  public void ReservAdd(string name, float fadeSpeed = 0.0f)
  {
    this.animeQueue.Enqueue(new AnimeReserv.AnimeData(name, fadeSpeed));
  }

  public void ReservEXE()
  {
    while (this.animeQueue.Count > 0)
    {
      AnimeReserv.AnimeData animeData = this.animeQueue.Dequeue();
      if ((double) animeData.FadeSpeed == 0.0)
        this.animation.PlayQueued(animeData.Name);
      else
        this.animation.CrossFadeQueued(animeData.Name, animeData.FadeSpeed);
    }
  }

  protected class AnimeData
  {
    private string name;
    private float fadeSpeed;

    public AnimeData(string _name, float _fadeSpeed = 0.0f)
    {
      this.name = _name;
      this.fadeSpeed = _fadeSpeed;
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    public float FadeSpeed
    {
      get
      {
        return this.fadeSpeed;
      }
    }
  }
}
