// Decompiled with JetBrains decompiler
// Type: Studio.AssociateSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace Studio
{
  public class AssociateSound : MonoBehaviour
  {
    [SerializeField]
    private AudioSource m_AudioSource;

    public AssociateSound()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (Object.op_Inequality((Object) this.m_AudioSource, (Object) null))
        this.m_AudioSource.set_outputAudioMixerGroup(Sound.Mixer.FindMatchingGroups("GameSE")[0]);
      Object.Destroy((Object) this);
    }
  }
}
