﻿using Com.Kana.Service.Upload.Lib.ViewModels.MonitoringUnitReceiptAllViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
	public interface IMonitoringUnitReceiptAllFacade
	{
		MemoryStream GenerateExcel(string no, string refNo, string roNo, string doNo, string unit, string supplier, DateTime? dateFrom, DateTime? dateTo, int page, int size, string Order, int offset);
		Tuple<List<MonitoringUnitReceiptAll>, int> GetReport(string no, string refNo, string roNo, string doNo, string unit, string supplier, DateTime? dateFrom, DateTime? dateTo, int page, int size, string Order, int offset);

	}
}
