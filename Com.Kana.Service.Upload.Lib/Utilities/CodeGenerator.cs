﻿using Com.Kana.Service.Upload.Lib.Interfaces;
using MlkPwgen;

namespace Com.Kana.Service.Upload.Lib.Utilities
{
    public class CodeGenerator
    {
        private const int _LENGTH = 8;
        private const string _ALLOWED_CHARACTER = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";

        public static string Generate()
        {
            return PasswordGenerator.Generate(length: _LENGTH, allowed: _ALLOWED_CHARACTER);
        }
    }
}
