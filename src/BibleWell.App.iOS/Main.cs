namespace BibleWell.App.iOS;

public class Application
{
    // This is the main entry point of the application.
    public static void Main(string[] args)
    {
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, principalClass: null, typeof(AppDelegate));
    }
}