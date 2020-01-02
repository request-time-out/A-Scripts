// Decompiled with JetBrains decompiler
// Type: AIProject.CameraConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace AIProject
{
  public class CameraConfig : MonoBehaviour
  {
    [SerializeField]
    private PostProcessProfile _ppProfile;
    [SerializeField]
    private PostProcessLayer _ppLayer;
    [SerializeField]
    private EnviroSky _enviroSky;

    public CameraConfig()
    {
      base.\u002Ector();
    }

    public PostProcessProfile PPProfile
    {
      get
      {
        return this._ppProfile;
      }
    }

    public PostProcessLayer PPLayer
    {
      get
      {
        return this._ppLayer;
      }
    }

    public EnviroSky EnviroSky
    {
      get
      {
        return this._enviroSky;
      }
    }

    private void Reset()
    {
    }
  }
}
