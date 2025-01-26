using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TsMap {
    public abstract class TsPrefabLook {
        public int ZIndex { get; set; }
        public Brush Color { get; set; }
        protected readonly List<PointF> Points;

        protected TsPrefabLook(List<PointF> points) {
            Points = points;
        }

        protected TsPrefabLook() : this(new List<PointF>()) { }

        public void AddPoint(PointF p) {
            Points.Add(p);
        }

        public void AddPoint(float x, float y) {
            AddPoint(new PointF(x, y));
        }

        public abstract void Draw(Graphics g);
    }

    public class TsPrefabRoadLook : TsPrefabLook {
        public float Width { private get; set; }

        public TsPrefabRoadLook() {
            ZIndex = 1;
        }

        public override void Draw(Graphics g) {
            g.DrawLines(new Pen(Color, Width), Points.ToArray());
        }
    }

    public class TsPrefabPolyLook : TsPrefabLook {
        public TsPrefabPolyLook(List<PointF> points) : base(points) { }

        public override void Draw(Graphics g) {
            g.FillPolygon(Color, Points.ToArray());
        }
    }

    public abstract class TsPrefabVectorLook {
        public int ZIndex { get; set; }
        public string Color { get; set; }
        protected readonly List<PointF> Points;

        protected TsPrefabVectorLook(List<PointF> points) {
            Points = points;
        }

        protected TsPrefabVectorLook() : this(new List<PointF>()) { }

        public void AddPoint(PointF p) {
            Points.Add(p);
        }

        public void AddPoint(float x, float y) {
            AddPoint(new PointF(x, y));
        }

        public abstract void Draw(StreamWriter g);
    }

    public class TsPrefabRoadVectorLook : TsPrefabVectorLook {
        public float Width { private get; set; }

        public TsPrefabRoadVectorLook() {
            ZIndex = 1;
        }

        public override void Draw(StreamWriter g) {
            g.WriteLine("LINE " + Points.Count + ";" + Color + ";" + Width);
            foreach (var point in Points) {
                g.WriteLine(FloatSerializer.Serialize(point));
            }
        }
    }

    public class TsPrefabPolyVectorLook : TsPrefabVectorLook {
        public TsPrefabPolyVectorLook(List<PointF> points) : base(points) { }

        public override void Draw(StreamWriter g) {
            g.WriteLine("FILL " + Points.Count + ";" + Color);
            foreach (var point in Points) {
                g.WriteLine(FloatSerializer.Serialize(point));
            }
        }
    }

    public class FloatSerializer {
        public static string Serialize(float f) {
            byte[] bytes = BitConverter.GetBytes(f); // Get the raw bytes
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(bytes); // Convert to big-endian if needed
            }
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static string Serialize(PointF p) {
            return $"{Serialize(p.X)} {Serialize(p.Y)}";
        }
    }
}