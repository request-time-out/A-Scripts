// Decompiled with JetBrains decompiler
// Type: ReMotion.TweenEngine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace ReMotion
{
  internal class TweenEngine
  {
    internal static TweenEngine Instance = new TweenEngine();
    private readonly object runningAndQueueLock = new object();
    private readonly object arrayLock = new object();
    private ITween[] tweens = new ITween[16];
    private Queue<ITween> waitQueue = new Queue<ITween>();
    private const int InitialSize = 16;
    private readonly Action<Exception> unhandledExceptionCallback;
    private int tail;
    private bool running;

    private TweenEngine()
    {
      this.unhandledExceptionCallback = (Action<Exception>) (ex => Debug.LogException(ex));
      MainThreadDispatcher.StartUpdateMicroCoroutine(this.RunEveryFrame());
    }

    public static void AddTween(ITween tween)
    {
      TweenEngine.Instance.Add(tween);
    }

    [DebuggerHidden]
    private IEnumerator RunEveryFrame()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TweenEngine.\u003CRunEveryFrame\u003Ec__Iterator0 everyFrameCIterator0 = new TweenEngine.\u003CRunEveryFrame\u003Ec__Iterator0();
      return (IEnumerator) everyFrameCIterator0;
    }

    public void Add(ITween tween)
    {
      lock (this.runningAndQueueLock)
      {
        if (this.running)
        {
          this.waitQueue.Enqueue(tween);
          return;
        }
      }
      lock (this.arrayLock)
      {
        if (this.tweens.Length == this.tail)
          Array.Resize<ITween>(ref this.tweens, checked (this.tail * 2));
        this.tweens[this.tail++] = tween;
      }
    }

    public void Run(float deltaTime, float unscaledDeltaTime)
    {
      lock (this.runningAndQueueLock)
        this.running = true;
      lock (this.arrayLock)
      {
        int index1 = this.tail - 1;
label_24:
        for (int index2 = 0; index2 < this.tweens.Length; ++index2)
        {
          ITween tween1 = this.tweens[index2];
          if (tween1 != null)
          {
            try
            {
              if (!tween1.MoveNext(ref deltaTime, ref unscaledDeltaTime))
                this.tweens[index2] = (ITween) null;
              else
                continue;
            }
            catch (Exception ex)
            {
              this.tweens[index2] = (ITween) null;
              try
              {
                this.unhandledExceptionCallback(ex);
              }
              catch
              {
              }
            }
          }
          while (index2 < index1)
          {
            ITween tween2 = this.tweens[index1];
            if (tween2 != null)
            {
              try
              {
                if (!tween2.MoveNext(ref deltaTime, ref unscaledDeltaTime))
                {
                  this.tweens[index1] = (ITween) null;
                  --index1;
                }
                else
                {
                  this.tweens[index2] = tween2;
                  this.tweens[index1] = (ITween) null;
                  --index1;
                  goto label_24;
                }
              }
              catch (Exception ex)
              {
                this.tweens[index1] = (ITween) null;
                --index1;
                try
                {
                  this.unhandledExceptionCallback(ex);
                }
                catch
                {
                }
              }
            }
            else
              --index1;
          }
          this.tail = index2;
          break;
        }
        lock (this.runningAndQueueLock)
        {
          this.running = false;
          while (this.waitQueue.Count != 0)
          {
            if (this.tweens.Length == this.tail)
              Array.Resize<ITween>(ref this.tweens, checked (this.tail * 2));
            this.tweens[this.tail++] = this.waitQueue.Dequeue();
          }
        }
      }
    }
  }
}
