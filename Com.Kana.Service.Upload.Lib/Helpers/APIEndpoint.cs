using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Helpers
{
    public static class APIEndpoint
    {
        public static string Purchasing { get; set; }
        public static string Core { get; set; }
        public static string Inventory { get; set; }
		public static string ConnectionString { get; set; }
        public static string Finance { get; set; }
		public static string CustomsReport { get; set; }
		public static string Sales { get; set; }
        public static string Auth { get; set; }
        public static string GarmentProduction { get; set; }
        public static string PackingInventory { get; set; }
        public static string Upload { get; set; }
        public static string Accurate { get; set; }
    }

    public static class AuthCredential
    {
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string ClientId { get; set; }
        public static string ClientSecret { get; set; }
        public static string Scope { get; set; }
        public static string AccessToken { get; set; }
    }
}
