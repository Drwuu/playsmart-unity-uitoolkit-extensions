using UnityEngine;
using UnityEngine.UIElements;

namespace Playsmart.UIToolkit
{
	[UxmlElement("SkewButton")]
	public partial class SkewButton : Button
	{
		private static readonly CustomStyleProperty<float> s_Skew = new CustomStyleProperty<float>("--skew");
		private static readonly CustomStyleProperty<float> s_SkewSize = new CustomStyleProperty<float>("--skew-size");
		private static readonly CustomStyleProperty<float> s_SkewAngle = new CustomStyleProperty<float>("--skew-angle");
		private static readonly CustomStyleProperty<Color> s_FillColor = new CustomStyleProperty<Color>("--skew-fill-color");
		private static readonly CustomStyleProperty<Color> s_StrokeColor = new CustomStyleProperty<Color>("--skew-stroke-color");
		private static readonly CustomStyleProperty<float> s_StrokeWidth = new CustomStyleProperty<float>("--skew-stroke-width");

		private float _skewSize = 10f;
		private float _skewAngle = 0f;
		private Color _fillColor = new Color(0.1f, 0.05f, 0.2f, 0.8f);
		private Color _strokeColor = new Color(0f, 0.94f, 1f, 0.3f);
		private float _strokeWidth = 1f;

		public SkewButton()
		{
			generateVisualContent += OnGenerateVisualContent;
			RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
		}

		private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
		{
			var customStyle = evt.customStyle;
			
			if (customStyle.TryGetValue(s_Skew, out float skew)) _skewSize = skew;
			if (customStyle.TryGetValue(s_SkewSize, out float size)) _skewSize = size;
			if (customStyle.TryGetValue(s_SkewAngle, out float angle)) _skewAngle = angle;
			if (customStyle.TryGetValue(s_FillColor, out Color fill)) _fillColor = fill;
			if (customStyle.TryGetValue(s_StrokeColor, out Color stroke)) _strokeColor = stroke;
			if (customStyle.TryGetValue(s_StrokeWidth, out float strokeWidth)) _strokeWidth = strokeWidth;

			MarkDirtyRepaint();
		}

		private void OnGenerateVisualContent(MeshGenerationContext mgc)
		{
			var painter = mgc.painter2D;
			var rect = contentRect;

			if (rect.width <= 0 || rect.height <= 0) return;

			painter.strokeColor = _strokeColor;
			painter.fillColor = _fillColor;
			painter.lineWidth = _strokeWidth;

			// Calculate slant offset based on size or angle
			float slantOffset = _skewSize;
			if (_skewAngle != 0)
			{
				float clampedAngle = Mathf.Clamp(_skewAngle, -85f, 85f);
				slantOffset = rect.height * Mathf.Tan(clampedAngle * Mathf.Deg2Rad);
			}

			// Calculate top and bottom X boundaries to keep the shape contained within bounds
			float leftTop, rightTop, leftBottom, rightBottom;
			if (slantOffset >= 0)
			{
				leftTop = slantOffset;
				rightTop = rect.width;
				leftBottom = 0;
				rightBottom = rect.width - slantOffset;
			}
			else
			{
				leftTop = 0;
				rightTop = rect.width + slantOffset;
				leftBottom = -slantOffset;
				rightBottom = rect.width;
			}

			// Draw the procedurally skewed parallelogram
			painter.BeginPath();
			painter.MoveTo(new Vector2(leftTop + _strokeWidth / 2f, _strokeWidth / 2f));
			painter.LineTo(new Vector2(rightTop - _strokeWidth / 2f, _strokeWidth / 2f));
			painter.LineTo(new Vector2(rightBottom - _strokeWidth / 2f, rect.height - _strokeWidth / 2f));
			painter.LineTo(new Vector2(leftBottom + _strokeWidth / 2f, rect.height - _strokeWidth / 2f));
			painter.ClosePath();

			painter.Fill();
			if (_strokeWidth > 0 && _strokeColor.a > 0)
			{
				painter.Stroke();
			}
		}

		public override bool ContainsPoint(Vector2 localPoint)
		{
			if (!base.ContainsPoint(localPoint)) return false;

			float rectHeight = contentRect.height;
			float rectWidth = contentRect.width;

			if (rectHeight <= 0 || rectWidth <= 0) return false;

			float slantOffset = _skewSize;
			if (_skewAngle != 0)
			{
				float clampedAngle = Mathf.Clamp(_skewAngle, -85f, 85f);
				slantOffset = rectHeight * Mathf.Tan(clampedAngle * Mathf.Deg2Rad);
			}

			// Top/Bottom left X-coordinates
			float leftTop = slantOffset >= 0 ? slantOffset : 0;
			float leftBottom = slantOffset >= 0 ? 0 : -slantOffset;

			// Linearly interpolate left boundary edge at current Y coordinate
			float t = localPoint.y / rectHeight;
			float minX = Mathf.Lerp(leftTop, leftBottom, t);
			float maxX = minX + (rectWidth - Mathf.Abs(slantOffset));

			return localPoint.x >= minX && localPoint.x <= maxX;
		}
	}
}
