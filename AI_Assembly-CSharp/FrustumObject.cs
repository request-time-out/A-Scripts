// Decompiled with JetBrains decompiler
// Type: FrustumObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FrustumObject : CollisionCamera
{
  private new void Start()
  {
    base.Start();
  }

  private void Update()
  {
    this.SetCollision();
    Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes((Camera) ((Component) this).GetComponent<Camera>());
    foreach (GameObject objDel in this.objDels)
    {
      List<GameObject> list = new List<GameObject>();
      objDel.get_transform().get_parent().FindLoopAll(list);
      if (!Object.op_Equality((Object) objDel.GetComponent<Collider>(), (Object) null) && !Object.op_Equality((Object) objDel.GetComponent<Renderer>(), (Object) null))
      {
        using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GameObject current = enumerator.Current;
            if (Object.op_Implicit((Object) current.GetComponent<Renderer>()))
              ((Renderer) current.GetComponent<Renderer>()).set_enabled(true);
          }
        }
        if (GeometryUtility.TestPlanesAABB(frustumPlanes, ((Collider) objDel.GetComponent<Collider>()).get_bounds()))
        {
          float num1 = Vector3.Distance(this.camCtrl.TargetPos, ((Component) this).get_transform().get_position());
          Bounds bounds = ((Collider) objDel.GetComponent<Collider>()).get_bounds();
          float num2 = Vector3.Distance(((Bounds) ref bounds).get_center(), ((Component) this).get_transform().get_position());
          if ((double) num1 > (double) num2)
          {
            using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GameObject current = enumerator.Current;
                if (Object.op_Implicit((Object) current.GetComponent<Renderer>()))
                  ((Renderer) current.GetComponent<Renderer>()).set_enabled(false);
              }
            }
          }
        }
      }
    }
  }

  private void OnGUI()
  {
    StringBuilder stringBuilder = new StringBuilder();
    float num1 = 1000f;
    int num2 = 0;
    foreach (GameObject objDel in this.objDels)
    {
      if (Object.op_Inequality((Object) objDel.GetComponent<Renderer>(), (Object) null) && (!((Renderer) objDel.GetComponent<Renderer>()).get_enabled() || !objDel.get_activeSelf()))
        ++num2;
    }
    stringBuilder.Append("Count:" + num2.ToString());
    stringBuilder.Append("\n");
    foreach (GameObject objDel in this.objDels)
    {
      if (Object.op_Inequality((Object) objDel.GetComponent<Renderer>(), (Object) null) && (!((Renderer) objDel.GetComponent<Renderer>()).get_enabled() || !objDel.get_activeSelf()))
      {
        stringBuilder.Append(((Object) objDel).get_name());
        stringBuilder.Append("\n");
      }
    }
    GUI.Box(new Rect(5f, 5f, 300f, num1), string.Empty);
    GUI.Label(new Rect(10f, 5f, 1000f, num1), stringBuilder.ToString());
  }

  public object[] GetObjects(Vector3 position, float distance, float fov, Vector3 direction)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    Object[] objectsOfType = Object.FindObjectsOfType(typeof (GameObject));
    ((Component) this).get_transform().set_position(position);
    ((Component) this).get_transform().set_forward(direction);
    ((Camera) ((Component) this).GetComponent<Camera>()).set_fieldOfView(fov);
    ((Camera) ((Component) this).GetComponent<Camera>()).set_farClipPlane(distance);
    Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes((Camera) ((Component) this).GetComponent<Camera>());
    foreach (GameObject gameObject in objectsOfType)
    {
      if (!Object.op_Equality((Object) gameObject.GetComponent<Collider>(), (Object) null) && GeometryUtility.TestPlanesAABB(frustumPlanes, ((Collider) gameObject.GetComponent<Collider>()).get_bounds()))
        gameObjectList.Add(gameObject);
    }
    return (object[]) gameObjectList.ToArray();
  }

  private bool IsLook(Vector3 pos)
  {
    Vector3 viewportPoint = ((Camera) ((Component) this).GetComponent<Camera>()).WorldToViewportPoint(pos);
    return viewportPoint.x < -0.5 || viewportPoint.x > 1.5 || (viewportPoint.y < -0.5 || viewportPoint.y > 1.5);
  }
}
