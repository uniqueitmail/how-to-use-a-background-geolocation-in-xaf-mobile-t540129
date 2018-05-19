Namespace BackgroundGeolocation.Module.Mobile
	Partial Public Class BackgroundGeolocationMobileModule
		''' <summary> 
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary> 
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Component Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			' 
			' BackgroundGeolocationMobileModule
			' 
			Me.RequiredModuleTypes.Add(GetType(BackgroundGeolocation.Module.BackgroundGeolocationModule))
			Me.RequiredModuleTypes.Add(GetType(DevExpress.ExpressApp.Mobile.SystemModule.SystemMobileModule))
			Me.RequiredModuleTypes.Add(GetType(DevExpress.ExpressApp.Maps.Mobile.MapsMobileModule))
		End Sub

		#End Region
	End Class
End Namespace