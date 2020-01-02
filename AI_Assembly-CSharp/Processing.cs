// Decompiled with JetBrains decompiler
// Type: Processing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Processing : MonoBehaviour
{
  [SerializeField]
  private Image[] img;
  private Color[] color;
  public bool update;

  public Processing()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    for (int index1 = 0; index1 < 12; ++index1)
      this.color[index1] = Color.HSVToRGB(0.0f, 0.0f, (float) (1.0 - (double) index1 * 0.0199999995529652));
    int index = 0;
    DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Interval(TimeSpan.FromMilliseconds(50.0)), (Action<M0>) (_ =>
    {
      if (this.update)
      {
        index = (index + 11) % 12;
        for (int index1 = 0; index1 < 12; ++index1)
        {
          int index2 = (index + index1) % 12;
          ((Graphic) this.img[index1]).set_color(this.color[index2]);
        }
      }
      for (int index1 = 0; index1 < 12; ++index1)
        ((Behaviour) this.img[index1]).set_enabled(this.update);
    })), (Component) this);
  }

  private void Update()
  {
  }
}
