# How to use a Background Geolocation in XAF Mobile


This example demonstrates how to take advantage of a mobile device positioning system in your XAF mobile application using the <a href="https://github.com/transistorsoft/cordova-background-geolocation-lt">Cordova Background Geolocation plugin</a> and <a href="https://github.com/apache/cordova-plugin-device">Cordova Device plugin</a> . Additionally, you will learn how to access <a href="http://docs.phonegap.com/phonegap-build/configuring/plugins/">the PhoneGap config file</a> (regular XML file) and add the plugin to your application.<br>
<p> </p>
<strong>Scenario:</strong><br>You want to automatically track movements of an XAF mobile application user and display the user's route on the map, e.g., in a separate administrative XAF Web application:<br><br><img src="https://raw.githubusercontent.com/DevExpress-Examples/how-to-use-a-background-geolocation-in-xaf-mobile-t540129/17.1.6+/media/19639a2b-ca69-41a0-aab8-1cb76761d636.png"><br><br><br><strong>Your feedback is needed!</strong><br>This is not an official feature of our Mobile UI (CTP) and our API may change in future release cycles. We are publishing this article prior to the 17.2 release to collect early user feedback and improve overall functionality. We would appreciate your thoughts on this feature once you've had the opportunity to review it. Please report any issues, missing capabilities or suggestions in <a href="https://www.devexpress.com/Support/Center/Question/Create">separate tickets in our Support Center</a>. Thanks for your help in advance!<br><br><strong>Prerequisites<br></strong>Install any <strong>v17.1.5+</strong> version.<br>The implementation steps below are given for a new application, though they can be applied to any other XAF mobile app.<br><br><strong>Steps to implement<br>1. </strong>Create a new XAF solution with Web and Mobile platforms and enable the <a href="https://documentation.devexpress.com/eXpressAppFramework/114776/Concepts/Extra-Modules/Maps/Maps-Module">Maps Module</a> in it.  Do not forget to specify the <a href="https://documentation.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Maps.Web.MapsAspNetModule.class">MapsAspNetModule</a> > <a href="https://documentation.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Maps.Web.MapsAspNetModule.GoogleApiKey.property">GoogleApiKey</a> property in the Application Designer invoked for the <em>YourSolutionName.Web/WebApplication.xx</em> file.<strong><br></strong><br><strong>2.</strong> In the <em>YourSoltutionName.Mobile/MobileApplication.xx</em> file, register the <a href="https://github.com/transistorsoft/cordova-background-geolocation-lt">Cordova Background Geolocation</a> plugin by adding required tags to the <a href="https://documentation.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Mobile.MobileApplication.AdditionalPhoneGapPlugins.property">MobileApplication.AdditionalPhoneGapPlugins</a> collection.<br>


```cs
using DevExpress.ExpressApp.Mobile

public partial class BackgroundGeolocationMobileApplication : MobileApplication {
    public BackgroundGeolocationMobileApplication() {
        AdditionalPhoneGapPlugins.Add(@"<plugin name=""cordova-background-geolocation-lt"" source=""npm"" spec=""2.7.3"">
			<variable name=""LOCATION_ALWAYS_USAGE_DESCRIPTION"" value=""Background location-tracking is required"" />
			<variable name=""LOCATION_WHEN_IN_USE_USAGE_DESCRIPTION"" value=""Background location-tracking is required"" />
			<variable name=""MOTION_USAGE_DESCRIPTION"" value=""Using the accelerometer increases battery-efficiency by intelligently toggling location-tracking only when the device is detected to be moving"" />
			<variable name=""LICENSE"" value=""YOUR_LICENSE_KEY"" />
			</plugin>");
        AdditionalPhoneGapPlugins.Add(@"<plugin name=""cordova-plugin-device"" source=""npm"" spec=""1.1.6""></plugin>");
    // ...
```




```vb
Imports Microsoft.VisualBasic
Imports DevExpress.ExpressApp.Mobile

Partial Public Class BackgroundGeolocationMobileApplication
    Inherits MobileApplication
    Public Sub New()
        AdditionalPhoneGapPlugins.Add("<plugin name=""cordova-background-geolocation-lt"" source=""npm"" spec=""2.7.3"">" & ControlChars.CrLf &
"    <variable name=""LOCATION_ALWAYS_USAGE_DESCRIPTION"" value=""Background location-tracking is required"" />" & ControlChars.CrLf &
"    <variable name=""LOCATION_WHEN_IN_USE_USAGE_DESCRIPTION"" value=""Background location-tracking is required"" />" & ControlChars.CrLf &
"    <variable name=""MOTION_USAGE_DESCRIPTION"" value=""Using the accelerometer increases battery-efficiency by intelligently toggling location-tracking only when the device is detected to be moving"" />" & ControlChars.CrLf &
"    <variable name=""LICENSE"" value=""YOUR_LICENSE_KEY"" />" & ControlChars.CrLf &
"    </plugin>")
        AdditionalPhoneGapPlugins.Add("<plugin name=""cordova-plugin-device"" source=""npm"" spec=""1.1.6""></plugin>")
    ' ...
```


Take note of the line which adds the "LICENSE" tag. If you have a license key (refer to the corresponding remark in the <a href="https://github.com/transistorsoft/cordova-background-geolocation-lt/blob/master/README.md">README</a> file), uncomment this code and replace the YOUR_LICENSE_KEY placeholder with your own key .<br><strong><br>3.</strong> In the <em>YourSolutionName.Module</em> project, copy the <em>BackgroundGeolocation.Module\BusinessObjects\DeviceInfo.xx</em> file to the <em>BusinessObjects </em>folder. <br>This file contains business classes used to store background geolocation data received from mobile clients. You may want to put these classes into separate files according to your code rules.<br><br><strong>4.</strong> In the <em>YourSolutionName.Mobile</em> project, create a new <em>Geolocation </em>folder and copy the several code files below into it as per the instructions below.<br><strong>4.1.</strong> Copy the <em>BackgroundGeolocation.Mobile\Geolocation\GeolocationScript.js</em> file and include it in the project. Change the <em>Build Action</em> property for this file to <em>Embedded Resource</em>. This code initializes the Cordova Background Geolocation plugin with default settings. Feel free to modify it according to your needs. More information about configuring options can be found in the <a href="https://github.com/transistorsoft/cordova-background-geolocation-lt/blob/master/docs/README.md#wrench-configuration-options">project's github repository</a>. <br><br><strong>4.2.</strong> Copy the <em>BackgroundGeolocation.Mobile\Geolocation\GeolocationJsonObject.xx</em> file and include it in the project.<br>These classes are used to deserialize JSON data set by mobile applications to the Geolocation Service.<strong><br><br>4.3. </strong>Copy the <em>BackgroundGeolocation.Mobile\Geolocation\GeolocationHttpHandler.xx</em> file and include it in the project. <br>The Background Geolocation plugin will send data to this service. The service is intended to save the device information to the database. It uses the connection string from the application configuration file (Web.config).<br><br>To enable the HTTP handler added on the previous step, add the following line to the <strong>configuration/system.webServer/handlers</strong> section of the <em>YourSolutionName</em><em>.Mobile/Web.config </em>file (you may need to change the <strong>type</strong> attribute value and specify the namespace qualified name of the <em>GeolocationHttpHandler</em> class declared in your project:<strong><strong><br><br></strong></strong>


```xml
<add name="Geolocation" verb="GET,POST" path="GeolocationProcessingService.svc" type="YourSolutionName.Mobile.GeolocationHttpHandler" />
```


<strong><br></strong><strong>5.  </strong>In the <em>YourSoltutionName.Mobile/MobileApplication.xx</em> file, register the <em>GeolocationScript.js code</em> (copied on step #4.1) using the <a href="https://documentation.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Mobile.MobileApplication.RegisterClientScriptOnApplicationStart.method"><em>MobileApplication.RegisterClientScriptOnApplicationStart</em></a> method so that this script executes when the mobile application starts up on the device. The code snippet below demonstrates how to implement the ReadResource and ReadResourceString methods required to load the code from the embedded resource into a String variable (you can find this code in the <em>BackgroundGeolocation.Mobile/MobileApplication.xx</em> file of the sample project): <br>


```cs
public BackgroundGeolocationMobileApplication() {
    // ...
    string geolocationScript = ReadResourceString("BackgroundGeolocation.Mobile.Geolocation.GeolocationScript.js");
    RegisterClientScriptOnApplicationStart("GeolocationScript", geolocationScript);
}


public static byte[] ReadResource(string resourceName) {
    byte[] buffer = null;
    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)) {
        if (stream != null) {
            buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
        }
    }
    return buffer;
}
public static string ReadResourceString(string resourceName) {
    byte[] resourceBytes = ReadResource(resourceName);
    //You need to escape CRLF characters in versions prior to version 18.1
    //return Encoding.UTF8.GetString(resourceBytes).Replace("\r\n", "\\r\\n");
    return Encoding.UTF8.GetString(resourceBytes);
}
```




```vb
Public Sub New()
	' ...
	Dim geolocationScript As String = ReadResourceString("GeolocationScript.js")
	RegisterClientScriptOnApplicationStart("GeolocationScript", geolocationScript)
End Sub

Public Shared Function ReadResource(ByVal resourceName As String) As Byte()
	Dim buffer() As Byte = Nothing
	Using stream As Stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
		If stream IsNot Nothing Then
			buffer = New Byte(stream.Length - 1){}
			stream.Read(buffer, 0, CInt(stream.Length))
		End If
	End Using
	Return buffer
End Function

Public Shared Function ReadResourceString(ByVal resourceName As String) As String
	Dim resourceBytes() As Byte = ReadResource(resourceName)
        'You need to escape CRLF characters in versions prior to version 18.1
        'Return Encoding.UTF8.GetString(resourceBytes).Replace(ControlChars.CrLf, "\r\n")
        Return Encoding.UTF8.GetString(resourceBytes)
End Function
```


<p>The value passed to the ReadResourceString method consists of two parts in C# projects: the default assembly namespace ("BackgroundGeolocation.Mobile") and the path to the resource file ("Geolocation.GeolocationScript.js"). The first part may be different in your project. In VB.NET projects, the resource name will be much simpler: "GeolocationScript.js". <br><strong id="tinymce" class="mce-content-body "><strong><br></strong></strong><strong>6.</strong> In the <em>YourSolutionName.Module.Web</em> project, install the <a href="https://www.nuget.org/packages/Newtonsoft.Json/8.0.3">Newtonsoft.Json NuGet package</a> and copy the <em>BackgroundGeolocation.Module.Web\Controllers\DisplayGeolocationController.xx</em> file to the <em>Controllers </em>folder.<br>This controller is intended to draw the client's route based on location points.<strong><br></strong><br><strong>7. </strong>Build and deploy your mobile application following the steps described in the <a href="https://documentation.devexpress.com/eXpressAppFramework/CustomDocument116434.aspx">Install the Application to a Smartphone</a>  help article, and ensure that the Geolocation services are enabled in the mobile device settings.<br>Once you get your mobile app running on your actual device, wait a few minutes and then run the Web version of your XAF application. Open the <em>DeviceInfo </em>List View, and you will see a record containing your mobile device information. If you click the ListView record, you will be navigated to the DetailView that contains the Map demonstrating the route tracked by the Background Geolocation module.<br><br><strong>See Also:<br></strong><a href="https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112670.aspx">eXpressApp Framework</a> > <a href="https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113577.aspx">Getting Started</a> > <a href="https://documentation.devexpress.com/eXpressAppFramework/CustomDocument116359.aspx">XAF Mobile (CTP) Tutorial</a><a href="https://www.youtube.com/watch?v=BZKcpcSo5bQ"><br></a><a href="https://documentation.devexpress.com/eXpressAppFramework/119193/Task-Based-Help/Miscellaneous-UI-Customizations/How-to-Use-a-Custom-Plugin-in-a-Mobile-Application">How to: Use a Custom Plugin in a Mobile Application</a></p>
<p><a href="https://www.youtube.com/watch?v=BZKcpcSo5bQ">XAF Mobile - Overview Video</a><br><a href="https://www.devexpress.com/Support/Center/p/T356939">FAQ: New XAF HTML5/JavaScript mobile UI (CTP)</a><br><a href="https://documentation.devexpress.com/eXpressAppFramework/118503/Getting-Started/XAF-Mobile-CTP-Tutorial/Using-Maps-in-a-Mobile-Application">Using Maps in a Mobile Application</a> <br><a href="https://www.devexpress.com/Support/Center/p/T533438">How to send push notifications to the XAF Mobile application using Azure Notifications Hub</a><br><a href="https://www.devexpress.com/Support/Center/p/T530459">How to use a Barcode Scanner in XAF Mobile</a></p>

<br/>


