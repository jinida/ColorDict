﻿namespace ColorDict.Core.Models
{
    public class ConfigModel
    {
        public List<ViewOptionModel> ViewOptions { get; set; }
        public string SpoidColor { get; set; } = "#FFFFFFFF";

        public ConfigModel()
        {
            ViewOptions = new List<ViewOptionModel>();
        }
    }
}
