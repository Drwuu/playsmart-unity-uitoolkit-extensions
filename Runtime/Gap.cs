using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Playsmart.UIToolkit
{
	[UxmlElement("Gap")]
	public partial class Gap : VisualElement
	{
		private static readonly CustomStyleProperty<float> s_GapProperty = new CustomStyleProperty<float>("--gap");
		private float _gapSize = 0f;

		public Gap()
		{
			RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
		}

		private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
		{
			if (evt.customStyle.TryGetValue(s_GapProperty, out float gap))
			{
				_gapSize = gap;
				UpdateGap();
			}
		}

		private void OnGeometryChanged(GeometryChangedEvent evt)
		{
			UpdateGap();
		}

		private void UpdateGap()
		{
			if (childCount == 0) return;

			// Gather only visible layout children (ignore display: none)
			List<VisualElement> visibleChildren = new List<VisualElement>();
			for (int i = 0; i < childCount; i++)
			{
				VisualElement child = this[i];
				if (child == null) continue;

				if (child.resolvedStyle.display == DisplayStyle.None)
				{
					ClearMargins(child);
				}
				else
				{
					visibleChildren.Add(child);
				}
			}

			if (visibleChildren.Count <= 1)
			{
				if (visibleChildren.Count == 1) ClearMargins(visibleChildren[0]);
				return;
			}

			FlexDirection direction = resolvedStyle.flexDirection;

			// Apply spacing only between visible sibling elements
			for (int i = 0; i < visibleChildren.Count; i++)
			{
				VisualElement child = visibleChildren[i];
				float targetMargin = (i == visibleChildren.Count - 1) ? 0f : _gapSize;

				ApplyTargetMargin(child, direction, targetMargin);
			}
		}

		private void ClearMargins(VisualElement element)
		{
			element.style.marginRight = StyleKeyword.Null;
			element.style.marginLeft = StyleKeyword.Null;
			element.style.marginTop = StyleKeyword.Null;
			element.style.marginBottom = StyleKeyword.Null;
		}

		private void ApplyTargetMargin(VisualElement child, FlexDirection direction, float targetMargin)
		{
			// Reset standard layout margins first to avoid conflicts
			ClearMargins(child);

			switch (direction)
			{
				case FlexDirection.Column:
					if (Mathf.Abs(child.resolvedStyle.marginBottom - targetMargin) > 0.01f)
						child.style.marginBottom = targetMargin;
					break;
				case FlexDirection.ColumnReverse:
					if (Mathf.Abs(child.resolvedStyle.marginTop - targetMargin) > 0.01f)
						child.style.marginTop = targetMargin;
					break;
				case FlexDirection.RowReverse:
					if (Mathf.Abs(child.resolvedStyle.marginLeft - targetMargin) > 0.01f)
						child.style.marginLeft = targetMargin;
					break;
				default: // FlexDirection.Row
					if (Mathf.Abs(child.resolvedStyle.marginRight - targetMargin) > 0.01f)
						child.style.marginRight = targetMargin;
					break;
			}
		}
	}
}
