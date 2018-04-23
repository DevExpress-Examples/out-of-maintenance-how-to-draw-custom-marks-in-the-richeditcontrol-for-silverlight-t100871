Imports Microsoft.VisualBasic
#Region "#customdrawmark"
Imports DevExpress.Xpf.Core.Native
Imports DevExpress.Xpf.RichEdit
Imports DevExpress.XtraRichEdit
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraRichEdit.Layout.Export
Imports System.IO
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Shapes

Namespace SilverlightApplication2
	Partial Public Class MainPage
		Inherits UserControl
		Public Sub New()
			InitializeComponent()

			Dim documentStream As Stream = GetType(MainPage).Assembly.GetManifestResourceStream("Test.docx")
			richEditControl1.LoadDocument(documentStream, DocumentFormat.OpenXml)
		End Sub
		Private Sub barButtonItem1_ItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
			Dim doc As Document = richEditControl1.Document
			' Create a custom mark at the caret position and attach arbitrary data to the mark.
			' In this example the data specifies the color that will be used to draw the mark.
			Dim m As CustomMark = doc.CreateCustomMark(doc.Selection.Start, New SolidColorBrush(Colors.Orange))
		End Sub

		Private Sub richEditControl1_CustomMarkDraw(ByVal sender As Object, ByVal e As CustomMarkDrawEventArgs)
			Dim surface As Canvas = TryCast(LayoutHelper.FindElementByName(richEditControl1, "Surface"), Canvas)
			If surface Is Nothing Then
				Return
			End If

			Dim transform As GeneralTransform = surface.TransformToVisual(richEditControl1)
			Dim clip As New RectangleGeometry() With {.Rect = New Rect(transform.Transform(New Point(0, 0)), surface.RenderSize)}
			richEditCanvas.Children.Clear()
			richEditCanvas.Clip = clip

			For Each info As CustomMarkVisualInfo In e.VisualInfoCollection
				Dim doc As Document = richEditControl1.Document
				Dim mark As CustomMark = doc.GetCustomMarkByVisualInfo(info)
				' Get a brush associated with the mark.
				Dim curBrush As Brush = TryCast(info.UserData, Brush)
				' Use a different brush to draw custom marks located above the caret.
				If mark.Position < doc.Selection.Start Then
					curBrush = New SolidColorBrush(Colors.Green)
				End If
				Dim rect As New Rectangle()
				rect.Width = 2
				rect.Height = info.Bounds.Height
				rect.Fill = curBrush
				Canvas.SetLeft(rect, info.Bounds.X)
				Canvas.SetTop(rect, info.Bounds.Y)
				richEditCanvas.Children.Add(rect)
			Next info
		End Sub
	End Class
End Namespace
#End Region ' #customdrawmark