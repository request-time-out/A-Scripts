// Decompiled with JetBrains decompiler
// Type: UnityEngineExtra.DontDestroyParent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace UnityEngineExtra
{
  public class DontDestroyParent : MonoBehaviour
  {
    private static DontDestroyParent instance;

    public DontDestroyParent()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (Object.op_Equality((Object) DontDestroyParent.Instance, (Object) this))
        Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      else
        Object.Destroy((Object) ((Component) this).get_gameObject());
    }

    public static void Register(GameObject obj)
    {
      obj.get_transform().set_parent(((Component) DontDestroyParent.Instance).get_transform());
    }

    public static void Register(MonoBehaviour component)
    {
      DontDestroyParent.Register(((Component) component).get_gameObject());
    }

    public static DontDestroyParent Instance
    {
      get
      {
        if (Object.op_Equality((Object) DontDestroyParent.instance, (Object) null))
        {
          DontDestroyParent.instance = (DontDestroyParent) Object.FindObjectOfType<DontDestroyParent>();
          if (Object.op_Equality((Object) DontDestroyParent.instance, (Object) null))
            DontDestroyParent.instance = (DontDestroyParent) new GameObject(nameof (DontDestroyParent)).AddComponent<DontDestroyParent>();
          ((Object) ((Component) DontDestroyParent.instance).get_gameObject()).set_hideFlags((HideFlags) 8);
        }
        return DontDestroyParent.instance;
      }
    }
  }
}
