using UnityEngine;
using Zenject;

namespace SiegeStorm.InteractSystem
{
    [RequireComponent(typeof(Outline))]
    public abstract class OutlineObject : InteractableObject
    {
        private ObjectInteractionView _visualData;
        private Outline _outline;

        protected override void Awake()
        {
            base.Awake();

            _outline = GetComponent<Outline>();
            HideOutline();

            ChangeOutlineColor(ObjectInteractionView.OutlineType.MouseStay);
        }

        public override void OnPointEnter()
        {
            ShowOutline();
        }

        public override void OnPointExit()
        {
            HideOutline();
        }

        public override void OnSelected()
        {
            _outline.OutlineWidth = _visualData.OutlineSizeOnSelect;
            ChangeOutlineColor(ObjectInteractionView.OutlineType.Selected);
        }

        public override void OnUnselected()
        {
            HideOutline();

            _outline.OutlineWidth = _visualData.OutlineSizeOnEnter;
            ChangeOutlineColor(ObjectInteractionView.OutlineType.MouseStay);
        }

        protected void ChangeOutlineColor(ObjectInteractionView.OutlineType type)
        {
            if (_outline == null || _visualData == null) return;
            _outline.OutlineColor = _visualData.OutlineColors[(int)type];
        }

        private void ShowOutline()
        {
            _outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
            _outline.OutlineWidth = _visualData.OutlineSizeOnEnter;
        }

        private void HideOutline()
        {
            _outline.OutlineMode = Outline.Mode.OutlineHidden;
            _outline.OutlineWidth = 0;
        }

        [Inject]
        public void Construct(ObjectInteractionView visualDataConfig)
        {
            _visualData = visualDataConfig;
        }
    }
}