// Decompiled with JetBrains decompiler
// Type: Studio.CheckScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class CheckScene : MonoBehaviour
  {
    [SerializeField]
    private Image back;
    [SerializeField]
    private VoiceNode buttonYes;
    [SerializeField]
    private VoiceNode buttonNo;
    private float timeScale;
    public static Sprite sprite;
    public static UnityAction unityActionYes;
    public static UnityAction unityActionNo;

    public CheckScene()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this.timeScale = Time.get_timeScale();
      Time.set_timeScale(0.0f);
    }

    private void Start()
    {
      this.back.set_sprite(CheckScene.sprite);
      this.buttonYes.addOnClick = CheckScene.unityActionYes;
      this.buttonNo.addOnClick = CheckScene.unityActionNo;
    }

    private void OnDestroy()
    {
      Time.set_timeScale(this.timeScale);
    }
  }
}
