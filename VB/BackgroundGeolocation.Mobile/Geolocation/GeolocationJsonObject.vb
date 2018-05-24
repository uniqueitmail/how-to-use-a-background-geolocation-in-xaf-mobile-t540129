Imports Newtonsoft.Json

Namespace BackgroundGeolocation.Module.Mobile.Services
	Public Class Coords
		<JsonProperty("latitude")>
		Public Property Latitude() As Double

		<JsonProperty("longitude")>
		Public Property Longitude() As Double

		<JsonProperty("accuracy")>
		Public Property Accuracy() As Double

		<JsonProperty("speed")>
		Public Property Speed() As Double

		<JsonProperty("heading")>
		Public Property Heading() As Double

		<JsonProperty("altitude")>
		Public Property Altitude() As Double
	End Class

	Public Class Activity

		<JsonProperty("type")>
		Public Property Type() As String

		<JsonProperty("confidence")>
		Public Property Confidence() As Integer
	End Class

	Public Class Battery

		<JsonProperty("is_charging")>
		Public Property IsCharging() As Boolean

		<JsonProperty("level")>
		Public Property Level() As Double
	End Class

	Public Class Extras
	End Class

	Public Class Location

		<JsonProperty("event")>
		Public Property [Event]() As String

		<JsonProperty("is_moving")>
		Public Property IsMoving() As Boolean

		<JsonProperty("uuid")>
		Public Property Uuid() As String

		<JsonProperty("timestamp")>
		Public Property Timestamp() As String

		<JsonProperty("odometer")>
		Public Property Odometer() As Double

		<JsonProperty("coords")>
		Public Property Coords() As Coords

		<JsonProperty("activity")>
		Public Property Activity() As Activity

		<JsonProperty("battery")>
		Public Property Battery() As Battery

		<JsonProperty("extras")>
		Public Property Extras() As Extras
	End Class

	Public Class Device

		<JsonProperty("model")>
		Public Property Model() As String

		<JsonProperty("platform")>
		Public Property Platform() As String

		<JsonProperty("uuid")>
		Public Property Uuid() As String

		<JsonProperty("version")>
		Public Property Version() As String

		<JsonProperty("manufacturer")>
		Public Property Manufacturer() As String

		<JsonProperty("framework")>
		Public Property Framework() As String
	End Class

	Public Class GeolocationJsonObject

		<JsonProperty("location")>
		Public Property Location() As Location

		<JsonProperty("device")>
		Public Property Device() As Device
	End Class
End Namespace
