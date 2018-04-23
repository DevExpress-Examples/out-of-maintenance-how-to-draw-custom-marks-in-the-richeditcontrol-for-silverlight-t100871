#region #customdrawmark
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Layout.Export;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SilverlightApplication2 {
	public partial class MainPage : UserControl {
		public MainPage() {
			InitializeComponent();

			Stream documentStream = typeof(MainPage).Assembly.GetManifestResourceStream("SilverlightApplication2.Test.docx");
			richEditControl1.LoadDocument(documentStream, DocumentFormat.OpenXml);
		}
        private void barButtonItem1_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Document doc = richEditControl1.Document;
            // Create a custom mark at the caret position and attach arbitrary data to the mark.
            // In this example the data specifies the color that will be used to draw the mark.
            CustomMark m = doc.CreateCustomMark(doc.Selection.Start, new SolidColorBrush(Colors.Orange));
        }

		private void richEditControl1_CustomMarkDraw(object sender, CustomMarkDrawEventArgs e) {
			Canvas surface = LayoutHelper.FindElementByName(richEditControl1, "Surface") as Canvas;
			if ( surface == null )
				return;

			GeneralTransform transform = surface.TransformToVisual(richEditControl1);
			RectangleGeometry clip = new RectangleGeometry() { Rect = new Rect(transform.Transform(new Point(0, 0)), surface.RenderSize) };
			richEditCanvas.Children.Clear();
			richEditCanvas.Clip = clip;

			foreach ( CustomMarkVisualInfo info in e.VisualInfoCollection ) {
                Document doc = richEditControl1.Document;
                CustomMark mark = doc.GetCustomMarkByVisualInfo(info);
                // Get a brush associated with the mark.
                Brush curBrush = info.UserData as Brush;
                // Use a different brush to draw custom marks located above the caret.
                if (mark.Position < doc.Selection.Start) curBrush = new SolidColorBrush(Colors.Green);
                Rectangle rect = new Rectangle();
                rect.Width = 2;
                rect.Height = info.Bounds.Height;
                rect.Fill = curBrush;
                Canvas.SetLeft(rect, info.Bounds.X);
                Canvas.SetTop(rect, info.Bounds.Y);
                richEditCanvas.Children.Add(rect);
			}
		}
	}
}
#endregion #customdrawmark