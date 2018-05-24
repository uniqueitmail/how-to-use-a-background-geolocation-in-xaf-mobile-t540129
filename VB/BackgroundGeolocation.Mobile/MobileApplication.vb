Imports System
Imports System.Configuration
Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Mobile
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.ExpressApp.Security
Imports System.Reflection
Imports System.IO
Imports System.Text
Imports System.Xml.Linq

Namespace BackgroundGeolocation.Mobile
	' For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWebWebApplicationMembersTopicAll.aspx
	Partial Public Class BackgroundGeolocationMobileApplication
		Inherits MobileApplication

		Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
		Private module2 As DevExpress.ExpressApp.Mobile.SystemModule.SystemMobileModule
		Private module3 As BackgroundGeolocation.Module.BackgroundGeolocationModule
		Private module4 As BackgroundGeolocation.Module.Mobile.BackgroundGeolocationMobileModule
		Private securityModule1 As DevExpress.ExpressApp.Security.SecurityModule
		Private mapsMobileModule As DevExpress.ExpressApp.Maps.Mobile.MapsMobileModule

		#Region "Default XAF configuration options (https:" 'www.devexpress.com/kb=T501418)
		Shared Sub New()
			DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = True
			DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = False
		End Sub
		#End Region
		Public Sub New()
			Tracing.Initialize()
			If ConfigurationManager.ConnectionStrings("ConnectionString") IsNot Nothing Then
				ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
			End If
#If EASYTEST Then
			If ConfigurationManager.ConnectionStrings("EasyTestConnectionString") IsNot Nothing Then
				ConnectionString = ConfigurationManager.ConnectionStrings("EasyTestConnectionString").ConnectionString
			End If
#End If
			InitializeComponent()
			If System.Diagnostics.Debugger.IsAttached AndAlso CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema Then
				DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways
			End If

			AdditionalPhoneGapPlugins.Add("<plugin name=""cordova-background-geolocation-lt"" source=""npm"" spec=""2.7.3"">" & ControlChars.CrLf & _
"			<variable name=""LOCATION_ALWAYS_USAGE_DESCRIPTION"" value=""Background location-tracking is required"" />" & ControlChars.CrLf & _
"			<variable name=""LOCATION_WHEN_IN_USE_USAGE_DESCRIPTION"" value=""Background location-tracking is required"" />" & ControlChars.CrLf & _
"			<variable name=""MOTION_USAGE_DESCRIPTION"" value=""Using the accelerometer increases battery-efficiency by intelligently toggling location-tracking only when the device is detected to be moving"" />" & ControlChars.CrLf & _
"			<variable name=""LICENSE"" value=""YOUR_LICENSE_KEY"" />" & ControlChars.CrLf & _
"			</plugin>")
			AdditionalPhoneGapPlugins.Add("<plugin name=""cordova-plugin-device"" source=""npm"" spec=""1.1.6""></plugin>")

            Dim geolocationScript As String = ReadResourceString("GeolocationScript.js")
            RegisterClientScriptOnApplicationStart("GeolocationScript", geolocationScript)
		End Sub

        Public Shared Function ReadResource(ByVal resourceName As String) As Byte()
            Dim buffer() As Byte = Nothing
            Using stream As Stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
                If stream IsNot Nothing Then
                    buffer = New Byte(stream.Length - 1) {}
                    stream.Read(buffer, 0, CInt(stream.Length))
                End If
            End Using
            Return buffer
        End Function
        Public Shared Function ReadResourceString(ByVal resourceName As String) As String
            Dim resourceBytes() As Byte = ReadResource(resourceName)
            Return Encoding.UTF8.GetString(resourceBytes)
        End Function
        Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
			args.ObjectSpaceProvider = New XPObjectSpaceProvider(GetDataStoreProvider(args.ConnectionString, args.Connection), True)
			args.ObjectSpaceProviders.Add(New NonPersistentObjectSpaceProvider(TypesInfo, Nothing))
		End Sub
		Private Function GetDataStoreProvider(ByVal connectionString As String, ByVal connection As System.Data.IDbConnection) As IXpoDataStoreProvider
			Dim dataStoreProvider As IXpoDataStoreProvider = Nothing
			If Not String.IsNullOrEmpty(connectionString) Then
				dataStoreProvider = New ConnectionStringDataStoreProvider(connectionString)
			ElseIf connection IsNot Nothing Then
				dataStoreProvider = New ConnectionDataStoreProvider(connection)
			End If
			Return dataStoreProvider
		End Function
		Private Sub BackgroundGeolocationMobileApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
#If EASYTEST Then
			e.Updater.Update()
			e.Handled = True
#Else
			If System.Diagnostics.Debugger.IsAttached Then
				e.Updater.Update()
				e.Handled = True
			Else
				Dim message As String = "The application cannot connect to the specified database, " & "because the database doesn't exist, its version is older " & "than that of the application or its schema does not match " & "the ORM data model structure. To avoid this error, use one " & "of the solutions from the https://www.devexpress.com/kb=T367835 KB Article."

				If e.CompatibilityError IsNot Nothing AndAlso e.CompatibilityError.Exception IsNot Nothing Then
					message &= ControlChars.CrLf & ControlChars.CrLf & "Inner exception: " & e.CompatibilityError.Exception.Message
				End If
				Throw New InvalidOperationException(message)
			End If
#End If
		End Sub
		Private Sub InitializeComponent()
			Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
			Me.module2 = New DevExpress.ExpressApp.Mobile.SystemModule.SystemMobileModule()
			Me.module3 = New BackgroundGeolocation.Module.BackgroundGeolocationModule()
			Me.module4 = New BackgroundGeolocation.Module.Mobile.BackgroundGeolocationMobileModule()
			Me.securityModule1 = New DevExpress.ExpressApp.Security.SecurityModule()
			Me.mapsMobileModule = New DevExpress.ExpressApp.Maps.Mobile.MapsMobileModule()
			DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
			' 
			' BackgroundGeolocationMobileApplication
			' 
			Me.ApplicationName = "BackgroundGeolocation"
			Me.Modules.Add(Me.module1)
			Me.Modules.Add(Me.module2)
			Me.Modules.Add(Me.module3)
			Me.Modules.Add(Me.module4)
			Me.Modules.Add(Me.securityModule1)
			Me.Modules.Add(Me.mapsMobileModule)
'			Me.DatabaseVersionMismatch += New System.EventHandler(Of DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)(Me.BackgroundGeolocationMobileApplication_DatabaseVersionMismatch)
			DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()

		End Sub
	End Class
End Namespace
