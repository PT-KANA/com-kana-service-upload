using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.SalesViewModel;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces.SalesUploadInterface
{
	public interface ISalesUpload
	{
		Task<List<AccuSalesViewModel>> MapToViewModel(List<SalesCsvViewModel> data);
		Tuple<bool, List<object>> UploadValidate(ref List<SalesCsvViewModel> data, List<KeyValuePair<string, StringValues>> list);
		Task UploadData(List<AccuSalesInvoice> data, string username);
		Task<int> Create(List<AccuSalesViewModel> data,string username);
		Task CreateSalesReceipt(List<AccuSalesViewModel> data, string username);
		List<string> CsvHeader { get; }
		Task<List<AccuSalesInvoice>> MapToModel(List<AccuSalesViewModel> data1);
		Tuple<List<AccuSalesInvoice>, int, Dictionary<string, string>> ReadForUpload(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}");
		Tuple<List<AccuSalesInvoice>, int, Dictionary<string, string>> ReadForApproved(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}");


	}
}
