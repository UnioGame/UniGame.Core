namespace UniGame.Utils.Runtime
{
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererParent : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private float _alpha;
        private SpriteRenderer[] _children;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _alpha = _renderer.color.a;

            OnTransformChildrenChanged();

        }

        public void FixedUpdate()
        {
            if(_renderer == null)
                return;
            
            if (Mathf.Approximately(_renderer.color.a, _alpha)) 
                return;
            
            _alpha = _renderer.color.a;
            UpdateChildrenTransparency();
        }

        private void OnTransformChildrenChanged()
        {
            _children = GetComponentsInChildren<SpriteRenderer>();
        }

        private void UpdateChildrenTransparency()
        {
            foreach (var spriteRenderer in _children)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, _alpha);
            }
        }
    }
}