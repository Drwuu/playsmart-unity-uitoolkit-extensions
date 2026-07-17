using UnityEngine;
using UnityEngine.UIElements;

namespace Playsmart.UIToolkit
{
	[UxmlElement("SkewButton")]
	public partial class SkewButton : Button
	{
		private readonly SkewHelper _skewHelper = new SkewHelper();
		private readonly Label _label;

		public SkewButton()
		{
			generateVisualContent += OnGenerateVisualContent;
			RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);

			// Transparent background and zero border to prevent Unity's default button chrome.
			style.backgroundColor = Color.clear;
			style.borderLeftWidth = 0f;
			style.borderRightWidth = 0f;
			style.borderTopWidth = 0f;
			style.borderBottomWidth = 0f;
			style.color = Color.clear;

			// Child label renders text on top of the custom background mesh.
			_label = new Label();
			_label.style.position = Position.Absolute;
			_label.style.left = 0f;
			_label.style.right = 0f;
			_label.style.top = 0f;
			_label.style.bottom = 0f;
			_label.style.unityTextAlign = TextAnchor.MiddleCenter;
			_label.pickingMode = PickingMode.Ignore;
			Add(_label);

			// One-shot sync after UXML deserialization sets base.text
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
		}

		private void OnAttachToPanel(AttachToPanelEvent evt)
		{
			_label.text = base.text;
			UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
		}

		private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
		{
			_skewHelper.ResolveStyles(evt.customStyle);
			_label.style.color = _skewHelper.TextColor;
			MarkDirtyRepaint();
		}

		private void OnGenerateVisualContent(MeshGenerationContext mgc)
		{
			_skewHelper.GenerateVisualContent(mgc, contentRect);
		}

		public override bool ContainsPoint(Vector2 localPoint)
		{
			if (!base.ContainsPoint(localPoint)) return false;
			return _skewHelper.ContainsPoint(contentRect, localPoint);
		}
	}
}
