using Newtonsoft.Json;

namespace BackgroundGeolocation.Module.Mobile.Services {
    public class Coords {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("accuracy")]
        public double Accuracy { get; set; }

        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("heading")]
        public double Heading { get; set; }

        [JsonProperty("altitude")]
        public double Altitude { get; set; }
    }

    public class Activity {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("confidence")]
        public int Confidence { get; set; }
    }

    public class Battery {

        [JsonProperty("is_charging")]
        public bool IsCharging { get; set; }

        [JsonProperty("level")]
        public double Level { get; set; }
    }

    public class Extras {
    }

    public class Location {

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("is_moving")]
        public bool IsMoving { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("odometer")]
        public double Odometer { get; set; }

        [JsonProperty("coords")]
        public Coords Coords { get; set; }

        [JsonProperty("activity")]
        public Activity Activity { get; set; }

        [JsonProperty("battery")]
        public Battery Battery { get; set; }

        [JsonProperty("extras")]
        public Extras Extras { get; set; }
    }

    public class Device {

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("framework")]
        public string Framework { get; set; }
    }

    public class GeolocationJsonObject {

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("device")]
        public Device Device { get; set; }
    }
}
