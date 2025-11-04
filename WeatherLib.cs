using System;

namespace WeatherUtils
{
    public class WeatherLib
    {
        // Temperatur in °C, relative Luftfeuchtigkeit in %
        public double Temperature { get; set; }
        public double Humidity { get; set; }

        // Optional: Konsolen-Eingabe beibehalten
        public void Eingabe()
        {
            Console.Write("Temperatur in °C: ");
            if (double.TryParse(Console.ReadLine(), out var t))
            {
                Temperature = t;
                Console.Write("Luftfeuchtigkeit in %: ");
                if (double.TryParse(Console.ReadLine(), out var rh))
                {
                    Humidity = rh;
                }
                else
                {
                    Console.WriteLine("Ungültige Eingabe für Luftfeuchtigkeit.");
                }
            }
            else
            {
                Console.WriteLine("Ungültige Eingabe für Temperatur.");
            }
        }

        // Taupunkt (°C) nach Magnus-Formel
        public double GetDewPoint()
        {
            // RH begrenzen, um ln(0) zu vermeiden
            var rh = Math.Clamp(Humidity, 0.1, 100.0);

            const double a = 17.27;
            const double b = 237.7;
            double gamma = (a * Temperature) / (b + Temperature) + Math.Log(rh / 100.0);
            return (b * gamma) / (a - gamma);
        }

        // Windchill (gefühlte Temperatur in °C) nach kanadischer Formel
        // windSpeedMs: Windgeschwindigkeit in m/s
        public double GetWindChill(double windSpeedMs)
        {
            double t = Temperature;
            double vKmh = Math.Max(0.0, windSpeedMs) * 3.6;

            // Gültigkeitsbereich: T <= 10°C und v > 4.8 km/h
            if (t <= 10.0 && vKmh > 4.8)
            {
                double v016 = Math.Pow(vKmh, 0.16);
                double wci = 13.12 + 0.6215 * t - 11.37 * v016 + 0.3965 * t * v016;
                return wci;
            }

            // Außerhalb des Bereichs: unveränderte Lufttemperatur
            return t;
        }

        // Heat Index (gefühlte Temperatur in °C) nach Rothfusz-Regression
        // Gültig primär für T >= ~27°C und RH >= ~40%
        public double GetHeatIndex()
        {
            double rh = Math.Clamp(Humidity, 0.0, 100.0);
            double tC = Temperature;

            // In Fahrenheit umrechnen
            double tF = tC * 9.0 / 5.0 + 32.0;

            // Rothfusz-Formel (Fahrenheit)
            double hiF =
                -42.379 +
                2.04901523 * tF +
                10.14333127 * rh -
                0.22475541 * tF * rh -
                0.00683783 * tF * tF -
                0.05481717 * rh * rh +
                0.00122874 * tF * tF * rh +
                0.00085282 * tF * rh * rh -
                0.00000199 * tF * tF * rh * rh;

            // Anpassungen gemäß NOAA
            if (rh < 13 && tF >= 80 && tF <= 112)
            {
                hiF -= ((13 - rh) / 4.0) * Math.Sqrt((17 - Math.Abs(tF - 95)) / 17.0);
            }
            else if (rh > 85 && tF >= 80 && tF <= 87)
            {
                hiF += ((rh - 85) / 10.0) * ((87 - tF) / 5.0);
            }

            // Zurück nach Celsius
            double hiC = (hiF - 32.0) * 5.0 / 9.0;

            // Außerhalb des sinnvollen Bereichs kann HI der Lufttemperatur ähneln
            // Hier kein hartes Override, aber numerisch stabile Ausgabe
            return hiC;
        }

        // Wolkenbasis (m über Grund) näherungsweise:
        // Höhe ? 125 m * (Temperatur - Taupunkt)
        public double ErmittleWolkenhoehe()
        {
            double td = GetDewPoint();
            double spread = Math.Max(0.0, Temperature - td);
            return 125.0 * spread; // Meter
        }
    }
}