using SmartHomeController;
using System.Net.NetworkInformation;

public class Program
{
    private static List<SmartDevice> devices = new List<SmartDevice>();

    static string desintationFilePath;
    public static void Main()
    {
        string folder = "Data";
        string filename = "smartdevices.csv";
        desintationFilePath = CopyDataToWorkingDir(folder, filename);
        LoadSmartDevices(desintationFilePath);

    }

    public static string CopyDataToWorkingDir(string folder, string filename)
    {
        // define the source and destination paths
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string sourceFilePath = Path.Combine(projectDirectory, folder, filename);
        string destinationFilePath = Path.Combine(Environment.CurrentDirectory, filename);
        if (File.Exists(sourceFilePath))
        {
            File.Copy(sourceFilePath, destinationFilePath, true);
        }
        else
        {
            Console.WriteLine("Source file not found:" + sourceFilePath);

        }
        return destinationFilePath;
    }

    public static void LoadSmartDevices(string destinationFilePath)
    {
        using (var reader = new StreamReader(destinationFilePath))
        {
            // skip header line in csv
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                if (values[3].Length == 0)
                {
                    values[3] = "0";
                }
                if (values[4].Length == 0)
                {
                    values[4] = "0";
                }
                if (values[6].Length == 0)
                {
                    values[6] = "0";
                }
                if (values[7].Length == 0)
                {
                    values[7] = "0";
                }
                if (values[8].Length == 0)
                {
                    values[8] = "0";
                }

                // read each value into the relevant variable
                // this contains the info for each row, one at a time
                int deviceID = int.Parse(values[0]);
                string deviceType = values[1];
                string deviceName = values[2];
                double brightness = Convert.ToDouble(values[3]);
                string colour = values[4];
                string cameraResolution = values[5];
                double currentTemperature = Convert.ToDouble(values[6]);
                double targetTemperature = Convert.ToDouble(values[7]);
                int speakerVolume = Convert.ToInt32(values[8]);

                SmartDevice device = null;

                switch (deviceType)
                {
                    case "SmartLight":
                        device = new SmartLight(deviceID, deviceName, brightness, colour);
                        break;
                    case "SmartSecurityCamera":
                        device = new SmartSecurityCamera(deviceID, deviceName, cameraResolution);
                        break;
                    case "SmartThermostat":
                        device = new SmartThermostat(deviceID, deviceName, currentTemperature, targetTemperature);
                        break;
                    case "SmartSpeaker":
                        device = new SmartSpeaker(deviceID, deviceName, speakerVolume);
                        break;
                }

                if (device != null)
                {
                    devices.Add(device);
                    device.GetStatus();
                }
            }
        }
    }
}
     


