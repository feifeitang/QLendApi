namespace QLendApi.Settings
{
    public class PushTemplates
    {
        public class Generic
        {
            public const string Android = "{ \"notification\": { \"title\" : \"$(alertTitle)\", \"body\" : \"$(alertMessage)\"}, \"data\" : { \"action\" : \"$(alertAction)\" } }";
        }

        public class Silent
        {
            public const string Android = "{ \"data\" : {\"message\" : \"$(alertMessage)\", \"action\" : \"$(alertAction)\"} }";
        }
    }
}