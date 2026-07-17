using UnityEngine;
using UnityEngine.UIElements;

namespace Playsmart.UIToolkit
{
	public class SkewHelper
	{
		public static readonly CustomStyleProperty<float> s_Skew = new CustomStyleProperty<float>("--skew");
		public static readonly CustomStyleProperty<float> s_SkewSize = new CustomStyleProperty<float>("--skew-size");
		public static readonly CustomStyleProperty<float> s_SkewAngle = new CustomStyleProperty<float>("--skew-angle");
		public static readonly CustomStyleProperty<Color> s_FillColor = new CustomStyleProperty<Color>("--skew-fill-color");
		public static readonly CustomStyleProperty<Color> s_StrokeColor = new CustomStyleProperty<Color>("--skew-stroke-color");
		public static readonly CustomStyleProperty<float> s_StrokeWidth = new CustomStyleProperty<float>("--skew-stroke-width");
		public static readonly CustomStyleProperty<Color> s_TextColor = new CustomStyleProperty<Color>("--skew-text-color");

		public float SkewSize = 10f;
		public float SkewAngle = 0f;
		public Color FillColor = new Color(0.1f, 0.05f, 0.2f, 0.8f);
		public Color StrokeColor = new Color(0f, 0.94f, 1f, 0.3f);
		public float StrokeWidth = 1f;
		public Color TextColor = Color.white;

		public void ResolveStyles(ICustomStyle customStyle)
		{
			if (customStyle.TryGetValue(s_Skew, out float skew)) SkewSize = skew;
			if (customStyle.TryGetValue(s_SkewSize, out float size)) SkewSize = size;
			if (customStyle.TryGetValue(s_SkewAngle, out float angle)) SkewAngle = angle;
			if (customStyle.TryGetValue(s_FillColor, out Color fill)) FillColor = fill;
			if (customStyle.TryGetValue(s_StrokeColor, out Color stroke)) StrokeColor = stroke;
			if (customStyle.TryGetValue(s_StrokeWidth, out float strokeWidth)) StrokeWidth = strokeWidth;

			if (customStyle.TryGetValue(s_TextColor, out Color textColor)) TextColor = textColor;
			else TextColor = Color.white;
		}

		public void GenerateVisualContent(MeshGenerationContext mgc, Rect rect)
		{
			var painter = mgc.painter2D;

			if (rect.width <= 0 || rect.height <= 0) return;

			painter.strokeColor = StrokeColor;
			painter.fillColor = FillColor;
			painter.lineWidth = StrokeWidth;

			// Calculate slant offset based on size or angle
			float slantOffset = SkewSize;
			if (SkewAngle != 0)
			{
				float clampedAngle = Mathf.Clamp(SkewAngle, -85f, 85f);
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
			painter.MoveTo(new Vector2(leftTop + StrokeWidth / 2f, StrokeWidth / 2f));
			painter.LineTo(new Vector2(rightTop - StrokeWidth / 2f, StrokeWidth / 2f));
			painter.LineTo(new Vector2(rightBottom - StrokeWidth / 2f, rect.height - StrokeWidth / 2f));
			painter.LineTo(new Vector2(leftBottom + StrokeWidth / 2f, rect.height - StrokeWidth / 2f));
			painter.ClosePath();

			painter.Fill();
			if (StrokeWidth > 0 && StrokeColor.a > 0)
			{
				painter.Stroke();
			}
		}

		public bool ContainsPoint(Rect rect, Vector2 localPoint)
		{
			float rectHeight = rect.height;
			float rectWidth = rect.width;

			if (rectHeight <= 0 || rectWidth <= 0) return false;

			float slantOffset = SkewSize;
			if (SkewAngle != 0)
			{
				float clampedAngle = Mathf.Clamp(SkewAngle, -85f, 85f);
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
