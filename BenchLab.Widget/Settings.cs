﻿using BenchLab.Widget.Base;
using BenchLab.Widget.Weather;

namespace BenchLab.Widget
{
    public class Settings : XmlSerializable
    {
        public Settings()
        {
            TempScale = TemperatureScale.Celsius;
            RefreshInterval = 20;
            ShowFeelsLike = false;
            EnableVideoBackground = true;
        }

        public string LocationCode { get; set; } 
        public double RefreshInterval { get; set; }
        public TemperatureScale TempScale { get; set; }
        public bool ShowFeelsLike { get; set; }
        public bool EnableVideoBackground { get; set; }
    }
}
