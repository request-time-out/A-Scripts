// Decompiled with JetBrains decompiler
// Type: Correct.BaseCorrect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Correct.Process;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Correct
{
  public abstract class BaseCorrect : MonoBehaviour
  {
    [SerializeField]
    private List<BaseCorrect.Info> _list;

    protected BaseCorrect()
    {
      base.\u002Ector();
    }

    public List<BaseCorrect.Info> list
    {
      get
      {
        return this._list;
      }
      set
      {
        this._list = value;
      }
    }

    public List<string> GetFrameNames(string[] FrameNames)
    {
      List<string> stringList = new List<string>();
      foreach (string frameName in FrameNames)
        stringList.Add(frameName);
      return stringList;
    }

    public bool isEnabled
    {
      set
      {
        foreach (BaseCorrect.Info info in this.list)
          info.enabled = value;
      }
    }

    [Serializable]
    public class Info
    {
      [SerializeField]
      public string name;
      public BaseCorrect.Info.ProcOrderType type;
      [SerializeField]
      private Component component;
      [SerializeField]
      private BaseProcess _process;
      private BaseData _data;

      public Info(Component component)
      {
        this.component = component;
        this.name = ((Object) component).get_name();
      }

      public BaseProcess process
      {
        get
        {
          if (Object.op_Equality((Object) this._process, (Object) null))
            this._process = this.CreateProcess();
          return this._process;
        }
      }

      public void ReSetup()
      {
        this._process = this.CreateProcess();
      }

      public BaseData data
      {
        get
        {
          if (Object.op_Equality((Object) this._data, (Object) null))
            this._data = this.process.data;
          return this._data;
        }
      }

      public bool enabled
      {
        get
        {
          return ((Behaviour) this.process).get_enabled();
        }
        set
        {
          ((Behaviour) this.process).set_enabled(value);
        }
      }

      private BaseProcess CreateProcess()
      {
        foreach (BaseProcess component in (BaseProcess[]) this.component.GetComponents<BaseProcess>())
        {
          BaseProcess delete = component;
          ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Action<M0>) (_ => Object.DestroyImmediate((Object) delete)));
        }
        switch (this.type)
        {
          case BaseCorrect.Info.ProcOrderType.First:
            return (BaseProcess) this.component.get_gameObject().AddComponent<IKBeforeOfDankonProcess>();
          case BaseCorrect.Info.ProcOrderType.Second:
            return (BaseProcess) this.component.get_gameObject().AddComponent<IKBeforeProcess>();
          case BaseCorrect.Info.ProcOrderType.Last:
            return (BaseProcess) this.component.get_gameObject().AddComponent<IKAfterProcess>();
          default:
            return (BaseProcess) null;
        }
      }

      public Transform bone
      {
        get
        {
          return this.data.bone;
        }
        set
        {
          this.data.bone = value;
        }
      }

      public Vector3 pos
      {
        get
        {
          return this.data.pos;
        }
        set
        {
          this.data.pos = value;
        }
      }

      public Quaternion rot
      {
        get
        {
          return this.data.rot;
        }
        set
        {
          this.data.rot = value;
        }
      }

      public Vector3 ang
      {
        get
        {
          return this.data.ang;
        }
        set
        {
          this.data.ang = value;
        }
      }

      public enum ProcOrderType
      {
        First,
        Second,
        Last,
      }
    }
  }
}
