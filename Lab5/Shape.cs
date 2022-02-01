using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Lab5
{
    public class UserShape
    {
        Path p = new Path();
        private string coordinates;
        Int32 mousePositionX;
        Int32 mousePositionY;

        public UserShape()
        {

        }

        public UserShape(Int32 _thicknes, Brush _fill, Brush _stroke, Int32 mouseX, Int32 mouseY)
        {
            p.StrokeThickness = _thicknes;
            p.Fill = _fill;
            p.Stroke = _stroke;
            mousePositionX = mouseX;
            mousePositionY = mouseY;

            coordinates = string.Format
                ("M{0},{1}   L{2},{3} " +
                    "L{4},{5}   L{6},{7} " +
                    "L{8},{9}   L{10},{11} " +
                    "L{12},{13} L{14},{15} z",
                    mouseX, mouseY, mouseX + 15, mouseY - 85,
                    mouseX + 100, mouseY - 100, mouseX + 15, mouseY - 115,
                    mouseX, mouseY - 200, mouseX - 15, mouseY - 115,
                    mouseX - 100, mouseY - 100, mouseX - 15, mouseY - 85);
            p.Data = Geometry.Parse(coordinates);
        }

        public UserShape(Int32 _thicknes, Brush _fill, Brush _stroke, String _coordinates)
        {
            p.StrokeThickness = _thicknes;
            p.Fill = _fill;
            p.Stroke = _stroke;
            coordinates = _coordinates;

        }

        public Path Coordinates
        {
            get { return p; }
        }

        public Int32 MouseX
        {
            get { return mousePositionX ; }
        }

        public Int32 MouseY
        {
            get { return mousePositionY; }
        }

        public Brush Fill
        {
            set { p.Fill = value; }
            get { return p.Fill; }
        }

        public Brush Stroke
        {
            set { p.Stroke = value; }
            get { return p.Stroke; }
        }

        public Int32 StrokeThickness
        {
            set { p.StrokeThickness = value; }
            get { return (Int32)p.StrokeThickness; }
        }

        public string Passport()
        {
            string passport = String.Format(StrokeThickness.ToString() + " " +
                                                Fill.ToString() + " " +
                                                Stroke.ToString() + " " +
                                                MouseX.ToString() + " " +
                                                MouseY.ToString());
            return passport;
        }

        public XElement PassportXml()
        {
            XElement xFigurePassport =
                    new XElement("UserFigure",
                        new XAttribute("type","TetrahedralStar"),
                        new XElement("StrokeThickness", StrokeThickness.ToString()),
                        new XElement("Fill", Fill.ToString()),
                        new XElement("Stroke", Stroke.ToString()),
                        new XElement("MouseX", MouseX.ToString()),
                        new XElement("MouseY", MouseY.ToString())
                            );
            return xFigurePassport;
        }
    }
}