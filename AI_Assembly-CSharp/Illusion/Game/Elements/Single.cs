// Decompiled with JetBrains decompiler
// Type: Illusion.Game.Elements.Single
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UniRx;

namespace Illusion.Game.Elements
{
  public class Single : IObservable<Unit>, IDisposable
  {
    private readonly AsyncSubject<Unit> _asyncSubject = new AsyncSubject<Unit>();
    private readonly object lockObject = new object();

    public void Done()
    {
      lock (this.lockObject)
      {
        if (this._asyncSubject.get_IsCompleted())
          return;
        this._asyncSubject.OnNext(Unit.get_Default());
        this._asyncSubject.OnCompleted();
      }
    }

    public IDisposable Subscribe(IObserver<Unit> observer)
    {
      lock (this.lockObject)
        return this._asyncSubject.Subscribe(observer);
    }

    public void Dispose()
    {
      lock (this.lockObject)
        this._asyncSubject.Dispose();
    }
  }
}
