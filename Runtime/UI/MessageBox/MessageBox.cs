using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#region
//保持UTF-8
#endregion
namespace Saber.UI {
    public static class MessageBox
    {
        static string messageBoxPrefab = "MessageBox";

        static float movePara = 100f;
        public static async void ShowMessage(string content, float continueTime, Vector2 moveDir, Vector2 pos = default)
        {
            var box = Resources.Load<GameObject>(messageBoxPrefab);
            box = GameObject.Instantiate(box, UIManager.Instance.MainCanvas.transform);
            box.transform.SetAsLastSibling();
            box.GetComponentInChildren<TMP_Text>().text = content;
            var rt = ((RectTransform)box.transform);

            if (moveDir != Vector2.zero)
            {
                DOAnchorPos(rt,rt.anchoredPosition + moveDir.normalized * continueTime * movePara, continueTime);
            }
            if (pos != default)
                rt.anchoredPosition = rt.InverseTransformPoint(pos);
            await UniTask.Delay((int)(continueTime * 1000));
            box.transform.DOKill();
            GameObject.Destroy(box.gameObject);
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorPos(RectTransform target, Vector2 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.anchoredPosition, x => target.anchoredPosition = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
    }
}


