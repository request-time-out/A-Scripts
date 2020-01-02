// Decompiled with JetBrains decompiler
// Type: Exploder.Utils.Compatibility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Utils
{
  public static class Compatibility
  {
    public static void SetVisible(GameObject obj, bool status, bool includeInactive)
    {
      if (!Object.op_Implicit((Object) obj))
        return;
      foreach (Renderer componentsInChild in (MeshRenderer[]) obj.GetComponentsInChildren<MeshRenderer>(includeInactive))
        componentsInChild.set_enabled(status);
      foreach (Renderer componentsInChild in (SkinnedMeshRenderer[]) obj.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive))
        componentsInChild.set_enabled(status);
    }

    public static bool IsActive(GameObject obj)
    {
      return Object.op_Implicit((Object) obj) && obj.get_activeSelf();
    }

    public static void SetActive(GameObject obj, bool status)
    {
      if (!Object.op_Implicit((Object) obj))
        return;
      obj.SetActive(status);
    }

    public static void SetActiveRecursively(GameObject obj, bool status)
    {
      if (!Object.op_Implicit((Object) obj))
        return;
      int childCount = obj.get_transform().get_childCount();
      for (int index = 0; index < childCount; ++index)
        Compatibility.SetActiveRecursively(((Component) obj.get_transform().GetChild(index)).get_gameObject(), status);
      obj.SetActive(status);
    }

    public static void EnableCollider(GameObject obj, bool status)
    {
      if (!Object.op_Implicit((Object) obj))
        return;
      foreach (Collider componentsInChild in (Collider[]) obj.GetComponentsInChildren<Collider>())
        componentsInChild.set_enabled(status);
    }

    public static void Destroy(Object obj, bool allowDestroyingAssets)
    {
      if (Application.get_isPlaying())
        Object.Destroy(obj);
      else
        Object.DestroyImmediate(obj, allowDestroyingAssets);
    }

    public static void SetCursorVisible(bool status)
    {
      Cursor.set_visible(status);
    }

    public static void LockCursor(bool status)
    {
      Cursor.set_lockState(!status ? (CursorLockMode) 2 : (CursorLockMode) 1);
    }

    public static bool IsCursorLocked()
    {
      return Cursor.get_lockState() == 1;
    }
  }
}
