// Decompiled with JetBrains decompiler
// Type: ImageHelpScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImageHelpScene : MonoBehaviour
{
  public static bool isAdd = true;
  [SerializeField]
  private GraphicRaycaster graphicRaycaster;
  [SerializeField]
  private Image imageHelp;
  [SerializeField]
  private Sprite[] sprite;
  private UnityAction action;
  public static ImageHelpScene.HelpKind kind;

  public ImageHelpScene()
  {
    base.\u002Ector();
  }

  private void Start()
  {
  }

  private void Update()
  {
  }

  public enum HelpKind
  {
    Custom,
    H,
    Game,
  }
}
