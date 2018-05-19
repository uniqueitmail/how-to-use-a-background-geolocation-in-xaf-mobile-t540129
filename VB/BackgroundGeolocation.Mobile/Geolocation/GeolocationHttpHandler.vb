Imports System
Imports System.Configuration
Imports System.Globalization
Imports System.IO
Imports System.Web
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.Base
Imports BackgroundGeolocation.Module.BusinessObjects
Imports BackgroundGeolocation.Module.Mobile.Services
Imports Newtonsoft.Json

Namespace BackgroundGeolocation.Mobile
	Public Class GeolocationHttpHandler
		Implements IHttpHandler

		Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
			AddLocation(context.Request.InputStream)
		End Sub

		Public Sub AddLocation(ByVal input As Stream)
			Try
				Dim geolocationJsonObject As GeolocationJsonObject = ParseGeolocationObjects(input)
				Dim device As Device = geolocationJsonObject.Device
				Dim location As Location = geolocationJsonObject.Location
				If geolocationJsonObject IsNot Nothing Then
					XpoTypesInfoHelper.GetXpoTypeInfoSource()
					XafTypesInfo.Instance.RegisterEntity(GetType(DeviceInfo))
					XafTypesInfo.Instance.RegisterEntity(GetType(DeviceLocation))
					Dim connectionString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
					Using objectSpaceProvider As New XPObjectSpaceProvider(New ConnectionStringDataStoreProvider(connectionString), True)
						Using objectSpace As IObjectSpace = objectSpaceProvider.CreateObjectSpace()
							Dim deviceInfo As DeviceInfo = objectSpace.FindObject(Of DeviceInfo)(CriteriaOperator.Parse("Uuid=?", geolocationJsonObject.Device.Uuid))
							If deviceInfo Is Nothing Then
								deviceInfo = objectSpace.CreateObject(Of DeviceInfo)()
								deviceInfo.Name = device.Uuid
							End If

							deviceInfo.Manufacturer = device.Manufacturer
							deviceInfo.Model = device.Model
							deviceInfo.Platform = device.Platform
							deviceInfo.Version = device.Version
							deviceInfo.Uuid = device.Uuid

							Dim deviceLocation As DeviceLocation = objectSpace.CreateObject(Of DeviceLocation)()
							deviceLocation.DeviceInfo = deviceInfo
							deviceLocation.Accuracy = location.Coords.Accuracy
							deviceLocation.Latitude = location.Coords.Latitude
							deviceLocation.Longitude = location.Coords.Longitude
							deviceLocation.Speed = location.Coords.Speed
							deviceLocation.ActivityType = location.Activity.Type
							deviceLocation.TimeStamp = Date.ParseExact(location.Timestamp, "yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture)

							deviceInfo.Locations.Add(deviceLocation)
							objectSpace.CommitChanges()
						End Using
					End Using
				End If
			Catch ex As Exception
				Dim message As String = "Cannot build a mobile application due to the following error: " & ex.Message
				Tracing.Tracer.LogError(message)
				'throw new WebFaultException<string>(message, HttpStatusCode.InternalServerError);
				Throw New Exception(message)
			End Try
		End Sub


		Private Function ParseGeolocationObjects(ByVal input As Stream) As GeolocationJsonObject
			Dim serializer = New JsonSerializer()
			Dim reader As TextReader = New StreamReader(input)
			Return TryCast(serializer.Deserialize(reader, GetType(GeolocationJsonObject)), GeolocationJsonObject)
		End Function

		Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
			Get
				Return True
			End Get
		End Property
	End Class
End Namespace