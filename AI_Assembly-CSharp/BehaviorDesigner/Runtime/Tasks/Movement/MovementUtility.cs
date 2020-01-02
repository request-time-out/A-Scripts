// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.MovementUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  public static class MovementUtility
  {
    private static Dictionary<GameObject, Dictionary<Type, Component>> gameObjectComponentMap = new Dictionary<GameObject, Dictionary<Type, Component>>();
    private static Dictionary<GameObject, Dictionary<Type, Component[]>> gameObjectComponentsMap = new Dictionary<GameObject, Dictionary<Type, Component[]>>();

    public static GameObject WithinSight(
      Transform transform,
      Vector3 positionOffset,
      float fieldOfViewAngle,
      float viewDistance,
      LayerMask objectLayerMask,
      Vector3 targetOffset,
      LayerMask ignoreLayerMask,
      bool useTargetBone,
      HumanBodyBones targetBone)
    {
      GameObject gameObject1 = (GameObject) null;
      Collider[] colliderArray = Physics.OverlapSphere(transform.get_position(), viewDistance, LayerMask.op_Implicit(objectLayerMask));
      if (colliderArray != null)
      {
        float num = float.PositiveInfinity;
        for (int index = 0; index < colliderArray.Length; ++index)
        {
          float angle;
          GameObject gameObject2;
          if (Object.op_Inequality((Object) (gameObject2 = MovementUtility.WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, ((Component) colliderArray[index]).get_gameObject(), targetOffset, false, 0.0f, out angle, LayerMask.op_Implicit(ignoreLayerMask), useTargetBone, targetBone)), (Object) null) && (double) angle < (double) num)
          {
            num = angle;
            gameObject1 = gameObject2;
          }
        }
      }
      return gameObject1;
    }

    public static GameObject WithinSight2D(
      Transform transform,
      Vector3 positionOffset,
      float fieldOfViewAngle,
      float viewDistance,
      LayerMask objectLayerMask,
      Vector3 targetOffset,
      float angleOffset2D,
      LayerMask ignoreLayerMask)
    {
      GameObject gameObject1 = (GameObject) null;
      Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(Vector2.op_Implicit(transform.get_position()), viewDistance, LayerMask.op_Implicit(objectLayerMask));
      if (collider2DArray != null)
      {
        float num = float.PositiveInfinity;
        for (int index = 0; index < collider2DArray.Length; ++index)
        {
          float angle;
          GameObject gameObject2;
          if (Object.op_Inequality((Object) (gameObject2 = MovementUtility.WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, ((Component) collider2DArray[index]).get_gameObject(), targetOffset, true, angleOffset2D, out angle, LayerMask.op_Implicit(ignoreLayerMask), false, (HumanBodyBones) 0)), (Object) null) && (double) angle < (double) num)
          {
            num = angle;
            gameObject1 = gameObject2;
          }
        }
      }
      return gameObject1;
    }

    public static GameObject WithinSight(
      Transform transform,
      Vector3 positionOffset,
      float fieldOfViewAngle,
      float viewDistance,
      GameObject targetObject,
      Vector3 targetOffset,
      LayerMask ignoreLayerMask,
      bool useTargetBone,
      HumanBodyBones targetBone)
    {
      return MovementUtility.WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, false, 0.0f, out float _, LayerMask.op_Implicit(ignoreLayerMask), useTargetBone, targetBone);
    }

    public static GameObject WithinSight2D(
      Transform transform,
      Vector3 positionOffset,
      float fieldOfViewAngle,
      float viewDistance,
      GameObject targetObject,
      Vector3 targetOffset,
      float angleOffset2D,
      LayerMask ignoreLayerMask,
      bool useTargetBone,
      HumanBodyBones targetBone)
    {
      return MovementUtility.WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, true, angleOffset2D, out float _, LayerMask.op_Implicit(ignoreLayerMask), useTargetBone, targetBone);
    }

    public static GameObject WithinSight(
      Transform transform,
      Vector3 positionOffset,
      float fieldOfViewAngle,
      float viewDistance,
      GameObject targetObject,
      Vector3 targetOffset,
      bool usePhysics2D,
      float angleOffset2D,
      out float angle,
      int ignoreLayerMask,
      bool useTargetBone,
      HumanBodyBones targetBone)
    {
      if (Object.op_Equality((Object) targetObject, (Object) null))
      {
        angle = 0.0f;
        return (GameObject) null;
      }
      Animator componentForType;
      if (useTargetBone && Object.op_Inequality((Object) (componentForType = MovementUtility.GetComponentForType<Animator>(targetObject)), (Object) null))
      {
        Transform boneTransform = componentForType.GetBoneTransform(targetBone);
        if (Object.op_Inequality((Object) boneTransform, (Object) null))
          targetObject = ((Component) boneTransform).get_gameObject();
      }
      Vector3 vector3 = Vector3.op_Subtraction(targetObject.get_transform().get_position(), transform.TransformPoint(positionOffset));
      if (usePhysics2D)
      {
        Vector3 eulerAngles = transform.get_eulerAngles();
        ref Vector3 local = ref eulerAngles;
        local.z = (__Null) (local.z - (double) angleOffset2D);
        angle = Vector3.Angle(vector3, Quaternion.op_Multiply(Quaternion.Euler(eulerAngles), Vector3.get_up()));
        vector3.z = (__Null) 0.0;
      }
      else
      {
        angle = Vector3.Angle(vector3, transform.get_forward());
        vector3.y = (__Null) 0.0;
      }
      return (double) ((Vector3) ref vector3).get_magnitude() < (double) viewDistance && (double) angle < (double) fieldOfViewAngle * 0.5 && (Object.op_Inequality((Object) MovementUtility.LineOfSight(transform, positionOffset, targetObject, targetOffset, usePhysics2D, ignoreLayerMask), (Object) null) || Object.op_Equality((Object) MovementUtility.GetComponentForType<Collider>(targetObject), (Object) null) && Object.op_Equality((Object) MovementUtility.GetComponentForType<Collider2D>(targetObject), (Object) null) && targetObject.get_gameObject().get_activeSelf()) ? targetObject : (GameObject) null;
    }

    public static GameObject LineOfSight(
      Transform transform,
      Vector3 positionOffset,
      GameObject targetObject,
      Vector3 targetOffset,
      bool usePhysics2D,
      int ignoreLayerMask)
    {
      if (usePhysics2D)
      {
        RaycastHit2D raycastHit2D;
        if (RaycastHit2D.op_Implicit(raycastHit2D = Physics2D.Linecast(Vector2.op_Implicit(transform.TransformPoint(positionOffset)), Vector2.op_Implicit(targetObject.get_transform().TransformPoint(targetOffset)), ~ignoreLayerMask)) && (((RaycastHit2D) ref raycastHit2D).get_transform().IsChildOf(targetObject.get_transform()) || targetObject.get_transform().IsChildOf(((RaycastHit2D) ref raycastHit2D).get_transform())))
          return targetObject;
      }
      else
      {
        RaycastHit raycastHit;
        if (Physics.Linecast(transform.TransformPoint(positionOffset), targetObject.get_transform().TransformPoint(targetOffset), ref raycastHit, ~ignoreLayerMask) && (((RaycastHit) ref raycastHit).get_transform().IsChildOf(targetObject.get_transform()) || targetObject.get_transform().IsChildOf(((RaycastHit) ref raycastHit).get_transform())))
          return targetObject;
      }
      return (GameObject) null;
    }

    public static GameObject WithinHearingRange(
      Transform transform,
      Vector3 positionOffset,
      float audibilityThreshold,
      float hearingRadius,
      LayerMask objectLayerMask)
    {
      GameObject gameObject1 = (GameObject) null;
      Collider[] colliderArray = Physics.OverlapSphere(transform.TransformPoint(positionOffset), hearingRadius, LayerMask.op_Implicit(objectLayerMask));
      if (colliderArray != null)
      {
        float num = 0.0f;
        for (int index = 0; index < colliderArray.Length; ++index)
        {
          float audibility = 0.0f;
          GameObject gameObject2;
          if (Object.op_Inequality((Object) (gameObject2 = MovementUtility.WithinHearingRange(transform, positionOffset, audibilityThreshold, ((Component) colliderArray[index]).get_gameObject(), ref audibility)), (Object) null) && (double) audibility > (double) num)
          {
            num = audibility;
            gameObject1 = gameObject2;
          }
        }
      }
      return gameObject1;
    }

    public static GameObject WithinHearingRange2D(
      Transform transform,
      Vector3 positionOffset,
      float audibilityThreshold,
      float hearingRadius,
      LayerMask objectLayerMask)
    {
      GameObject gameObject1 = (GameObject) null;
      Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(Vector2.op_Implicit(transform.TransformPoint(positionOffset)), hearingRadius, LayerMask.op_Implicit(objectLayerMask));
      if (collider2DArray != null)
      {
        float num = 0.0f;
        for (int index = 0; index < collider2DArray.Length; ++index)
        {
          float audibility = 0.0f;
          GameObject gameObject2;
          if (Object.op_Inequality((Object) (gameObject2 = MovementUtility.WithinHearingRange(transform, positionOffset, audibilityThreshold, ((Component) collider2DArray[index]).get_gameObject(), ref audibility)), (Object) null) && (double) audibility > (double) num)
          {
            num = audibility;
            gameObject1 = gameObject2;
          }
        }
      }
      return gameObject1;
    }

    public static GameObject WithinHearingRange(
      Transform transform,
      Vector3 positionOffset,
      float audibilityThreshold,
      GameObject targetObject)
    {
      float audibility = 0.0f;
      return MovementUtility.WithinHearingRange(transform, positionOffset, audibilityThreshold, targetObject, ref audibility);
    }

    public static GameObject WithinHearingRange(
      Transform transform,
      Vector3 positionOffset,
      float audibilityThreshold,
      GameObject targetObject,
      ref float audibility)
    {
      AudioSource[] componentsForType;
      if ((componentsForType = MovementUtility.GetComponentsForType<AudioSource>(targetObject)) != null)
      {
        for (int index = 0; index < componentsForType.Length; ++index)
        {
          if (componentsForType[index].get_isPlaying())
          {
            float num = Vector3.Distance(transform.get_position(), targetObject.get_transform().get_position());
            audibility = componentsForType[index].get_rolloffMode() != null ? componentsForType[index].get_volume() * Mathf.Clamp01((float) (((double) num - (double) componentsForType[index].get_minDistance()) / ((double) componentsForType[index].get_maxDistance() - (double) componentsForType[index].get_minDistance()))) : componentsForType[index].get_volume() / Mathf.Max(componentsForType[index].get_minDistance(), num - componentsForType[index].get_minDistance());
            if ((double) audibility > (double) audibilityThreshold)
              return targetObject;
          }
        }
      }
      return (GameObject) null;
    }

    public static void DrawLineOfSight(
      Transform transform,
      Vector3 positionOffset,
      float fieldOfViewAngle,
      float angleOffset,
      float viewDistance,
      bool usePhysics2D)
    {
    }

    public static T GetComponentForType<T>(GameObject target) where T : Component
    {
      Dictionary<Type, Component> dictionary;
      if (MovementUtility.gameObjectComponentMap.TryGetValue(target, out dictionary))
      {
        Component component;
        if (dictionary.TryGetValue(typeof (T), out component))
          return component as T;
      }
      else
      {
        dictionary = new Dictionary<Type, Component>();
        MovementUtility.gameObjectComponentMap.Add(target, dictionary);
      }
      Component component1 = (Component) (object) target.GetComponent<T>();
      dictionary.Add(typeof (T), component1);
      return component1 as T;
    }

    public static T[] GetComponentsForType<T>(GameObject target) where T : Component
    {
      Dictionary<Type, Component[]> dictionary;
      if (MovementUtility.gameObjectComponentsMap.TryGetValue(target, out dictionary))
      {
        Component[] componentArray;
        if (dictionary.TryGetValue(typeof (T), out componentArray))
          return componentArray as T[];
      }
      else
      {
        dictionary = new Dictionary<Type, Component[]>();
        MovementUtility.gameObjectComponentsMap.Add(target, dictionary);
      }
      Component[] components = (Component[]) target.GetComponents<T>();
      dictionary.Add(typeof (T), components);
      return components as T[];
    }

    public static void ClearCache()
    {
      MovementUtility.gameObjectComponentMap.Clear();
      MovementUtility.gameObjectComponentsMap.Clear();
    }
  }
}
