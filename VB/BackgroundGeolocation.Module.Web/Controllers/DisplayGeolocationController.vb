Imports System.Collections.Generic
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Maps.Web
Imports DevExpress.ExpressApp.Maps.Web.Helpers
Imports DevExpress.ExpressApp.SystemModule
Imports DevExpress.Persistent.Base
Imports BackgroundGeolocation.Module.BusinessObjects
Imports Newtonsoft.Json

Namespace MobileTestApplication.Module.Web.Controllers
	Public Class DisplayGeolocationController
		Inherits ObjectViewController(Of ListView, DeviceLocation)

		Protected Overrides Sub OnActivated()
			MyBase.OnActivated()

			Dim resetViewSettingsController As ResetViewSettingsController = Frame.GetController(Of ResetViewSettingsController)()
			If resetViewSettingsController IsNot Nothing Then
				resetViewSettingsController.ResetViewSettingsAction.Active("EnabledInNestedMaps") = False
			End If
		End Sub
		Protected Overrides Sub OnDeactivated()
			Dim resetViewSettingsController As ResetViewSettingsController = Frame.GetController(Of ResetViewSettingsController)()
			If resetViewSettingsController IsNot Nothing Then
				resetViewSettingsController.ResetViewSettingsAction.Active("EnabledInNestedMaps") = True
			End If

			MyBase.OnDeactivated()
		End Sub
		Protected Overrides Sub OnViewControlsCreated()
			MyBase.OnViewControlsCreated()

			Dim webMapsListEditor As WebMapsListEditor = TryCast(View.Editor, WebMapsListEditor)
			If webMapsListEditor IsNot Nothing Then
				webMapsListEditor.MapViewer.ClientSideEvents.Customize = GetCustomizeScript()
			End If
		End Sub
		Private Function GetPolylinePoints() As IEnumerable(Of MapPoint)
			Dim polylinePoints As New List(Of MapPoint)()
			For Each marker As IMapsMarker In View.CollectionSource.List
				polylinePoints.Add(New MapPoint(marker.Latitude, marker.Longitude))
			Next marker
			Return polylinePoints
		End Function
		Private Function GetCustomizeScript() As String
			Dim polylines = JsonConvert.SerializeObject(GetPolylinePoints())
			Return String.Format("function(sender, map) {{" & ControlChars.CrLf & _
"    map.on('ready', function(e) {{" & ControlChars.CrLf & _
"        var googleMap = e.originalMap;" & ControlChars.CrLf & _
"        var clientPathCoordinates = {0};" & ControlChars.CrLf & _
"        " & ControlChars.CrLf & _
"        var iconSettings = {{" & ControlChars.CrLf & _
"            path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW" & ControlChars.CrLf & _
"        }};" & ControlChars.CrLf & _
ControlChars.CrLf & _
"        var clientPath = new google.maps.Polyline({{" & ControlChars.CrLf & _
"            path: clientPathCoordinates," & ControlChars.CrLf & _
"            strokeColor: '#FF0000'," & ControlChars.CrLf & _
"            strokeOpacity: 1.0," & ControlChars.CrLf & _
"            strokeWeight: 2," & ControlChars.CrLf & _
"            icons: [{{" & ControlChars.CrLf & _
"                icon: iconSettings," & ControlChars.CrLf & _
"                repeat: '35px'," & ControlChars.CrLf & _
"                offset: '100%'}}]" & ControlChars.CrLf & _
"        }});" & ControlChars.CrLf & _
ControlChars.CrLf & _
"        window.clientPath = clientPath;" & ControlChars.CrLf & _
"        clientPath.setMap(googleMap);" & ControlChars.CrLf & _
"    }});" & ControlChars.CrLf & _
"}}", polylines)
		End Function
	End Class
End Namespace
