using System;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace BackgroundGeolocation.Module.BusinessObjects {
    [DefaultClassOptions]
    public class DeviceInfo : BaseObject {
        public DeviceInfo(Session session) : base(session) { }

        private string name;
        public string Name {
            get { return name; }
            set { SetPropertyValue("Name", ref name, value); }
        }

        private string uuid;
        public string Uuid {
            get { return uuid; }
            set { SetPropertyValue("Uuid", ref uuid, value); }
        }

        private string model;
        public string Model {
            get { return model; }
            set { SetPropertyValue("Model", ref model, value); }
        }

        private string platform;
        public string Platform {
            get { return platform; }
            set { SetPropertyValue("Platform", ref platform, value); }
        }

        private string version;
        public string Version {
            get { return version; }
            set { SetPropertyValue("Version", ref version, value); }
        }

        private string manufacturer;
        public string Manufacturer {
            get { return manufacturer; }
            set { SetPropertyValue("Manufacturer", ref manufacturer, value); }
        }

        [Association("DeviceLocation-Locations")]
        public XPCollection<DeviceLocation> Locations {
            get { return GetCollection<DeviceLocation>("Locations"); }
        }
    }

    //[DefaultClassOptions]
    public class DeviceLocation : BaseObject, IMapsMarker {
        public DeviceLocation(Session session) : base(session) { }

        private DeviceInfo deviceInfo;
        [Association("DeviceLocation-Locations")]
        public DeviceInfo DeviceInfo {
            get { return deviceInfo; }
            set { SetPropertyValue("DeviceInfo", ref deviceInfo, value); }
        }

        private DateTime timeStamp;
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy HH:mm:ss}")]
        public DateTime TimeStamp {
            get { return timeStamp; }
            set { SetPropertyValue("TimeStamp", ref timeStamp, value); }
        }

        public string Title {
            get {
                return String.Format("Time: {0}<br>Accuracy: {1}<br>Speed: {2}<br>Activity: {3}", TimeStamp, Accuracy, Speed, ActivityType);
            }
        }

        private double latitude;
        public double Latitude {
            get { return latitude; }
            set { SetPropertyValue("Latitude", ref latitude, value); }
        }

        private double longitude;
        public double Longitude {
            get { return longitude; }
            set { SetPropertyValue("Longitude", ref longitude, value); }
        }

        private double accuracy;
        public double Accuracy {
            get { return accuracy; }
            set { SetPropertyValue("Accuracy", ref accuracy, value); }
        }

        private double speed;
        public double Speed {
            get { return speed; }
            set { SetPropertyValue("Speed", ref speed, value); }
        }

        private string activityType;
        public string ActivityType {
            get { return activityType; }
            set { SetPropertyValue("ActivityType", ref activityType, value); }
        }
    }
}
