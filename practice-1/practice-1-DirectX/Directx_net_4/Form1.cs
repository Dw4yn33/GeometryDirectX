using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Directx_net_4
{
    public partial class Form1 : Form
    {
        private Device device;
        private float angle = 0f;
        private CustomVertex.PositionNormalColored[] vertices;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            InitializeDevice();
            CameraPositioning();
            //InitializeLight();
            VertexDeclaration();
        }

        public void InitializeDevice()
        {
            PresentParameters presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParams);
        }

        public void CameraPositioning()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1f, 50f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(20, 20, 20), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            device.RenderState.Lighting = false; // Отключаем освещение для отображения цвета вершин
            device.RenderState.CullMode = Cull.None;
        }

        public void InitializeLight()
        {
            // Оставим код освещения, если нужно будет его включить позже
            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Direction = new Vector3(-1, -1, -1);
            device.Lights[0].Position = new Vector3(20, 20, 20);
            device.Lights[0].Diffuse = Color.White;
            device.Lights[0].Enabled = true;

            device.RenderState.Ambient = Color.FromArgb(0x404040);
        }

        public void VertexDeclaration()
        {
            // Вершины с нормалями и цветом, но с разными цветами для каждой вершины
            vertices = new CustomVertex.PositionNormalColored[6];

            Vector3 normal = new Vector3(0, 0, 1); // Нормаль для плоской фигуры

            // Треугольник 1 (включает вершины 0, 1, 2)
            vertices[0].Position = new Vector3(0f, 0f, 0f);
            vertices[0].Normal = normal;
            vertices[0].Color = Color.Red.ToArgb(); // Красный

            vertices[1].Position = new Vector3(3f, 3f, 0f);
            vertices[1].Normal = normal;
            vertices[1].Color = Color.Green.ToArgb(); // Зеленый

            vertices[2].Position = new Vector3(0f, 3f, 0f);
            vertices[2].Normal = normal;
            vertices[2].Color = Color.Blue.ToArgb(); // Синий

            // Треугольник 2 (включает вершины 0, 3, 4)
            vertices[3].Position = new Vector3(0f, 0f, 0f);
            vertices[3].Normal = normal;
            vertices[3].Color = Color.Red.ToArgb(); // Красный

            vertices[4].Position = new Vector3(0f, -3f, 0f);
            vertices[4].Normal = normal;
            vertices[4].Color = Color.Yellow.ToArgb(); // Желтый

            vertices[5].Position = new Vector3(3f, -3f, 0f);
            vertices[5].Normal = normal;
            vertices[5].Color = Color.Aqua.ToArgb(); // Голубой
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            device.Clear(ClearFlags.Target, Color.DarkSlateBlue, 1.0f, 0);

            device.BeginScene();
            device.VertexFormat = CustomVertex.PositionNormalColored.Format;
            device.DrawUserPrimitives(PrimitiveType.TriangleList, 2, vertices);
            device.EndScene();

            device.Present();
            this.Invalidate();
            angle += 0.05f;
        }
    }
}
