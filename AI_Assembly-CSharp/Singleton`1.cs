// Decompiled with JetBrains decompiler
// Type: Singleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
  private static T instance;

  public Singleton()
  {
    base.\u002Ector();
  }

  public static T Instance
  {
    get
    {
      if (!Object.op_Implicit((Object) (object) Singleton<T>.instance))
      {
        Singleton<T>.instance = (T) Object.FindObjectOfType(typeof (T));
        if (!Object.op_Implicit((Object) (object) Singleton<T>.instance))
          Debug.LogError((object) (typeof (T).ToString() + " is none."));
      }
      return Singleton<T>.instance;
    }
  }

  public static bool IsInstance()
  {
    return Object.op_Inequality((Object) null, (Object) (object) Singleton<T>.instance);
  }

  protected virtual void Awake()
  {
    this.CheckInstance();
  }

  protected virtual void OnDestroy()
  {
    if (!Object.op_Implicit((Object) (object) Singleton<T>.instance) || Object.op_Inequality((Object) this, (Object) (object) Singleton<T>.Instance))
      return;
    Singleton<T>.instance = (T) null;
  }

  protected bool CheckInstance()
  {
    if (Object.op_Equality((Object) this, (Object) (object) Singleton<T>.Instance))
      return true;
    Object.Destroy((Object) this);
    return false;
  }
}
