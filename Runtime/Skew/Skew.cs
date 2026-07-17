using UnityEngine;
using UnityEngine.UIElements;

namespace Playsmart.UIToolkit
{
	[UxmlElement("Skew")]
	public partial class Skew : VisualElement
	{
		private readonly SkewHelper _skewHelper = new SkewHelper();

		public Skew()
		{
			generateVisualContent += OnGenerateVisualContent;
			RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
		}

		private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
		{
			_skewHelper.ResolveStyles(evt.customStyle);
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
