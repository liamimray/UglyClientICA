﻿using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var client = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:5077/") };
        const string apiKey = "API_KEY_CLIENT_3";
        client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

        // Device counts
        int fanCount = 3, heaterCount = 3, sensorCount = 3;

        var httpClientAdapter = new SystemHttpClient(client);
        var factory = new HttpDeviceFactory(httpClientAdapter);

        var controller = new DeviceController(factory, fanCount, heaterCount, sensorCount);

        while (true)
        {
            Console.WriteLine("Simulation Control:");
            Console.WriteLine("1. Control Fan");
            Console.WriteLine("2. Control Heater");
            Console.WriteLine("3. Read Temperature");
            Console.WriteLine("4. Display State of All Devices");
            Console.WriteLine("X. Exit");
            Console.Write("Select an option: ");
            var input = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (input)
                {
                    case "1":
                        Console.Write("Enter Fan Number: ");
                        if (int.TryParse(Console.ReadLine(), out int fanId))
                        {
                            Console.Write("Turn Fan On or Off? (on/off): ");
                            var stateInput = Console.ReadLine();
                            bool isOn = stateInput?.ToLower() == "on";
                            await controller.SetFanState(fanId, isOn);
                            Console.WriteLine($"Fan {fanId} has been turned {(isOn ? "On" : "Off")}.");
                        }
                        break;

                    case "2":
                        Console.Write("Enter Heater Number: ");
                        if (int.TryParse(Console.ReadLine(), out int heaterId))
                        {
                            Console.Write("Set Heater Level (0-5): ");
                            if (int.TryParse(Console.ReadLine(), out int level) && level >= 0 && level <= 5)
                            {
                                await controller.SetHeaterLevel(heaterId, level);
                                Console.WriteLine($"Heater {heaterId} level set to {level}.");
                            }
                        }
                        break;

                    case "3":
                        Console.Write("Enter Sensor Number: ");
                        if (int.TryParse(Console.ReadLine(), out int sensorId))
                        {
                            double temp = await controller.GetSensorTemperature(sensorId);
                            Console.WriteLine($"Sensor {sensorId} Temperature: {temp:F1}°C");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Sensor Number.");
                        }
                        break;

                    case "4":
                        Console.WriteLine("Fetching state of all devices...");
                        await controller.ShowDeviceStates();
                        break;

                    case "X":
                    case "x":
                        Console.WriteLine("Exiting program...");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine(); // For improved readability between operations
        }
    }
}