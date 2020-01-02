// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomRender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace CharaCustom
{
  public class CustomRender : MonoBehaviour
  {
    public bool update;

    public CustomRender()
    {
      base.\u002Ector();
    }

    public RenderTexture rtCamera { get; private set; }

    private void Start()
    {
      this.rtCamera = new RenderTexture(Screen.get_width(), Screen.get_height(), 24, (RenderTextureFormat) 0, (RenderTextureReadWrite) 0);
    }

    public RenderTexture GetRenderTexture()
    {
      return this.rtCamera;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
      if (this.update)
      {
        if (Object.op_Equality((Object) this.rtCamera, (Object) null))
        {
          Graphics.Blit((Texture) src, dst);
        }
        else
        {
          Graphics.Blit((Texture) src, this.rtCamera);
          Graphics.Blit((Texture) src, dst);
        }
      }
      else
        Graphics.Blit((Texture) this.rtCamera, dst);
    }

    private void OnDestroy()
    {
      if (!Object.op_Implicit((Object) this.rtCamera))
        return;
      this.rtCamera.Release();
      this.rtCamera = (RenderTexture) null;
    }
  }
}
