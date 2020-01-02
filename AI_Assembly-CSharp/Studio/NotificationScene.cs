// Decompiled with JetBrains decompiler
// Type: Studio.NotificationScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class NotificationScene : MonoBehaviour
  {
    public static float waitTime = 1f;
    public static float width = 416f;
    public static float height = 48f;
    [SerializeField]
    private Image imageMessage;
    [SerializeField]
    private RectTransform transImage;
    public static Sprite spriteMessage;

    public NotificationScene()
    {
      base.\u002Ector();
    }

    [DebuggerHidden]
    private IEnumerator NotificationCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NotificationScene.\u003CNotificationCoroutine\u003Ec__Iterator0 coroutineCIterator0 = new NotificationScene.\u003CNotificationCoroutine\u003Ec__Iterator0();
      return (IEnumerator) coroutineCIterator0;
    }

    private void Awake()
    {
      this.imageMessage.set_sprite(NotificationScene.spriteMessage);
      this.transImage.set_sizeDelta(new Vector2(NotificationScene.width, NotificationScene.height));
      this.StartCoroutine(this.NotificationCoroutine());
      NotificationScene.width = 416f;
      NotificationScene.height = 48f;
    }
  }
}
