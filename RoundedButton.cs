using System.Drawing.Drawing2D;

class RoundedButton : Button
{
    
    private GraphicsPath GetRoundPath(RectangleF rect, int radius)
    {
        float gridMargin = 2.75F;
        float r2 = radius / 2f;
        GraphicsPath graphPath = new GraphicsPath();

        graphPath.AddArc(rect.X + gridMargin, rect.Y + gridMargin, radius, radius, 180, 90);
        graphPath.AddLine(rect.X + r2 + gridMargin, rect.Y + gridMargin, rect.Width - r2 - gridMargin, rect.Y + gridMargin);
        graphPath.AddArc(rect.X + rect.Width - radius - gridMargin, rect.Y + gridMargin, radius, radius, 270, 90);
        graphPath.AddLine(rect.Width - gridMargin, rect.Y + r2, rect.Width - gridMargin, rect.Height - r2 - gridMargin);
        graphPath.AddArc(rect.X + rect.Width - radius - gridMargin, rect.Y + rect.Height - radius - gridMargin, radius, radius, 0, 90);
        graphPath.AddLine(rect.Width - r2 - gridMargin, rect.Height - gridMargin, rect.X + r2 - gridMargin, rect.Height - gridMargin);
        graphPath.AddArc(rect.X + gridMargin, rect.Y + rect.Height - radius - gridMargin, radius, radius, 90, 90);
        graphPath.AddLine(rect.X + gridMargin, rect.Height - r2 - gridMargin, rect.X + gridMargin, rect.Y + r2 + gridMargin);

        graphPath.CloseFigure();
        return graphPath;
    }
    
    protected override void OnPaint(PaintEventArgs e)
    {
        int curveRadius = 40;
        float borderThickness = 2;
        base.OnPaint(e);
        RectangleF rect = new RectangleF(0, 0, Width, Height);
        GraphicsPath graphPath = GetRoundPath(rect, curveRadius);

        Region = new Region(graphPath);
        using (Pen pen = new Pen(Color.LightGray, borderThickness))
        {
            pen.Alignment = PenAlignment.Inset;
            e.Graphics.DrawPath(pen, graphPath);
        }
    }
    
}