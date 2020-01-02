// Decompiled with JetBrains decompiler
// Type: Trajectory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class Trajectory : MonoBehaviour
{
  private float fTimer;
  private float fAlpha;
  private static float fExistTime;
  private Image image;

  public Trajectory()
  {
    base.\u002Ector();
  }

  private void Start()
  {
  }

  public void Init(float fTime = 0.0f)
  {
    this.fAlpha = 1f;
    this.fTimer = 0.0f;
    if ((double) Trajectory.fExistTime != 0.0)
      return;
    Trajectory.fExistTime = fTime;
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.image, (Object) null))
      this.image = (Image) ((Component) this).get_gameObject().GetComponentInChildren<Image>();
    if (Object.op_Equality((Object) this.image, (Object) null) || !((Component) this).get_gameObject().get_activeSelf())
      return;
    this.fTimer += Time.get_unscaledDeltaTime();
    this.fAlpha -= 1f / Trajectory.fExistTime * Time.get_unscaledDeltaTime();
    Color color = ((Graphic) this.image).get_color();
    color.a = (__Null) (double) this.fAlpha;
    ((Graphic) this.image).set_color(color);
    if ((double) this.fTimer <= (double) Trajectory.fExistTime)
      return;
    this.fTimer = 0.0f;
    ((Component) this).get_gameObject().SetActive(false);
  }

  public void Set(Vector3 vSetPos, Quaternion qRot)
  {
    ((Component) this).get_transform().set_position(vSetPos);
    ((Component) this).get_transform().set_rotation(qRot);
    this.fTimer = 0.0f;
    this.fAlpha = 1f;
    ((Component) this).get_gameObject().SetActive(true);
  }

  public void Dead()
  {
    this.fTimer = 0.0f;
  }
}
