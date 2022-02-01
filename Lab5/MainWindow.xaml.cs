using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using System.Linq;


namespace Lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        //shape parameters
        public Int32 cmbThickness;
        public Int32 cmbFillSelected;
        public Int32 cmbStrokeSelected;

        //list of shapes
        private List<UserShape> shapesCollection = new List<UserShape>();

        public MainWindow()
        {
            InitializeComponent();

            //set default shape parameters
            cmbThickness = Cmb_Thickness.SelectedIndex;
            cmbFillSelected = Cmb_Fill.SelectedIndex;
            cmbStrokeSelected = Cmb_Stroke.SelectedIndex;

            //settings for save-command
            CommandBinding binding = new CommandBinding(ApplicationCommands.Save);
            binding.Executed += Save_Executed;
            CommandBindings.Add(binding);

            //Event for getting of a mouse move coordinates
            canvas.MouseMove += new MouseEventHandler(Dynamic_MouseMove);
        }

        //-----------Menu---EVENT---Export collection in XML-file
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.FileName = "Lab5_saved"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension
            dlg.ShowDialog();

            //Запись List<T> в XML формате
            XDocument doc =
                new XDocument(new XElement("Figures",
                    from shape in shapesCollection
                    select
                    shape.PassportXml()));
            doc.Save(dlg.FileName);
        }
        
        //-----------Menu---EVENT---EXIT
        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        //-----------Menu---EVENT---LOAD
        private void Menu_Open_Click(object sender, RoutedEventArgs e)
        {
             //Delete all shapes in a definite collection and from canvas if they exist
            if (shapesCollection.Count > 0)
            {
                shapesCollection.Clear();
                canvas.Children.Clear();
            }

            //Read XML-file
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Lab5_saved"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension
            dlg.ShowDialog();

            FileName.Content = "File: " + dlg.FileName.ToString();

            XDocument xDoc = XDocument.Load(dlg.FileName);
            IEnumerable<XElement> mainContainer = xDoc.Descendants();
            List<XElement> elementContainer = new List<XElement>();

            //Write shapes from opened file
            Int32 _lineThikness;
            SolidColorBrush _fillColor;
            SolidColorBrush _strokeColor;
            Int32 _mouse_X;
            Int32 _mouse_Y;

            //Export collection from mainContainer to elemntContainer (can routing by index)
            foreach (XElement elem in mainContainer)
            {
                elementContainer.Add(elem);
            }

            //Create and draw new Shapes 
            for (Int32 i = 0; i < elementContainer.Count; i++)
            {
                if (elementContainer[i].Name.ToString() == "UserFigure")
                {
                    //(Re)Initialization 
                    _lineThikness = Convert.ToInt32(elementContainer[i + 1].Value.ToString());
                    _fillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(elementContainer[i + 2].Value.ToString()));
                    _strokeColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(elementContainer[i + 3].Value.ToString()));
                    _mouse_X = Convert.ToInt32(elementContainer[i + 4].Value.ToString());
                    _mouse_Y = Convert.ToInt32(elementContainer[i + 5].Value.ToString());

                    //Creation
                    UserShape newShape = new UserShape(_lineThikness, _fillColor, _strokeColor, _mouse_X, _mouse_Y);
                    canvas.Children.Add(newShape.Coordinates);

                    //create a collection of shapes for each TabItem
                    //Int32 num = _tabItemShapeCollections[canvasNumer].Count();
                    //_tabItemShapeCollections[canvasNumer].Insert(num, newShape);
                }
            }
        }

        //-----------Menu---EVENT---ABOUT A PROGRAM
        private void Menu_About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow W = new AboutWindow();
            W.ShowDialog();
        }

        //-----------STATUS---EVENT---DISPLAY MOUSE POSITION 
        private void Dynamic_MouseMove(object sender, MouseEventArgs e)
        {
            MousePosition.Content = "Mouse position: " + e.GetPosition(this).ToString();
        }

        //-----------SHAPE---EVENT---DRAW A SHAPE
        private void Dynamic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Get information about future shape
            ComboBoxItem itemThickness = (ComboBoxItem)Cmb_Thickness.SelectedItem;
            Int32 lineThickness = Convert.ToInt32(itemThickness.Content.ToString());
            ComboBoxItem itemFill = (ComboBoxItem)Cmb_Fill.SelectedItem;
            ComboBoxItem itemStroke = (ComboBoxItem)Cmb_Stroke.SelectedItem;
            SolidColorBrush _fillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(itemFill.Content.ToString()));
            SolidColorBrush _strokeColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(itemStroke.Content.ToString()));

            //Get mouse coordinates            
            var relativePosition = e.GetPosition(this);
            Int32 mouseX = (Int32)relativePosition.X;
            Int32 mouseY = (Int32)relativePosition.Y;

            //Draw a shape
            UserShape newShape = new UserShape(lineThickness, _fillColor, _strokeColor, mouseX, mouseY);
            canvas.Children.Add(newShape.Coordinates);
            shapesCollection.Add(newShape);
            
            //Enable menu shape
            Menu_Save.IsEnabled = true;
        }

        //-----------Menu---EVENT---Shape Editor
        private void MenuItem_PaintPrameteres_Click(object sender, RoutedEventArgs e)
        {
            ShapeEditor editor = new ShapeEditor(cmbThickness, Cmb_Fill.SelectedIndex, Cmb_Stroke.SelectedIndex);
            editor.Owner = this;
            editor.ShowDialog();
            Cmb_Thickness.SelectedIndex = cmbThickness;
            Cmb_Fill.SelectedIndex = cmbFillSelected;
            Cmb_Stroke.SelectedIndex = cmbStrokeSelected;
        }

        
    }
}
