// Decompiled with JetBrains decompiler
// Type: LookHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class LookHit : MonoBehaviour
{
  private bool isNowDragging;

  public LookHit()
  {
    base.\u002Ector();
  }

  public bool IsNowDragging
  {
    get
    {
      return this.isNowDragging;
    }
  }

  private void Start()
  {
  }

  private void Update()
  {
  }

  private void OnMouseDown()
  {
    Debug.Log((object) "ClickDown");
    this.isNowDragging = true;
  }

  private void OnMouseUp()
  {
    Debug.Log((object) "ClickUp");
    this.isNowDragging = false;
  }

  private void OnCollisionEnter(Collision col)
  {
    Debug.Log((object) ("col_enter:" + ((Object) col.get_gameObject()).get_name()));
  }

  private void OnCollisionExit(Collision col)
  {
    Debug.Log((object) ("col_end:" + ((Object) col.get_gameObject()).get_name()));
  }

  private void OnCollisionStay(Collision col)
  {
    Debug.Log((object) ("col_stay:" + ((Object) col.get_gameObject()).get_name()));
  }

  private void OnTriggerEnter(Collider col)
  {
    if (!(((Component) col).get_gameObject().get_tag() == "CollDels"))
      return;
    ((Renderer) ((Component) col).get_gameObject().GetComponent<Renderer>()).set_enabled(false);
  }

  private void OnTriggerExit(Collider col)
  {
    if (!(((Component) col).get_gameObject().get_tag() == "CollDels"))
      return;
    ((Renderer) ((Component) col).get_gameObject().GetComponent<Renderer>()).set_enabled(true);
  }

  private void OnTriggerStay(Collider col)
  {
    if (!(((Object) ((Component) col).get_gameObject()).get_name() == "Floor"))
      return;
    Debug.Log((object) "Hit Floor tag object");
  }
}
