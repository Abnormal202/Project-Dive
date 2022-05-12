﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace Helpers {
    /// <summary>
    /// Static helper methods. Add whatever you want!
    /// </summary>
    class Helper {
        public static float ActualLerp(float a, float b, float t) {
            return a + (b - a) * t;
        }

        public static Vector3 ActualLerp(Vector3 a, Vector3 b, float t) {
            return new Vector3(ActualLerp(a.x, b.x, t), ActualLerp(a.y, b.y, t), ActualLerp(a.z, b.z, t));
        }

        //https://forum.unity.com/threads/debug-drawarrow.85980/
        public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f) {
            if (direction.Equals(Vector2.zero)) {
                return;
            }

            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                            new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                           new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction / 2, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction / 2, left * arrowHeadLength);
        }

        public static String DictToString<Key, Val>(Dictionary<Key, Val> d) {
            return "{" + string.Join(",", d.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }

        public static float RoundHalf(double value) {
            return (float) (Math.Round(value * 2) / 2);
        }

        public static Vector2 RoundVectorHalf(Vector2 v) {
            return new Vector2(RoundHalf(v.x), RoundHalf(v.y));
        }

        /// <summary>
        /// At a given position, casts a vector up and down the z axis to find Components of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="pos">Position</param>
        /// <typeparam name="T">Component</typeparam>
        /// <returns>Component to find</returns>
        public static T OnComponent<T>(Vector3 pos) where T : MonoBehaviour {
            RaycastHit2D[] found = Physics2D.RaycastAll(
                pos,
                new Vector3(0, 0, 1),
                LayerMask.GetMask("Interactable")
            );
            T highestComponent = default(T);
            foreach (RaycastHit2D curCol in found) {
                T interact = curCol.transform.GetComponent<T>();
                if (interact != null) {
                    if (highestComponent == null || interact.transform.position.z < highestComponent.transform.position.z) {
                        highestComponent = interact;
                    }
                }
            }

            return highestComponent;
        }

        public static List<T> OnComponents<T>(Vector3 pos) where T : MonoBehaviour {
            RaycastHit2D[] found = Physics2D.RaycastAll(
                pos,
                new Vector3(0, 0, 1),
                LayerMask.GetMask("Interactable")
            );
            List<T> components = new List<T>();
            foreach (RaycastHit2D curCol in found) {
                components.Add(curCol.transform.GetComponent<T>());
            }

            return components;
        }

        public static double Clamp(double low, double high, double a) {
            return Math.Min(Math.Max(a, low), high);
        }

        public static Color ColorLerp(Color a, Color b, float t) {
            Color c = Color.Lerp(a, b, t);
            c.a = ActualLerp(a.a, b.a, t);
            return c;
        }
    
        public static IEnumerator Fade(SpriteRenderer sr, float time, Color newColor, Action<int> done = null) {
            Color origColor = sr.color;
            for (float ft = 0; ft < time; ft += Game.IsPaused ? 0 : Time.deltaTime) {
                sr.color = ColorLerp(origColor, newColor, ft/time);
                yield return null;
            }
            if (done != null) done(0);
        }

        public static IEnumerator Shake(Transform t, float duration=0.5f, float speed=100f, float amount=0.05f) {
            Vector3 origPos = t.localPosition;
            for (float ft = 0; ft < duration; ft += Game.IsPaused ? 0 : Time.deltaTime) {
                t.localPosition = origPos + new Vector3(Mathf.Sin(ft/duration * speed) * amount, Mathf.Sin(ft * speed) * amount);
                yield return null;
            }

            t.localPosition = origPos;
        }

        public static void DrawText(Vector3 pos, string text, Color col = default) {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = col == default ? Color.white : col; 
            Handles.Label(pos, text, style);
        }
    }
}