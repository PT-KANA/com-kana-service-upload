﻿using System;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IBankDocumentNumberGenerator
    {
        Task<string> GenerateDocumentNumber(string Type, string BankCode, string Username);
        Task<string> GenerateDocumentNumber(string Type, string BankCode, string Username, DateTime Date);
    }
}
