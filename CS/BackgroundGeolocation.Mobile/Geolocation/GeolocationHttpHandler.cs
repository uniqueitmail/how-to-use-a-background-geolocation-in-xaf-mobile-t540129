using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using BackgroundGeolocation.Module.BusinessObjects;
using BackgroundGeolocation.Module.Mobile.Services;
using Newtonsoft.Json;

namespace BackgroundGeolocation.Mobile {
    public class GeolocationHttpHandler : IHttpHandler {
        public void ProcessRequest(HttpContext context) {
            AddLocation(context.Request.InputStream);            
        }

        public void AddLocation(Stream input) {
            try {
                GeolocationJsonObject geolocationJsonObject = ParseGeolocationObjects(input);
                Device device = geolocationJsonObject.Device;
                Location location = geolocationJsonObject.Location;
                if(geolocationJsonObject != null) {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(DeviceInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(DeviceLocation));
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    using(XPObjectSpaceProvider objectSpaceProvider = new XPObjectSpaceProvider(new ConnectionStringDataStoreProvider(connectionString), true)) {
                        using(IObjectSpace objectSpace = objectSpaceProvider.CreateObjectSpace()) {
                            DeviceInfo deviceInfo = objectSpace.FindObject<DeviceInfo>(CriteriaOperator.Parse("Uuid=?", geolocationJsonObject.Device.Uuid));
                            if(deviceInfo == null) {
                                deviceInfo = objectSpace.CreateObject<DeviceInfo>();
                                deviceInfo.Name = device.Uuid;
                            }

                            deviceInfo.Manufacturer = device.Manufacturer;
                            deviceInfo.Model = device.Model;
                            deviceInfo.Platform = device.Platform;
                            deviceInfo.Version = device.Version;
                            deviceInfo.Uuid = device.Uuid;

                            DeviceLocation deviceLocation = objectSpace.CreateObject<DeviceLocation>();
                            deviceLocation.DeviceInfo = deviceInfo;
                            deviceLocation.Accuracy = location.Coords.Accuracy;
                            deviceLocation.Latitude = location.Coords.Latitude;
                            deviceLocation.Longitude = location.Coords.Longitude;
                            deviceLocation.Speed = location.Coords.Speed;
                            deviceLocation.ActivityType = location.Activity.Type;
                            deviceLocation.TimeStamp = DateTime.ParseExact(location.Timestamp, "yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);

                            deviceInfo.Locations.Add(deviceLocation);
                            objectSpace.CommitChanges();
                        }
                    }
                }
            } catch(Exception ex) {
                string message = "Cannot build a mobile application due to the following error: " + ex.Message;
                Tracing.Tracer.LogError(message);
                //throw new WebFaultException<string>(message, HttpStatusCode.InternalServerError);
                throw new Exception(message);
            }
        }


        private GeolocationJsonObject ParseGeolocationObjects(Stream input) {
            var serializer = new JsonSerializer();
            TextReader reader = new StreamReader(input);
            return serializer.Deserialize(reader, typeof(GeolocationJsonObject)) as GeolocationJsonObject;
        }

        public bool IsReusable { get { return true; } }
    }
}