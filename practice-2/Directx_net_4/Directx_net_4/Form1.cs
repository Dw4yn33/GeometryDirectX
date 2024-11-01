using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Directx_net_4
{
    public partial class Form1 : Form
    {
        private Device device;
        private CustomVertex.PositionNormalColored[] vertices;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            InitializeDevice();
            CameraPositioning();
            InitializeLight();
            VertexDeclaration();
        }

        public void InitializeDevice()
        {
            PresentParameters presentParams = new PresentParameters
            {
                Windowed = true,
                SwapEffect = SwapEffect.Discard
            };
            device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParams);
        }

        public void CameraPositioning()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1f, 50f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(20, 20, 20), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            device.RenderState.Lighting = true; // Включаем освещение
            device.RenderState.CullMode = Cull.None;
        }

        public void InitializeLight()
        {
            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Direction = new Vector3(1, 1, 1); // Направление света
            device.Lights[0].Diffuse = Color.White;
            device.Lights[0].Enabled = true;

            device.RenderState.Ambient = Color.FromArgb(0x404040);
        }

        public void VertexDeclaration()
        {
            // Вершины пирамиды или призмы с нормалями и цветом
            vertices = new CustomVertex.PositionNormalColored[19];

            Vector3 normalBase = new Vector3(0, 0, -1); // Нормаль для основания
            Vector3 normalSide1 = new Vector3(-1, 1, 1); // Нормали для сторон
            Vector3 normalSide2 = new Vector3(1, 1, 1);
            Vector3 normalSide3 = new Vector3(-1, -1, 1);
            Vector3 normalSide4 = new Vector3(1, -1, 1);

            // Координаты основания пятиугольника
            vertices[0] = new CustomVertex.PositionNormalColored(new Vector3(0f, 0f, 0f), normalBase, Color.Red.ToArgb());
            vertices[1] = new CustomVertex.PositionNormalColored(new Vector3(3f, 3f, 0f), normalBase, Color.Green.ToArgb());
            vertices[2] = new CustomVertex.PositionNormalColored(new Vector3(0f, 3f, 0f), normalBase, Color.Blue.ToArgb());

            vertices[3] = new CustomVertex.PositionNormalColored(new Vector3(0f, 0f, 0f), normalBase, Color.Red.ToArgb());
            vertices[4] = new CustomVertex.PositionNormalColored(new Vector3(0f, -3f, 0f), normalBase, Color.Yellow.ToArgb());
            vertices[5] = new CustomVertex.PositionNormalColored(new Vector3(3f, -3f, 0f), normalBase, Color.Aqua.ToArgb());

            // Вершина пирамиды
            vertices[6] = new CustomVertex.PositionNormalColored(new Vector3(0f, 0f, 5f), normalSide1, Color.White.ToArgb());

            // Треугольники сторон пирамиды
            vertices[7] = vertices[0];
            vertices[8] = vertices[1];
            vertices[9] = vertices[6];

            vertices[10] = vertices[1];
            vertices[11] = vertices[2];
            vertices[12] = vertices[6];

            vertices[13] = vertices[2];
            vertices[14] = vertices[4];
            vertices[15] = vertices[6];

            vertices[16] = vertices[4];
            vertices[17] = vertices[5];
            vertices[18] = vertices[6];
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            device.Clear(ClearFlags.Target, Color.DarkSlateBlue, 1.0f, 0);
            device.BeginScene();
            device.VertexFormat = CustomVertex.PositionNormalColored.Format;

            // Мировое преобразование для поворота
            device.Transform.World = Matrix.RotationY(0.01f);

            // Отрисовка пирамиды или призмы
            device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices.Length / 3, vertices);

            device.EndScene();
            device.Present();

            this.Invalidate();
        }
    }
}
