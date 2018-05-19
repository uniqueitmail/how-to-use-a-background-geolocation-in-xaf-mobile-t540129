Imports System
Imports DevExpress.ExpressApp.Model
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo

Namespace BackgroundGeolocation.Module.BusinessObjects
	<DefaultClassOptions>
	Public Class DeviceInfo
		Inherits BaseObject

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub

        Private fName As String
        Public Property Name() As String
            Get
                Return fName
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Name", fName, value)
            End Set
		End Property

        Private fUuid As String
        Public Property Uuid() As String
            Get
                Return fUuid
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Uuid", fUuid, value)
            End Set
		End Property

        Private fModel As String
        Public Property Model() As String
            Get
                Return fModel
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Model", fModel, value)
            End Set
		End Property

        Private fPlatform As String
        Public Property Platform() As String
            Get
                Return fPlatform
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Platform", fPlatform, value)
            End Set
		End Property

        Private fVersion As String
        Public Property Version() As String
            Get
                Return fVersion
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Version", fVersion, value)
            End Set
		End Property

        Private fManufacturer As String
        Public Property Manufacturer() As String
            Get
                Return fManufacturer
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Manufacturer", fManufacturer, value)
            End Set
		End Property

		<Association("DeviceLocation-Locations")>
		Public ReadOnly Property Locations() As XPCollection(Of DeviceLocation)
			Get
				Return GetCollection(Of DeviceLocation)("Locations")
			End Get
		End Property
	End Class

	'[DefaultClassOptions]
	Public Class DeviceLocation
		Inherits BaseObject
		Implements IMapsMarker

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub

        Private fDeviceInfo As DeviceInfo
        <Association("DeviceLocation-Locations")>
        Public Property DeviceInfo() As DeviceInfo
            Get
                Return fDeviceInfo
            End Get
            Set(ByVal value As DeviceInfo)
                SetPropertyValue("DeviceInfo", fDeviceInfo, value)
            End Set
		End Property

        Private fTimeStamp As Date
        <ModelDefault("DisplayFormat", "{0: dd/MM/yyyy HH:mm:ss}")>
        Public Property TimeStamp() As Date
            Get
                Return fTimeStamp
            End Get
            Set(ByVal value As Date)
                SetPropertyValue("TimeStamp", fTimeStamp, value)
            End Set
		End Property

		Public ReadOnly Property Title() As String Implements DevExpress.Persistent.Base.IBaseMapsMarker.Title
			Get
				Return String.Format("Time: {0}<br>Accuracy: {1}<br>Speed: {2}<br>Activity: {3}", TimeStamp, Accuracy, Speed, ActivityType)
			End Get
		End Property

        Private fLatitude As Double
        Private ReadOnly Property IBaseMapsMarker_Latitude() As Double Implements DevExpress.Persistent.Base.IBaseMapsMarker.Latitude
            Get
                Return Latitude
            End Get
        End Property
        Public Property Latitude() As Double
            Get
                Return fLatitude
            End Get
            Set(ByVal value As Double)
                SetPropertyValue("Latitude", fLatitude, value)
            End Set
		End Property

        Private fLongitude As Double
        Private ReadOnly Property IBaseMapsMarker_Longitude() As Double Implements DevExpress.Persistent.Base.IBaseMapsMarker.Longitude
            Get
                Return Longitude
            End Get
        End Property
        Public Property Longitude() As Double
            Get
                Return fLongitude
            End Get
            Set(ByVal value As Double)
                SetPropertyValue("Longitude", fLongitude, value)
            End Set
		End Property

        Private fAccuracy As Double
        Public Property Accuracy() As Double
            Get
                Return fAccuracy
            End Get
            Set(ByVal value As Double)
                SetPropertyValue("Accuracy", fAccuracy, value)
            End Set
		End Property

        Private fSpeed As Double
        Public Property Speed() As Double
            Get
                Return fSpeed
            End Get
            Set(ByVal value As Double)
                SetPropertyValue("Speed", fSpeed, value)
            End Set
		End Property

        Private fActivityType As String
        Public Property ActivityType() As String
            Get
                Return fActivityType
            End Get
            Set(ByVal value As String)
                SetPropertyValue("ActivityType", fActivityType, value)
            End Set
		End Property
	End Class
End Namespace
