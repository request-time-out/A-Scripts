// Decompiled with JetBrains decompiler
// Type: Studio.FrameCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class FrameCtrl : MonoBehaviour
  {
    [SerializeField]
    private Camera cameraUI;
    [SerializeField]
    private RawImage imageFrame;

    public FrameCtrl()
    {
      base.\u002Ector();
    }

    public bool Load(string _file)
    {
      this.Release();
      string str = UserData.Path + "frame/" + _file;
      if (!File.Exists(str))
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.frame = string.Empty;
        return false;
      }
      Texture texture = (Texture) PngAssist.LoadTexture(str);
      if (Object.op_Equality((Object) texture, (Object) null))
        return false;
      this.imageFrame.set_texture(texture);
      ((Behaviour) this.imageFrame).set_enabled(true);
      ((Behaviour) this.cameraUI).set_enabled(true);
      Singleton<Studio.Studio>.Instance.sceneInfo.frame = _file;
      Resources.UnloadUnusedAssets();
      GC.Collect();
      return true;
    }

    public void Release()
    {
      Object.Destroy((Object) this.imageFrame.get_texture());
      this.imageFrame.set_texture((Texture) null);
      ((Behaviour) this.imageFrame).set_enabled(false);
      ((Behaviour) this.cameraUI).set_enabled(false);
      Resources.UnloadUnusedAssets();
    }
  }
}
