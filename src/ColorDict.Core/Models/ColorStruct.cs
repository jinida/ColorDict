﻿using System.Drawing;

namespace ColorDict.Core.Models
{
    public struct ColorStruct
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public byte Alpha { get; set; }

        public ColorStruct(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
        public ColorStruct(int red, int green, int blue, int alpha)
        {
            Red = (byte)red;
            Green = (byte)green;
            Blue = (byte)blue;
            Alpha = (byte)alpha;
        }

        public ColorStruct(Color color)
        {
            Red = color.R;
            Green = color.G;
            Blue = color.B;
            Alpha = color.A;
        }

        public ColorStruct SetAdd(int value)
        {
            Red = (byte)(Red + value);
            Green = (byte)(Green + value);
            Blue = (byte)(Blue + value);
            return this;
        }
    }
}
