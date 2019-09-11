using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using D2D = SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Flattiverse;

namespace _2018Bot
{
    public partial class MainWindow : Form
    {
        private D2D.Factory factory;
        private D2D.WindowRenderTarget renderTarget;
        private UIBrushes brushes;
        private float width;
        private float height;
        private Starbase starbase;
        private Coordinates coordinates;
        private Factory textFactory;
        private TextFormat textFormat;
        private int firstCall = 0;
        private int fps;
        private int totalFps;
        private int second;
        private int lastSecond;

        public MainWindow(Starbase starbase)
        {
            this.starbase = starbase;

            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, false);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            Shown += MainWindow_Shown;
            starbase.ReadyEvent += ReadyMethod;

            InitializeComponent();
        }


        private void ReadyMethod()
        {
            Invalidate();
        }

        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            Vector position;
            Vector leftMargin;
            Vector rightMargin;

            if (second != lastSecond)
            {
                totalFps = fps;
                lastSecond = second;
                fps = 0;
            }

            if (initialize.Enabled)
                return;

            renderTarget.BeginDraw();

            renderTarget.Clear(new SharpDX.Mathematics.Interop.RawColor4(0, 0, 0, 0));

            UIMessage.WriteChat(renderTarget, starbase, textFactory, textFormat, ClientRectangle.Height, brushes.White);

            if (starbase.BaseMapHolder.Count == 0 || starbase.SelectedShip.CurrentMap == null)
            {
                renderTarget.EndDraw();
                return;
            }

            if (starbase.BaseMapHolder.Count > 0 && starbase.StarShipHolder.Count > 0 && firstCall == 0)
            {

                leftMargin = starbase.SelectedShip.CurrentMap[starbase.SelectedShip.Name].Position - new Vector(width / 2, height / 2);
                rightMargin = leftMargin + new Vector(width, height);
                coordinates = new Coordinates(leftMargin, rightMargin);

                firstCall = 1;
            }

            coordinates.CheckPosition(width, height);

            int playerCount = 0;

            using (SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(textFactory, "Fps: " + totalFps.ToString(), textFormat, 500, 500))
                renderTarget.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2() { X = 5, Y = 70 }, layout, brushes.White, D2D.DrawTextOptions.Clip);


            foreach (Player player in starbase.Connector.Player.UniverseGroup.Players)
            {
                using (Brush brush = new SolidBrush(player.Team.Color))
                {
                    using (SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(textFactory, "Name: " + player.Name, textFormat, 500, 500))
                        renderTarget.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2() { X = 5, Y = 95 + playerCount * 40 }, layout, brushes.White, D2D.DrawTextOptions.Clip);
                    using (SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(textFactory, "Ping: " + player.Ping.ToString(), textFormat, 500, 500))
                        renderTarget.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2() { X = 5, Y = 105 + playerCount * 40 }, layout, brushes.White, D2D.DrawTextOptions.Clip);
                    using (SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(textFactory, "ACT: " + player.AverageCommitTime, textFormat, 500, 500))
                        renderTarget.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2() { X = 5, Y = 115 + playerCount * 40 }, layout, brushes.White, D2D.DrawTextOptions.Clip);
                }

                playerCount++;
            }

            foreach (CosmicUnit unit in starbase.SelectedShip.CurrentMap.Query(-5000, -5000, 5000, 5000))
            {
                position = coordinates.Translate(unit.Position);

                if (unit.Type == CosmicUnitKind.Ship || unit.Type == CosmicUnitKind.Sun || unit.Type == CosmicUnitKind.Unknown)
                {
                    using (SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(textFactory, unit.Name, textFormat, 500, 500))
                        renderTarget.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2() { X = position.X - layout.Metrics.Width / 2f, Y = position.Y - layout.Metrics.Height / 2f }, layout, brushes.White, D2D.DrawTextOptions.Clip);
                }

                if (position > 0)
                    switch (unit.Type)
                    {
                        case CosmicUnitKind.Sun:
                            Vector vector = coordinates.Translate(unit.Position, unit.Radius);
                            float radius = coordinates.TranslateRadius(unit.Radius);
                            UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.Yellow);

                            foreach (CosmicCorona corona in ((CosmicSun)unit).Coronas)
                            {
                                float coronaRadius = (float)(corona.Radius);
                                vector = coordinates.Translate(unit.Position, coronaRadius);
                                radius = coordinates.TranslateRadius(coronaRadius);
                                UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.Yellow);
                            }
                            break;
                        case CosmicUnitKind.Planet:
                            vector = coordinates.Translate(unit.Position, unit.Radius);
                            radius = coordinates.TranslateRadius(unit.Radius);
                            UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.Green);
                            break;
                        case CosmicUnitKind.Meteroid:
                            if (!((CosmicMeteroid)unit).Still && ((CosmicMeteroid)unit).Timeout > 0)
                            {
                                ((CosmicMeteroid)unit).Timeout--;
                                vector = coordinates.Translate(unit.Position, unit.Radius);
                                radius = coordinates.TranslateRadius(unit.Radius);
                                UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.DarkRed);
                            }
                            else
                            {
                                vector = coordinates.Translate(unit.Position, unit.Radius);
                                radius = coordinates.TranslateRadius(unit.Radius);
                                UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.DarkRed);
                            }
                            break;
                        case CosmicUnitKind.MissionTarget:
                            vector = coordinates.Translate(unit.Position, unit.Radius);
                            radius = coordinates.TranslateRadius(unit.Radius);

                            float dominationSize = coordinates.TranslateRadius(((CosmicMissionTarget)unit).DominationRadius);


                            UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.Gray);
                            renderTarget.DrawLine(new SharpDX.Mathematics.Interop.RawVector2(coordinates.Translate(unit.Position).X, coordinates.Translate(unit.Position).Y), new SharpDX.Mathematics.Interop.RawVector2(coordinates.Translate(unit.Position + ((CosmicMissionTarget)unit).Direction * 100).X, coordinates.Translate(unit.Position + ((CosmicMissionTarget)unit).Direction * 100).Y), brushes.GetCustomColorBrush(((CosmicMissionTarget)unit).Color, renderTarget));

                            using (SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(textFactory, ((CosmicMissionTarget)unit).Number.ToString(), textFormat, 500, 500))
                                renderTarget.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2() { X = coordinates.Translate(unit.Position).X + 5f, Y = coordinates.Translate(unit.Position).Y - 5f }, layout, brushes.GetCustomColorBrush(((CosmicMissionTarget)unit).Color, renderTarget), D2D.DrawTextOptions.Clip);

                            using (SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(textFactory, ((CosmicMissionTarget)unit).DominationTicks.ToString(), textFormat, 500, 500))
                                renderTarget.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2() { X = coordinates.Translate(unit.Position).X + 10f, Y = coordinates.Translate(unit.Position).Y - 10f }, layout, brushes.GetCustomColorBrush(((CosmicMissionTarget)unit).Color, renderTarget), D2D.DrawTextOptions.Clip);

                            break;
                        case CosmicUnitKind.Ship:
                            if (((CosmicShip)unit).Timeout > 0)
                            {
                                ((CosmicShip)unit).Timeout--;

                                vector = coordinates.Translate(unit.Position, unit.Radius);
                                radius = coordinates.TranslateRadius(unit.Radius);

                                UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.GetCustomColorBrush(((CosmicShip)unit).Team.Color, renderTarget));

                                Vector bar1 = position - new Vector(radius * 2 / 2, 0) - new Vector(0, radius * 2 / 2 + 15f);
                                Vector bar2 = position - new Vector(radius * 2 / 2, 0) - new Vector(0, radius * 2 / 2 + 22f);

                                renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(bar1.X, bar1.Y, bar1.X + radius * 2, bar1.Y + 5), brushes.Gray);
                                renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(bar1.X, bar1.Y, bar1.X + ((CosmicShip)unit).Hull / ((CosmicShip)unit).HullMax * radius * 2, bar1.Y + 5), brushes.Red);

                                renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(bar2.X, bar2.Y, bar2.X + radius * 2, bar2.Y + 5), brushes.Gray);

                                if (((CosmicShip)unit).ShieldMax != 0)
                                    renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(bar2.X, bar2.Y, bar2.X + ((CosmicShip)unit).Shield / ((CosmicShip)unit).ShieldMax * radius * 2, bar2.Y + 5), brushes.Blue);

                            }
                            break;
                        case CosmicUnitKind.StarShip:
                            if (((CosmicStarship)unit).Timeout > 0)
                            {
                                ((CosmicStarship)unit).Timeout--;

                                vector = coordinates.Translate(unit.Position, unit.Radius);
                                radius = coordinates.TranslateRadius(unit.Radius);
                                UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.GetCustomColorBrush(((CosmicStarship)unit).Team.Color, renderTarget));

                            
                                if (starbase.SelectedShip.Name == ((CosmicStarship)unit).Name)
                                {
                                    renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(5, 5, 35, 15), brushes.Gray);
                                    renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(5, 5, ((CosmicStarship)unit).Hull / ((CosmicStarship)unit).HullMax * 35, 15), brushes.Red);

                                    renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(5, 20, 35, 30), brushes.Gray);
                                    renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(5, 20, ((CosmicStarship)unit).Energy / ((CosmicStarship)unit).EnergyMax * 35, 30), brushes.Yellow);

                                    UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.GetCustomColorBrush(((CosmicStarship)unit).Team.Color, renderTarget), 3);


                                    if (((CosmicStarship)unit).WeaponProductionStatus < 1)
                                        renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(5, 45, 10, 60), brushes.Yellow);
                                    else
                                        renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(5, 45, 10, 60), brushes.Green);

                                    Vector bar1 = position - new Vector(radius * 2 / 2, 0) - new Vector(0, radius * 2 / 2 + 15f);
                                    Vector bar2 = position - new Vector(radius * 2 / 2, 0) - new Vector(0, radius * 2 / 2 + 22f);

                                    renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(bar1.X, bar1.Y, bar1.X + radius * 2, bar1.Y + 5), brushes.Gray);
                                    renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(bar1.X, bar1.Y, bar1.X + ((CosmicStarship)unit).Hull / ((CosmicStarship)unit).HullMax * radius * 2, bar1.Y + 5), brushes.Red);

                                    renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(bar2.X, bar2.Y, bar2.X + radius * 2, bar2.Y + 5), brushes.Gray);

                                    if (((CosmicStarship)unit).ShieldMax != 0)
                                        renderTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(bar2.X, bar2.Y, bar2.X + ((CosmicStarship)unit).Shield / ((CosmicStarship)unit).ShieldMax * radius * 2, bar2.Y + 5), brushes.Blue);

                                    Vector direction = unit.MoveVector / coordinates.Magnify;

                                    direction.Length = radius * 2;
                                    Vector constructVector = (direction).RotatedBy(90f);
                                    constructVector.Length = radius;

                                    renderTarget.DrawLine(new SharpDX.Mathematics.Interop.RawVector2((position + constructVector).X, (position + constructVector).Y), new SharpDX.Mathematics.Interop.RawVector2((position + direction).X, (position + direction).Y), brushes.GetCustomColorBrush(((CosmicStarship)unit).Team.Color, renderTarget));
                                    renderTarget.DrawLine(new SharpDX.Mathematics.Interop.RawVector2((position - constructVector).X, (position - constructVector).Y), new SharpDX.Mathematics.Interop.RawVector2((position + direction).X, (position + direction).Y), brushes.GetCustomColorBrush(((CosmicStarship)unit).Team.Color, renderTarget));
                                }
                            }
                            break;
                        case CosmicUnitKind.Shot:
                            if (((CosmicShot)unit).Timeout > 0)
                            {
                                ((CosmicShot)unit).Timeout--;

                                vector = coordinates.Translate(unit.Position, unit.Radius);
                                radius = coordinates.TranslateRadius(unit.Radius);
                                UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.Gray);

                            }
                            break;
                        case CosmicUnitKind.Explosion:
                            if (((CosmicExplosion)unit).Timeout > 0)
                            {
                                ((CosmicExplosion)unit).Timeout--;

                                vector = coordinates.Translate(unit.Position, unit.Radius);
                                radius = coordinates.TranslateRadius(unit.Radius);
                                UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.Gray);
                            }
                            break;
                        default:
                            vector = coordinates.Translate(unit.Position, unit.Radius);
                            radius = coordinates.TranslateRadius(unit.Radius);
                            UICircle.DrawCircle(renderTarget, radius, new SharpDX.Mathematics.Interop.RawVector2(vector.X, vector.Y), brushes.Gray);
                            break;
                    }
            }

            renderTarget.EndDraw();
        }

        private void refresh_Tick(object sender, EventArgs e)
        {
            refresh.Enabled = false;

            Invalidate();
        }

        private void initialize_Tick(object sender, EventArgs e)
        {
            initialize.Enabled = false;

            factory = new D2D.Factory(D2D.FactoryType.SingleThreaded);
            renderTarget = new D2D.WindowRenderTarget(factory,
                new D2D.RenderTargetProperties(), new D2D.HwndRenderTargetProperties()
                {
                    Hwnd = Handle,
                    PixelSize = new SharpDX.Size2(ClientRectangle.Width, ClientRectangle.Height),
                    PresentOptions = D2D.PresentOptions.Immediately
                }
                );

            brushes = new UIBrushes(renderTarget);
          
            textFactory = new Factory();
            textFormat = new SharpDX.DirectWrite.TextFormat(textFactory, "Segoe UI Light", 10f);

            width = ClientRectangle.Width;
            height = ClientRectangle.Height;

            refresh.Enabled = true;
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {

            string email = "charlotte_gill@gmx.de";

            #region pw
            string pw = "!<3matze";
            #endregion pw            

            starbase.Connect(email, pw);
        }

        private void MainWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            CosmicUnit cosmicShip;

            switch (e.KeyChar)
            {
                case 'n':
                    try
                    {
                        starbase.CreateNewShip();
                    }
                    catch (Exception ex)
                    {
                        lock (starbase.SyncMessages)
                        {
                            starbase.Messages.Add(ex.Message);
                            ReadyMethod();
                        }
                    }
                    break;
                case '-':
                    coordinates.Magnify += 0.1f;
                    break;
                case '+':
                    coordinates.Magnify -= 0.1f;
                    break;
                case '1':
                    if (starbase.BaseMapHolder.Count > 0)
                    {
                        starbase.SelectedShip.CurrentMap = starbase.BaseMapHolder[0];

                        if (starbase.SelectedShip.CurrentMap.TryGetValue(starbase.SelectedShip.Name, out cosmicShip))
                            coordinates.Offset = cosmicShip.Position;
                    }
                    break;
                case '2':
                    if (starbase.BaseMapHolder.Count > 1)
                    {
                        starbase.SelectedShip.CurrentMap = starbase.BaseMapHolder[1];

                        if (starbase.SelectedShip.CurrentMap.TryGetValue(starbase.SelectedShip.Name, out cosmicShip))
                            coordinates.Offset = cosmicShip.Position;
                    }
                    break;
                case '3':
                    if (starbase.BaseMapHolder.Count > 2)
                    {
                        starbase.SelectedShip.CurrentMap = starbase.BaseMapHolder[2];

                        if (starbase.SelectedShip.CurrentMap.TryGetValue(starbase.SelectedShip.Name, out cosmicShip))
                            coordinates.Offset = cosmicShip.Position;
                    }
                    break;
                case '4':
                    if (starbase.BaseMapHolder.Count > 3)
                    {
                        starbase.SelectedShip.CurrentMap = starbase.BaseMapHolder[3];

                        if (starbase.SelectedShip.CurrentMap.TryGetValue(starbase.SelectedShip.Name, out cosmicShip))
                            coordinates.Offset = cosmicShip.Position;
                    }
                    break;
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            starbase.Connector.Close();
            Environment.Exit(0);
        }

        private void MainWindow_MouseClick(object sender, MouseEventArgs e)
        {
            CosmicUnit shipUnit;

            if (e.Button == MouseButtons.Right)
                starbase.SetNewDestination(coordinates.TranslateBack(new Vector(e.X, e.Y)));

            if (e.Button == MouseButtons.Left)
            {
                Vector leftClick = new Vector(e.X, e.Y);
                foreach (Starship ship in starbase.StarShipHolder)
                {
                    if (starbase.SelectedShip.CurrentMap.TryGetValue(ship.Name, out shipUnit))
                        if ((leftClick - coordinates.Translate(shipUnit.Position)).Length <= coordinates.TranslateRadius(shipUnit.Radius))
                        {
                            starbase.SelectedShip = ship;
                            break;
                        }
                }
            }

        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (coordinates == null)
                return;

            coordinates.Mouse = new Vector(e.X, e.Y);
        }
    }
}
